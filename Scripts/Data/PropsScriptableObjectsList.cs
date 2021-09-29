using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsScriptableObjectsList : MonoBehaviour
{
    [SerializeField] O_Data None;
    [SerializeField] O_Data Candle;
    [SerializeField] O_Data LightStand;
    [SerializeField] O_Data SupportCollumn;
    [SerializeField] O_Data Barrel;
    [SerializeField] O_Data DeadMarine;
    [SerializeField] O_Data Burning_Barrel;
    [SerializeField] O_Data Dead_Tree_A;

    public O_Data SetItemTypeData(Props.PropType type)
    {
        switch (type)
        {
            case Props.PropType.Candle              : return Candle;
            case Props.PropType.LightStand          : return LightStand;
            case Props.PropType.SupportCollumn      : return SupportCollumn;
            case Props.PropType.Barrel              : return Barrel;
            case Props.PropType.DeadMarine          : return DeadMarine;
            case Props.PropType.Burning_Barrel      : return Burning_Barrel;
            case Props.PropType.Dead_Tree_A         : return Dead_Tree_A;
            default                                 : return None;
        }
    }
}