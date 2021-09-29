using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Items
{
    public enum ItemType
    {
        KeyBlue,
        KeyYellow,
        KeyRed,
        MedkitSmall,
        MedkitMed,
        MedkitLarge,
        ArmorScrap,
        Armor,
        MegaArmor,
        Backback,
        Clip,
        Shell,
        Cell,
        Rocket,
        Berserk,
        InvisSphere,
        MegaSphere,
    }
}
public static class Keys
{
    public enum KeyType
    {
        None,
        Blue,
        Yellow,
        Red,
        Trigger
    }
    //[Flags]
    //public enum KeyType : short
    //{ 
    //    None    = 0,
    //    Blue    = 1,
    //    Yellow  = 2,
    //    Red     = 3
    //}
}

public static class Doors
{
    public enum DoorState { Unlocked, Locked }
    public enum DoorStatus { Open, Closed }
}

public static class Props
{
    public enum PropType
    {
        None,
        LightStand,
        Candle,
        SupportCollumn,
        Barrel,
        DeadMarine,
        Burning_Barrel,
        Dead_Tree_A
    }
}
