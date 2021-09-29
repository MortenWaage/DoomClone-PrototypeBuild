using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Controller : MonoBehaviour
{
    [HideInInspector] public Camera cam;

    [SerializeField] GameObject muzzleObject;
    IWeapons muzzleFlashInterface;

    #region PROPERTIES
    public Weapons.WeaponState State { get; private set; }
    public Weapons.WeaponType equippedWeapon;
    public W_Animator wepAnimator { get; private set; }
    IWeapons wepAnimatorInterface;
    public float WeaponSwapDuration { get; private set; } = 1f;

    [SerializeField] Texture[] tex_weapon;
    [SerializeField] Texture[] tex_effect;
    [SerializeField] Texture[] tex_impact;

    int Ammo_Clip = 80;
    int Ammo_Shell = 24;
    int Ammo_Cell = 0;
    int Ammo_Rockets = 12;

    int Ammo_Clip_Max = 200;
    int Ammo_Shell_Max = 50;
    int Ammo_Cell_Max = 300;
    int Ammo_Rockets_Max = 50;

    int Backpack = 0;

    public Texture[] Tex_Weapon { get => tex_weapon; set => tex_weapon = value; }
    public Texture[] Tex_Effect { get => tex_effect; set => tex_effect = value; }
    public Texture[] Tex_Impact { get => tex_impact; set => tex_impact = value; }

    
    #region Weapons
    [SerializeField] GameObject wFists;
    [SerializeField] GameObject wChainsaw;
    [SerializeField] GameObject wPistol;
    [SerializeField] GameObject wShotgun;
    [SerializeField] GameObject wChaingun;
    [SerializeField] GameObject wRockets;
    [SerializeField] GameObject wPlasma;
    [SerializeField] GameObject wBFG9000;

    IWeapons iFists;
    IWeapons iChainsaw;
    IWeapons iPistol;
    IWeapons iShotgun;
    IWeapons iChaingun;
    IWeapons iRockets;
    IWeapons iPlasma;
    IWeapons iBFG9000;

    List <IWeapons> weaponsList;
    #endregion
    #endregion
    

    private void Start()
    {
        cam = Camera.main;
        State = Weapons.WeaponState.Free;

        muzzleFlashInterface = muzzleObject.GetComponent<IWeapons>();
        wepAnimator = GetComponent<W_Animator>();
        wepAnimatorInterface = wepAnimator.GetComponent<IWeapons>();

        SetWeaponInterfaces();       
    }
    void ChangeState(Weapons.WeaponState _newState)
    {
        State = _newState;
    }
    public void Shoot()
    {
        if (OutOfAmmo()) return;

        ChangeState(Weapons.WeaponState.Shoot);

        foreach (IWeapons weapon in weaponsList)
            weapon.Shoot();

        muzzleFlashInterface.Shoot();

        UI_UpdateAmmo();
    }
    public void RemoveAmmo(int ammoPerShot)
    {
        if (equippedWeapon == Weapons.WeaponType.Pistol || equippedWeapon == Weapons.WeaponType.Chaingun)
        {
            Ammo_Clip -= ammoPerShot;
        }
        else if (equippedWeapon == Weapons.WeaponType.Shotgun)
        {
            Ammo_Shell -= ammoPerShot;
        }
        else if (equippedWeapon == Weapons.WeaponType.Rocket)
        {
            Ammo_Rockets -= ammoPerShot;
        }
    }
    private bool OutOfAmmo()
    {
        if (equippedWeapon == Weapons.WeaponType.Pistol || equippedWeapon == Weapons.WeaponType.Chaingun)
        {
            if (Ammo_Clip <= 0) return true;
        }
        else if (equippedWeapon == Weapons.WeaponType.Shotgun)
        {
            if (Ammo_Shell <= 0) return true;
        }
        else if (equippedWeapon == Weapons.WeaponType.Rocket)
        {
            if (Ammo_Rockets <= 0) return true;
        }

        return false;
    }
    public void SeizeFire()
    {
        foreach (IWeapons weapon in weaponsList)
            weapon.SeizeFire();
    }
    public void SwapWeapon(Weapons.WeaponType _type)
    {
        ChangeState(Weapons.WeaponState.Swapping);

        foreach (IWeapons weapon in weaponsList)
            weapon.ChangeWeapon(_type);

        wepAnimatorInterface.ChangeWeapon(_type);
        muzzleFlashInterface.ChangeWeapon(_type);
        equippedWeapon = _type;

        UI_UpdateWeaponType();
        UI_UpdateAmmo();

        StartCoroutine("CompleteWeaponSwap");
    }
    private void UI_UpdateWeaponType()
    {
        GameController.Instance.Interface.UpdateType(equippedWeapon);
    }
    void UI_UpdateAmmo()
    {
        if (equippedWeapon == Weapons.WeaponType.Pistol || equippedWeapon == Weapons.WeaponType.Chaingun)
            GameController.Instance.Interface.UpdateAmmo((float)Ammo_Clip / (float)Ammo_Clip_Max, Ammo_Clip);
        else if (equippedWeapon == Weapons.WeaponType.Shotgun)
            GameController.Instance.Interface.UpdateAmmo((float)Ammo_Shell / (float)Ammo_Shell_Max, Ammo_Shell);
        else if (equippedWeapon == Weapons.WeaponType.Rocket)
            GameController.Instance.Interface.UpdateAmmo((float)Ammo_Rockets / (float)Ammo_Rockets_Max, Ammo_Rockets);

        float _clip = (float)Ammo_Clip / (float)Ammo_Clip_Max;
        float _shell = (float)Ammo_Shell / (float)Ammo_Shell_Max;
        float _cell = (float)Ammo_Cell / (float)Ammo_Cell_Max;
        float _rock = (float)Ammo_Rockets / (float)Ammo_Rockets_Max;

        GameController.Instance.Interface.UpdateOverview(_clip, _shell, _cell, _rock);
    }
    private void SetWeaponInterfaces()
    {
        weaponsList = new List<IWeapons>();

        if (wFists != null)
        {
            iFists = wFists.GetComponent<IWeapons>();
            weaponsList.Add(iFists);
        }
        if (wChainsaw != null)
        {
            iChainsaw = wChainsaw.GetComponent<IWeapons>();
            weaponsList.Add(iChainsaw);
        }
        if (wPistol != null)
        {
            iPistol = wPistol.GetComponent<IWeapons>();
            weaponsList.Add(iPistol);
        }
        if (wShotgun != null)
        {
            iShotgun = wShotgun.GetComponent<IWeapons>();
            weaponsList.Add(iShotgun);
        }
        if (wChaingun != null)
        {
            iChaingun = wChaingun.GetComponent<IWeapons>();
            weaponsList.Add(iChaingun);
        }
        if (wPlasma != null)
        {
            iPlasma = wPlasma.GetComponent<IWeapons>();
            weaponsList.Add(iPlasma);
        }
        if (wRockets != null)
        {
            iRockets = wRockets.GetComponent<IWeapons>();
            weaponsList.Add(iRockets);
        }
        if (wBFG9000 != null)
        {
            iBFG9000 = wBFG9000.GetComponent<IWeapons>();
            weaponsList.Add(iBFG9000);
        }

        foreach (IWeapons weapon in weaponsList)
        {
            weapon.SetController(this);
        }            
    }
    public void SetShotCompletionTime(float animationSpeed)
    {
        StartCoroutine("CompleteShoot", animationSpeed);
    }    
    IEnumerator CompleteShoot(float animationSpeed)
    {
        yield return new WaitForSeconds(animationSpeed);
        ChangeState(Weapons.WeaponState.Free);
    }
    IEnumerator CompleteWeaponSwap()
    {
        yield return new WaitForSeconds(WeaponSwapDuration);
        ChangeState(Weapons.WeaponState.Free);
    }
    public void ResetWeapons()
    {
        Ammo_Clip = 20;
        Ammo_Shell = 0;
        Ammo_Cell = 0;
        Ammo_Rockets = 0;

        /* defaults

    int Ammo_Clip_Max = 80;
    int Ammo_Shell_Max = 50;
    int Ammo_Cell_Max = 120;
    int Ammo_Rockets_Max = 30;
        */

        UI_UpdateWeaponType();
        UI_UpdateAmmo();
    }
    public void AddAmmo(int clip, int shell, int cell, int rocket, int backpack)
    {
        Ammo_Clip = Mathf.Min(Ammo_Clip + clip, Ammo_Clip_Max);
        Ammo_Shell = Mathf.Min(Ammo_Shell + shell, Ammo_Shell_Max);
        Ammo_Cell = Mathf.Min(Ammo_Cell + cell, Ammo_Cell_Max);
        Ammo_Rockets = Mathf.Min(Ammo_Rockets + rocket, Ammo_Rockets_Max);
        Backpack = Mathf.Min(Backpack + backpack, 1);

        UI_UpdateAmmo();
    }


    public void MaxAmmo()
    {
        Ammo_Clip = Ammo_Clip_Max;
        Ammo_Shell = Ammo_Shell_Max;
        Ammo_Cell = Ammo_Cell_Max;
        Ammo_Rockets = Ammo_Rockets_Max;

        UI_UpdateWeaponType();
        UI_UpdateAmmo();
    }
}
