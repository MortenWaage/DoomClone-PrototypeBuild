using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour, IMovement
{
    public bool IsDead { get; set; } = false;
    Camera cam;

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
        collider = GetComponent<Collider>();
        state = MoveState.Free;

        StartCoroutine("Initialize");
    }

    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.2f);
        ResetPlayer();
    }

    private void Update()
    {
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

            velocity.Normalize();
        }
    }
    #endregion

    #region RAYCASTING_FUNCTIONS

    /*----------------------------*
    *          RAYCASTS           *
    *-----------------------------*/

    bool HitObstacle()
    {
        Vector3 direction = velocity.normalized;
        Vector3 rayStartPosition = UnderPlayer + (Vector3.up * obstacleTraversionHeight);

        bool hitObstacle = (Physics.Raycast(rayStartPosition, direction, obstacleCheckDistance, groundLayer));

        return hitObstacle;
    }

    private void AlignWithGround()
    {
        Vector3 rayCastOrigin = transform.position + velocity.normalized;
        RaycastHit hit;

        float range = collider.bounds.extents.y * boundsExtend;

        bool insideGround = (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, range, groundLayer));

        if (hit.collider != null)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + collider.bounds.extents.y + 0.1f;
            transform.position = newPosition;
        }
        else
            ChangeState(MoveState.Falling);
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
            float distance = collider.bounds.extents.y + 0.1f;

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
    /*-----------------------------*
     *       HELPER FUNCTIONS      *
     *-----------------------------*/


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

        GetComponent<WeaponController>().ResetWeapons();
        GetComponent<PlayerVitals>().ResetVitals();

    }

    #endregion

    #region GIZMOS
    /*-----------------------------*
     *          GIZMOS             *
     *-----------------------------*/


    [SerializeField] [Range(0, 5f)] float boundsExtend = 1; // offsets the distance for the ground check
    void OnDrawGizmos()
    {
        // Quit if in Editor
        if (!Application.isPlaying) return;

        // On Ground
        Gizmos.color = Color.red;

        Vector3 lineStart = transform.position + velocity.normalized;
        Vector3 lineEnd = transform.position + (Vector3.down * collider.bounds.extents.y * boundsExtend);

        Gizmos.DrawLine(lineStart, lineEnd);
    }

    #endregion

    #region DECREPATED_FUNCTIONS
    /*-----------------------------*
     *   DECREPATED FUNCTIONS      *
     *-----------------------------*/


    [SerializeField] [Range(0.1f, 3f)] float groundCheckOffset = 1f;
    [Obsolete("AlignWithGround() - Replaced all other ground check functions")]
    void MoveUpSlope()
    {
        RaycastHit hit;

        Vector3 rayCastOrigin = (transform.position + (Vector3.up * collider.bounds.extents.y)) + (velocity.normalized * groundCheckOffset);

        bool insideGround = (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, (collider.bounds.extents.y * 2), groundLayer));

        if (insideGround)
        {
            Debug.Log("Is Inside Ground");

            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + collider.bounds.extents.y + 0.1f;
            transform.position = newPosition;
        }
    }

    float rayOffset, rayDistance = 0; // Decrepated floats
    [Obsolete("AlignWithGround() - Replaced all other ground check functions")]
    bool OnGround(RayOffset offset)
    {
        Vector3 _offset;
        if (offset == RayOffset.FRONT) _offset = (transform.forward * rayOffset);
        else _offset = -(transform.forward * rayOffset);

        Vector3 _rayPos = transform.position + _offset;
        float distance = rayDistance;

        bool grounded = (Physics.Raycast(_rayPos, Vector3.down, distance, groundLayer));

        return grounded;
    }

    #endregion
}