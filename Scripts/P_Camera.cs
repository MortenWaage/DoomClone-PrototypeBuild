using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Camera : MonoBehaviour, ICamera
{
    Camera cam;


    float tiltAngle = 20f;
    float tiltSpeed = 3;

    [SerializeField] [Range(0, 10f)] float wobbleSpeed = 1f;
    [SerializeField] [Range(0, 20f)] float wobbleHeight = .25f;

    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {

    }

    public void ApplyCameraTilt(Vector3 velocity, Inputs.Directions dir)
    {
        //switch (dir)
        //{
        //    case Inputs.Directions.Forward      : MoveUpDown(); break;
        //    case Inputs.Directions.Backwards    : MoveUpDown(); break;
        //    case Inputs.Directions.StrafeLeft   : break;
        //    case Inputs.Directions.StrafeRight  : break;
        //    case Inputs.Directions.Stopping     : break;
        //}
    }

    public void ResetCameraTilt()
    {
        //forward = false;
        //cam.transform.position += Vector3.up * currentHeight;
    }

    public void MoveUpDown()
    {
        //forward = true;
    }
}
