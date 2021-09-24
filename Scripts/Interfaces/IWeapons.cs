using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapons
{
    public void Shoot();
    public void SeizeFire();
    public void ChangeWeapon(Weapons.WeaponType _type);
    public void SetController(W_Controller controller);
}
