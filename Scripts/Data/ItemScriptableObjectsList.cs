using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScriptableObjectsList : MonoBehaviour
{
    [SerializeField] I_Data None;
    [SerializeField] I_Data KeyBlue;
    [SerializeField] I_Data KeyYellow;
    [SerializeField] I_Data KeyRed;
    [SerializeField] I_Data ArmorScrap;
    [SerializeField] I_Data MedKitSmall;
    [SerializeField] I_Data AmmoClip;
    [SerializeField] I_Data AmmoShell;
    [SerializeField] I_Data AmmoCell;
    [SerializeField] I_Data AmmoRocket;

    public I_Data SetItemTypeData(Items.ItemType type)
    {
        if (type == Items.ItemType.KeyBlue)     return KeyBlue;
        if (type == Items.ItemType.KeyYellow)   return KeyYellow;
        if (type == Items.ItemType.KeyRed)      return KeyRed;
        if (type == Items.ItemType.ArmorScrap)  return ArmorScrap;
        if (type == Items.ItemType.MedkitSmall) return MedKitSmall;
        if (type == Items.ItemType.Clip)        return AmmoClip;
        if (type == Items.ItemType.Shell)       return AmmoShell;
        if (type == Items.ItemType.Cell)        return AmmoCell;
        if (type == Items.ItemType.Rocket)      return AmmoRocket;
        else return None;
    }
}