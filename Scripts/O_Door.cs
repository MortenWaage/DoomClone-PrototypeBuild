using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Door : MonoBehaviour, IInteractable
{
    [SerializeField] Keys.KeyType key;
    [SerializeField] Doors.DoorState doorState;
    [SerializeField] Doors.DoorStatus doorStatus;

    AudioSource doorSound;

    [SerializeField] float doorRaiseSpeed = 5f;

    bool isLocked = false;
    bool isTriggered = false;
    float doorCloseDelay = 2.5f;
    float doorHeight;
    float doorInitialHeight;

    bool DoorOpen
    {
        get => transform.position.y >= (doorInitialHeight + doorHeight);
    }
    bool DoorClosed
    {
        get => transform.position.y <= doorInitialHeight;
    }
    bool DoorLocked
    {
        get
        {
            if (!isLocked) return false;

            if (GotKey)
            {
                return false;
            }
            else return true;
                
        }
    }



    bool GotKey { get; set; } = false;


    #region Start and Update
    void Start()
    {
        doorSound = GetComponent<AudioSource>();

        doorHeight = GetComponent<Collider>().bounds.extents.y * 2f;
        doorHeight -= doorHeight * 0.2f;
        doorInitialHeight = transform.position.y;

        if (doorStatus == Doors.DoorStatus.Open) transform.position += Vector3.up * doorHeight;
        if (doorState == Doors.DoorState.Locked) isLocked = true;
    }
    private void Update()
    {
        switch (doorStatus)
        {
            case Doors.DoorStatus.Open      : if (!DoorOpen)    RaiseDoor();    break;
            case Doors.DoorStatus.Closed    : if (!DoorClosed)  LowerDoor();    break;
        }

        if (Input.GetKeyDown(KeyCode.R))
            ResetDoor();
    }
    #endregion

    #region Methods
    void RaiseDoor()
    {
        Debug.Log("Raising Door");
        transform.position += Vector3.up * doorRaiseSpeed * Time.deltaTime;
    }
    void LowerDoor()
    {
        transform.position -= Vector3.up * doorRaiseSpeed * Time.deltaTime;
    }
    public void Interact(P_Inventory inventory)
    {
        if (inventory.ChecKey(key))
            GotKey = true;

        OpenDoor();
    }

    
    public void TriggerDoor()
    {
        if (isTriggered) return;

        Debug.Log("Opening the Door");
        doorStatus = Doors.DoorStatus.Open;
        isTriggered = true;
    }

    private void OpenDoor()
    {       
        if (!DoorOpen && !DoorLocked)
        {
            doorStatus = Doors.DoorStatus.Open;
            StopCoroutine("CloseDoorAfterTime");
            StartCoroutine("CloseDoorAfterTime");
            PlayDoorSound();
        }            
    }
    public void Close()
    {
        doorStatus = Doors.DoorStatus.Closed;
        GotKey = false;
    }

    public void ResetDoor()
    {
        GotKey = false;
        isTriggered = false;
    }

    IEnumerator CloseDoorAfterTime()
    {
        yield return new WaitForSeconds(doorCloseDelay);
        doorStatus = Doors.DoorStatus.Closed;
        GotKey = false;
        PlayDoorSound();
    }
    void PlayDoorSound()
    {
        if (doorSound != null)
            doorSound.Play();
    }

    #endregion
}
