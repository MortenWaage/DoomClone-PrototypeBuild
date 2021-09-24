using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Camera : MonoBehaviour, ICamera
{
    Camera cam;

    float currentHeight = 0;
    float tiltAngle = 20f;
    float tiltSpeed = 3;

    [SerializeField] [Range(0, 10f)] float wobbleSpeed = 1f;
    [SerializeField] [Range(0, 10f)] float wobbleHeight = .25f;

    void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {

    }

    public void ApplyCameraTilt(Vector3 velocity, Inputs.Directions dir)
    {
        switch (dir)
        {
            case Inputs.Directions.Forward      : MoveUpDown(); break;
            case Inputs.Directions.Backwards    : MoveUpDown(); break;
            case Inputs.Directions.StrafeLeft   : break;
            case Inputs.Directions.StrafeRight  : break;
            case Inputs.Directions.Stopping     : break;
        }
    }

    public void ResetCameraTilt()
    {
        cam.transform.position += Vector3.up * currentHeight;
    }

    public void MoveUpDown()
    {
        float startRange = 0.2f;    //your chosen start value
        float endRange = 1;    //your chose end value
        var oscilationRange = (endRange - startRange) / 2;
        var oscilationOffset = oscilationRange + startRange;

        var result = oscilationOffset + Mathf.Sin(Time.time) * oscilationRange;

        cam.transform.position += Vector3.up * result;
    }
}
