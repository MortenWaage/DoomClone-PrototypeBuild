using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    void GetInput(Inputs.Directions dir);
    void TurnPlayer(Inputs.Directions dir, float sensitivity);
    void LookPlayer(Inputs.Directions dir, float sensitivity);
}
