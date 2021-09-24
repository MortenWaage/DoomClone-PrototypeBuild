using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScriptableObjectsList : MonoBehaviour
{
    [SerializeField] E_Data RomerosHead;
    [SerializeField] E_Data Rifleman;
    [SerializeField] E_Data Imp;
    [SerializeField] E_Data Pinky;
    [SerializeField] E_Data Experimental;

    public E_Data SetDemonTypeData(Demons.DemonType type)
    {
        if (type == Demons.DemonType.Rifleman) return Rifleman;
        if (type == Demons.DemonType.Imp) return Imp;
        if (type == Demons.DemonType.Pinky) return Pinky;
        if (type == Demons.DemonType.Experimental) return Experimental;
        else return RomerosHead;
    }
}
