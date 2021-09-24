using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Keys
{
    [Flags]
    public enum KeyType : short
    { 
        None    = 0,
        Blue    = 1,
        Yellow  = 2,
        Red     = 3
    }
}

public static class Doors
{
    public enum DoorState { Unlocked, Locked }
    public enum DoorStatus { Open, Closed }
}
