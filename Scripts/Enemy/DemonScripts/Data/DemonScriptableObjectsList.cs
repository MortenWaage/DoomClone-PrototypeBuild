using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScriptableObjectsList : MonoBehaviour
{
    [SerializeField] DemonData RomerosHead;
    [SerializeField] DemonData Rifleman;
    [SerializeField] DemonData Imp;
    [SerializeField] DemonData Pinky;
    [SerializeField] DemonData Experimental;

    public DemonData SetDemonTypeData(Demons.DemonType type)
    {
        if (type == Demons.DemonType.Rifleman) return Rifleman;
        if (type == Demons.DemonType.Imp) return Imp;
        if (type == Demons.DemonType.Pinky) return Pinky;
        if (type == Demons.DemonType.Experimental) return Experimental;
        else return RomerosHead;
    }
}
