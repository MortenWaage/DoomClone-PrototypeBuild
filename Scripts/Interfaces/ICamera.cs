using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICamera
{
    public void ApplyCameraTilt(Vector3 velocity, Inputs.Directions dir);
    public void ResetCameraTilt();
}
