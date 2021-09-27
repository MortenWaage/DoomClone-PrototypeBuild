using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Inventory : MonoBehaviour
{
    P_Vitals playerVitals;
    W_Controller playerWeapon;

    bool blueKey = false;
    bool yellowKey = false;
    bool redKey = false;

    private void Start()
    {
        playerVitals = GetComponent<P_Vitals>();
        playerWeapon = GetComponent<W_Controller>();
    }

    public bool ChecKey(Keys.KeyType type)
    {
        switch (type)
        {
            case Keys.KeyType.Blue  : return blueKey;
            case Keys.KeyType.Yellow: return yellowKey;
            case Keys.KeyType.Red   : return redKey;
            default                 : return false;
        }
    }    
    public void AddKey(Keys.KeyType type)
    {
        GameController.Instance.Interface.PlayPickUpText(type);

        switch (type)
        {
            case Keys.KeyType.Blue  : blueKey   = true; break;
            case Keys.KeyType.Yellow: yellowKey = true; break;
            case Keys.KeyType.Red   : redKey    = true; break;
        }

        UpdateKeyInfo();
    }

    private void UpdateKeyInfo()
    {
        GameController.Instance.Interface.UpdateKeys(blueKey, yellowKey, redKey);
    }

    public void Cheat_Toggle_Keys(bool toggle)
    {
        blueKey = toggle;
        yellowKey = toggle;
        redKey = toggle;

        UpdateKeyInfo();
    }
    public void AddAmmo(int clip, int shell, int cell, int rocket, int backback)
    {
        playerWeapon.AddAmmo(clip, shell, cell, rocket, backback);
    }
    public void AddVitals(int health, int armor)
    {
        playerVitals.AddVitals(health, armor);
    }
}
