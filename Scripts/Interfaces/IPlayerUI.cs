using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerUI
{
    public void UpdateHealth(float health);
    public void UpdateArmor(float armor);
    public void UpdateReload(float reload);
    public void UpdateAmmo(float ammo, int count);

    public void UpdateType(Weapons.WeaponType type);

    public void UpdateOverview(float clip, float shell, float cell, float rock);

}
