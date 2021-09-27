using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Movement : MonoBehaviour, IMovement
{
    P_Vitals vitals;
    public bool IsDead { get; set; } = false;
    Camera cam;
    ICamera camController;

    #region PROPERTIES
    enum RayOffset { FRONT, BACK }
    enum MoveState { Free, Stopping, Moving, Falling, OutOfBounds }

    MoveState state;

    [Header("Movement Settings")]
    [SerializeField] [Range(0, 2f)] float accelerationRate = 1f;
    [SerializeField] [Range(0, 0.99f)] float slideMultiplier = 0.85f;
    [SerializeField] [Range(0, 5f)] float slideStrength = 1f;
    [SerializeField] [Range(0, 0.99f)] float stopVelocity = 0.2f;
    [SerializeField] [Range(0, 100f)] float playerSpeed = 15f;
    [SerializeField] [Range(0, 2f)] float turnSpeed = 1f;
    float playerMaxSpeed;

    [Header("Global Settings")]
    [SerializeField] [Range(0, 100f)] float gravity = 0;

    [Header("Raycast Settings")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] [Range(0, 5f)] float obstacleTraversionHeight;
    [SerializeField] [Range(0, 5f)] float obstacleCheckDistance;

    Collider collider;

    public float PlayerSpeed
    {
        get
        {
            if (state == MoveState.Moving)
                return velocity.magnitude;
            else return 0;
        }
        private set { }
    }
    public float Acceleration { get => state == MoveState.Moving ? acceleration : 0; }
    private Vector3 UnderPlayer
    {
        get => transform.position + (Vector3.down * ((collider.bounds.extents.y * 0.5f) + 0.1f));
    }
    float magnitude;

    Vector3 velocity = Vector3.zero;
    float acceleration = 0;

    #endregion

    private void Start()
    {
        GameController.Instance.DoomGuy = gameObject;
        cam = Camera.main;
        vitals = GetComponent<P_Vitals>();
        camController = GetComponent<ICamera>();
        collider = GetComponent<Collider>();
        state = MoveState.Free;
        playerMaxSpeed = playerSpeed;

        StartCoroutine("Initialize");
    }

    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.2f);
        ResetPlayer();
    }

    private void Update()
    {
        if (IsDead) velocity = Vector3.zero;

        switch (state)
        {
            case MoveState.Moving:

                if (!HitObstacle()) AcceleratePlayerMovement();
                else velocity = Vector3.zero;

                AlignWithGround();

                break;

            case MoveState.Stopping:

                if (!HitObstacle()) SlowPlayerMovement();
                else velocity = Vector3.zero;

                AlignWithGround();

                break;

            case MoveState.Falling:

                if (HitObstacle()) velocity = Vector3.zero;

                CheckIfFalling();

                break;

            case MoveState.OutOfBounds:

                break;

            case MoveState.Free:

                if (HitObstacle()) velocity = Vector3.zero;

                CheckIfFalling();

                break;

        }
    }

    #region INPUT_FUNCTIONS
    public void GetInput(Inputs.Directions dir)
    {
        if (dir == Inputs.Directions.Stopping)
        {
            if (state != MoveState.Falling && state != MoveState.OutOfBounds)
                ChangeState(MoveState.Stopping);

            //camController.ResetCameraTilt();
        }
        else
        {
            if (state == MoveState.Falling || state == MoveState.OutOfBounds) return;

            ChangeState(MoveState.Moving);

            if (dir == Inputs.Directions.Forward)
                velocity += transform.forward;

            if (dir == Inputs.Directions.Backwards)
                velocity += -transform.forward;

            if (dir == Inputs.Directions.StrafeLeft)
                velocity += -transform.right;

            if (dir == Inputs.Directions.StrafeRight)
                velocity += transform.right;

            //camController.ApplyCameraTilt(velocity, dir);

            velocity.Normalize();
        }
    }
    #endregion

    #region RAYCASTING_FUNCTIONS

    bool HitObstacle()
    {
        Vector3 direction = velocity.normalized;
        Vector3 rayStartPosition = UnderPlayer + (Vector3.up * obstacleTraversionHeight);
        RaycastHit hit;

        if (!Physics.Raycast(rayStartPosition, direction, out hit, obstacleCheckDistance))
            return false;

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(Layers.Map))
            return true;

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(Layers.Attackable))
            return true;

        return false;
    }
    private void AlignWithGround()
    {
        Vector3 rayCastOrigin = transform.position + velocity.normalized;
        RaycastHit hit;

        float range = collider.bounds.extents.y * 2f;

        if (!Physics.Raycast(rayCastOrigin, Vector3.down, out hit, range))
        {
            ChangeState(MoveState.Falling);
            return;
        }
            

        if (hit.collider.tag == "Elevator")
        {
            if (Vector3.Distance(hit.point, transform.position) > (collider.bounds.extents.y + 0.1f)) return;

            transform.position += Vector3.up * hit.collider.GetComponent<DELETE_LATER_ELEVATOR>().elevatorSpeed * Time.deltaTime;
            return;
        }
        if (hit.collider.tag == "NuclearWaste")
        {
            vitals.ApplyDamageEnvironment(10);
        }
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(Layers.Map))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + collider.bounds.extents.y + 0.1f;
            transform.position = newPosition;
            return;
        }     
    }

    private void CheckIfFalling()
    {
        if (!OnGround())
        {
            if (state != MoveState.Falling) ChangeState(MoveState.Falling);

            ApplyGravity();
        }
        else ChangeState(MoveState.Stopping);

        void ApplyGravity()
        {
            transform.position += Vector3.down * gravity * Time.deltaTime;
            transform.position += velocity * playerSpeed * Time.deltaTime;
        }

        bool OnGround()
        {
            float distance = collider.bounds.extents.y + 0.01f;

            bool grounded = (Physics.Raycast(transform.position, Vector3.down, distance, groundLayer));

            return grounded;
        }
    }

    #endregion

    #region MOVEMENT_FUNCTIONS
    /*----------------------------*
    *          MOVEMENT           *
    *-----------------------------*/

    private void AcceleratePlayerMovement()
    {
        acceleration += accelerationRate * Time.deltaTime;
        if (acceleration > 1) acceleration = 1;

        magnitude = velocity.magnitude; // Stores the magnitude globally rather than grabbing it when moveState is Stopping

        transform.position += velocity * acceleration * playerSpeed * Time.deltaTime;
    }

    private void SlowPlayerMovement()
    {
        acceleration -= accelerationRate * Time.deltaTime;
        if (acceleration < 0) acceleration = 0;

        magnitude -= (magnitude * slideMultiplier) * slideStrength * Time.deltaTime;

        velocity *= magnitude;
        if (velocity.magnitude <= stopVelocity) velocity = Vector3.zero;

        transform.position += velocity * playerSpeed * Time.deltaTime;
    }
    public void TurnPlayer(Inputs.Directions dir, float turnSpeed)
    {
        Vector3 rotation = Vector3.zero;

        if (dir == Inputs.Directions.Right)
            rotation += new Vector3(0, turnSpeed, 0);

        if (dir == Inputs.Directions.Left)
            rotation -= new Vector3(0, turnSpeed, 0);

        /*rotation *= Time.deltaTime * turnSpeed*/;

        transform.Rotate(rotation);
    }

    public void LookPlayer(Inputs.Directions dir, float sensitivity) { }

    #endregion

    #region HELPER_FUNCTIONS
    void ChangeState(MoveState newState)
    {
        if (state != newState)
        {
            state = newState;
        }
    }
    public void ResetPlayer()
    {
        Debug.Log("Resetting Player");

        transform.position = GameController.Instance.SpawnPoint.transform.position;
        transform.rotation = GameController.Instance.SpawnPoint.transform.rotation;
        velocity = Vector3.zero;

        GetComponent<W_Controller>().ResetWeapons();
        GetComponent<P_Vitals>().ResetVitals();
        ResetSpeed();
        GameController.Instance.Interface.UpdateKeys(false, false, false);

    }
    #endregion

    public void CheatSpeed()
    {
        Debug.Log("At warp speed!");
        playerSpeed = 70f;
    }
    public void ResetSpeed()
    {
        playerSpeed = playerMaxSpeed;
    }

    #region DECREPATED_FUNCTIONS
    /*-----------------------------*
     *   DECREPATED FUNCTIONS      *
     *-----------------------------*/


    [SerializeField] [Range(0.1f, 3f)] float groundCheckOffset = 1f;

    #endregion
}