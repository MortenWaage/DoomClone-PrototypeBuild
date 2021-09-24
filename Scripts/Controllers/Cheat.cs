using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public static Cheat Code { get; private set; }
    public bool IsGodMode { get; private set; } = false;
    string[] cheat;

    private void Awake()
    {
        if (Code == null) Code = this;
        else Destroy(gameObject);

        cheat = new string[5];
    }   
    public void Add(string pressedChar)
    {
        for (int i = 1; i < cheat.Length; i++)
            if (cheat[i] != null)
                cheat[i - 1] = cheat[i];

        cheat[4] = pressedChar;

        CheckCheats();
    }
    void CheckCheats()
    {
        string code = "";

        foreach(string _char in cheat)
        {
            code += _char;
        }

        if (code == "iddqd")
        {          
            if (!IsGodMode) IsGodMode = true;
            else IsGodMode = false;
            Array.Clear(cheat, 0, 5);
        }
        else if (code == "idkfa")
        {
            GameController.Instance.DoomGuy.GetComponent<W_Controller>().MaxAmmo();
            Array.Clear(cheat, 0, 5);
        }       
    }
    public void ResetAllCheats()
    {
        IsGodMode = false;
        Array.Clear(cheat, 0, 5);
    }
}
