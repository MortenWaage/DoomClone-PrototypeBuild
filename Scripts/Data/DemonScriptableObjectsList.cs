using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScriptableObjectsList : MonoBehaviour
{
    [SerializeField] E_Data RomerosHead;
    [SerializeField] E_Data Rifleman;
    [SerializeField] E_Data Imp;
    [SerializeField] E_Data Pinky;
    [SerializeField] E_Data Baron;
    [SerializeField] E_Data Experimental;

    public E_Data SetDemonTypeData(Demons.DemonType type)
    {
        switch (type)
        {
            case Demons.DemonType.Rifleman  : return Rifleman;
            case Demons.DemonType.Imp       : return Imp;
            case Demons.DemonType.Pinky     : return Pinky;
            case Demons.DemonType.Baron     : return Baron;
            default                         : return RomerosHead;
        }
    }
}
