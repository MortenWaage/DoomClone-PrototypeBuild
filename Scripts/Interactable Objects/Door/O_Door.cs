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

    float doorCloseDelay = 2.5f;
    float doorHeight;
    float doorInitialHeight;
    bool DoorOpen
    {
        get => transform.position.y >= doorHeight;
    }
    bool DoorClosed
    {
        get => transform.position.y <= doorInitialHeight;
    }
    bool DoorLocked
    {
        get
        {
            return isLocked;            
        }
    }
    bool GotKey
    {
        get => true;
    }

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
            case Doors.DoorStatus.Open      : if (!DoorOpen && !DoorLocked) RaiseDoor();   break;
            case Doors.DoorStatus.Closed    : if (!DoorClosed) LowerDoor(); break;
        }
    }
    #endregion

    #region Methods
    void RaiseDoor()
    {
        transform.position += Vector3.up * doorRaiseSpeed * Time.deltaTime;
    }
    void LowerDoor()
    {
        transform.position -= Vector3.up * doorRaiseSpeed * Time.deltaTime;
    }
    public void Interact(byte? keys = null)
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log((Keys.KeyType)i);
        }

        doorStatus = Doors.DoorStatus.Open;
        StopCoroutine("CloseDoorAfterTime");
        StartCoroutine("CloseDoorAfterTime");

        PlayDoorSound();
    }
    IEnumerator CloseDoorAfterTime()
    {
        yield return new WaitForSeconds(doorCloseDelay);
        doorStatus = Doors.DoorStatus.Closed;
        PlayDoorSound();
    }
    void PlayDoorSound()
    {
        if (doorSound != null)
            doorSound.Play();
    }
    #endregion
}
