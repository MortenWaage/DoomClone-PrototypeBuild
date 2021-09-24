using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Demons
{
    public enum DemonType
    { 
        Rifleman,
        Shotgunner,
        Imp,
        Pinky,
        Chaingunner,
        Cacodemon,
        Mancubus,
        Revenant,
        LostSoul,
        ArchVile,
        Baron,
        Cyberdemon,
        Idol,
        Experimental
    }
    public enum MonsterState
    {
        Wander,
        Dead,
        Staggered,
        Attacking
    }

    public enum AttackType
    {
        Hitscanner,
        Melee,
        Projectile,
    }

    public enum Projectile
    {
        NONE,
        Rifle,
        ImpBolt,
        Shotgun,
    }
}
