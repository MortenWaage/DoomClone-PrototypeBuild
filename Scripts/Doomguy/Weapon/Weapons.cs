using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Weapons
{
    public enum WeaponState { Free, Shoot, Swapping }
    public enum WeaponType { NO_ARGUMENT, Fists, Chainsaw, Pistol, Shotgun, Chaingun, Rocket, Plasma, BFG9000 }
    public enum FiringMode { Single, Continuous }
}
