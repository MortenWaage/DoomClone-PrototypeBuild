using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    public enum Directions { Forward, Backwards, Left, Right, Stopping, StrafeLeft, StrafeRight, Up, Down }

    #region Properties
    W_Controller pWeapon;
    P_Movement pMovement;
    P_Inventory pInventory;
    Camera cam;

    [SerializeField] [Range(0, 150f)] float mouseSensitivity = 1f;

    bool isInitialized = false;
    public bool IsDead { get => pMovement.IsDead; set { } }
    float cameraLookUp = 0;
    float cameraDeadDownSpeed = 3f;
    float initialCamHeight = 2f;
    
    IMovement playerMovement;

    #endregion

    IEnumerator Initiate()
    {
        yield return new WaitForEndOfFrame();

        playerMovement = GameController.Instance.DoomGuy.GetComponent<IMovement>();
        pWeapon = GameController.Instance.DoomGuy.GetComponent<W_Controller>();
        pMovement = GameController.Instance.DoomGuy.GetComponent<P_Movement>();
        pInventory = GameController.Instance.DoomGuy.GetComponent<P_Inventory>();
        pWeapon.SwapWeapon(Weapons.WeaponType.Pistol);

        isInitialized = true;
    }

    #region Start and Update
    private void Start()
    {
        cam = Camera.main;        
        StartCoroutine("Initiate");
    }
    public bool gameOver { get; set; } = false;
    void Update()
    {
        if (!isInitialized) return;

        #region Respawn
        // Respawn
        if (Input.GetKeyUp(KeyCode.R))
        {
            GameController.Instance.DoomGuy.GetComponent<P_Movement>().ResetPlayer();
            GameController.Instance.ResetItems();
            E_EnemyController[] enemies = FindObjectsOfType<E_EnemyController>();

            cam.transform.position = GameController.Instance.DoomGuy.transform.position;
            cam.transform.transform.position += Vector3.up * initialCamHeight;
            cameraLookUp = 0;

            foreach (E_EnemyController enemy in enemies)
            {
                enemy.ResetMonster();
            }

            GameController.Instance.Interface.EnableInterfaceAtRestart();
            Cheat.Code.ResetAllCheats();
        }
        #endregion
        
        // No need for further inputs if player died.
        if (gameOver) return;
        if (IsDead)
        {
            if (cam.transform.position.y > (GameController.Instance.DoomGuy.transform.position.y - GameController.Instance.DoomGuy.GetComponent<Collider>().bounds.extents.y) + 0.4f)
            {
                cam.transform.position += Vector3.down * Time.deltaTime * cameraDeadDownSpeed;
            }               
            
            return;
        }

        #region Movement
        // Movement
        if (Input.GetKey(KeyCode.W))
            MovePlayer(Directions.Forward);
        else if (Input.GetKey(KeyCode.S))
            MovePlayer(Directions.Backwards);
        else
            MovePlayer(Directions.Stopping);

        //if (Input.GetKey(KeyCode.Q))
        //    MovePlayer(Directions.Left);

        //if (Input.GetKey(KeyCode.E))
        //    MovePlayer(Directions.Right);

        if (Input.GetKey(KeyCode.A))
            MovePlayer(Directions.StrafeLeft);

        if (Input.GetKey(KeyCode.D))
            MovePlayer(Directions.StrafeRight);
        #endregion

        #region Mouse Movement
        // MOUSE
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (mouseX < 0) TurnCamera(Directions.Left, mouseX);
        if (mouseX > 0) TurnCamera(Directions.Right, mouseX);
        if (mouseY < 0) TurnCamera(Directions.Down, mouseY);
        if (mouseY > 0) TurnCamera(Directions.Up, mouseY);
        #endregion

        #region Attack
        // Weapon
        if (Input.GetButton("Fire1"))
        {
            if (pWeapon.State != Weapons.WeaponState.Free) return;

            pWeapon.Shoot();
        }
        
        if (Input.GetButtonUp("Fire1"))
        {
            if (pWeapon.State != Weapons.WeaponState.Shoot) return;

            pWeapon.SeizeFire();
        }
        #endregion

        #region Weapon Swap
        // Weapon Swap

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (pWeapon.State != Weapons.WeaponState.Free || pWeapon.equippedWeapon == Weapons.WeaponType.Fists) return;

            pWeapon.SwapWeapon(Weapons.WeaponType.Fists);            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (pWeapon.State != Weapons.WeaponState.Free || pWeapon.equippedWeapon == Weapons.WeaponType.Pistol) return;

            pWeapon.SwapWeapon(Weapons.WeaponType.Pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (pWeapon.State != Weapons.WeaponState.Free || pWeapon.equippedWeapon == Weapons.WeaponType.Shotgun) return;

            pWeapon.SwapWeapon(Weapons.WeaponType.Shotgun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (pWeapon.State != Weapons.WeaponState.Free || pWeapon.equippedWeapon == Weapons.WeaponType.Chaingun) return;

            pWeapon.SwapWeapon(Weapons.WeaponType.Chaingun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (pWeapon.State != Weapons.WeaponState.Free || pWeapon.equippedWeapon == Weapons.WeaponType.Rocket) return;

            pWeapon.SwapWeapon(Weapons.WeaponType.Rocket);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (pWeapon.State != Weapons.WeaponState.Free || pWeapon.equippedWeapon == Weapons.WeaponType.Plasma) return;

            pWeapon.SwapWeapon(Weapons.WeaponType.Plasma);
        }
        #endregion

        #region Interactable
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            float interactDistance = 7f;

            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactDistance);

            if (hit.collider != null)
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                    interactable.Interact(pInventory);
            }
        }
        #endregion

        #region Cheats

        if (Input.GetKeyDown(KeyCode.I)) Cheat.Code.Add("i");
        if (Input.GetKeyDown(KeyCode.D)) Cheat.Code.Add("d");
        if (Input.GetKeyDown(KeyCode.Q)) Cheat.Code.Add("q");
        if (Input.GetKeyDown(KeyCode.K)) Cheat.Code.Add("k");
        if (Input.GetKeyDown(KeyCode.F)) Cheat.Code.Add("f");
        if (Input.GetKeyDown(KeyCode.A)) Cheat.Code.Add("a");
        if (Input.GetKeyDown(KeyCode.S)) Cheat.Code.Add("s");
        if (Input.GetKeyDown(KeyCode.P)) Cheat.Code.Add("p");
        #endregion
    }
    #endregion

    void MovePlayer(Directions dir) 
    {
        if (playerMovement == null) return;

        playerMovement.GetInput(dir);
    }
    //void TurnPlayer(Directions dir, float sensitivity)
    //{
    //    if (playerMovement == null) return;

    //    playerMovement.TurnPlayer(dir, sensitivity);
    //}    
    void TurnCamera(Directions dir, float cursorMoveDistance)
    {
        #region Properties
        var _camTransform = cam.transform.rotation.eulerAngles;
        var DoomTransform = GameController.Instance.DoomGuy.transform.eulerAngles;
        cursorMoveDistance = Mathf.Abs(cursorMoveDistance);
        #endregion

        #region Turn Player
        if (dir == Inputs.Directions.Left)
            playerMovement.TurnPlayer(dir, (cursorMoveDistance * mouseSensitivity * Time.deltaTime));
        if (dir == Inputs.Directions.Right)
            playerMovement.TurnPlayer(dir, (cursorMoveDistance * mouseSensitivity * Time.deltaTime));
        #endregion

        #region Rotate Player
        if (dir == Inputs.Directions.Down)
            cameraLookUp += cursorMoveDistance * mouseSensitivity * Time.deltaTime;

        if (dir == Inputs.Directions.Up)
            cameraLookUp -= cursorMoveDistance * mouseSensitivity * Time.deltaTime;

        cameraLookUp = Mathf.Clamp(cameraLookUp, -75f, 75);

        _camTransform.x = cameraLookUp;
        _camTransform.y = DoomTransform.y;
        _camTransform.z = DoomTransform.z;
        cam.transform.rotation = Quaternion.Euler(_camTransform);
        #endregion

        #region Comments
        //lookRotation = Quaternion.Euler(_lookRotation, 0, 0);
        //cam.transform.Rotate(rot);

        //GameController.Instance.debug.SetText(3, cam.transform.rotation.x + " " + cam.transform.rotation.y + " " + cam.transform.rotation.z);
        //if (cam.transform.rotation.x > 180)
        //{
        //    var _rot = cam.transform.rotation;
        //    _rot.x = 179.9f;
        //    cam.transform.rotation = _rot;
        //}
        #endregion
    }
}
