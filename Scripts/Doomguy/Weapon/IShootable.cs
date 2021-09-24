using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    public void Shoot();

    public void StopShoot();

    public void SwapWeapon(Weapons.WeaponType type);

    public void GetWeaponData();
}
