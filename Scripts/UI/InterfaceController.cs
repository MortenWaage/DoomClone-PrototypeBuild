using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour, IPlayerUI
{
    [SerializeField] RawImage HP_Bar;
    [SerializeField] RawImage Armor_Bar;

    [SerializeField] RawImage Ammo_Bar;
    [SerializeField] RawImage Wep_Cooldown;

    [SerializeField] RawImage Clip_Bar;
    [SerializeField] RawImage Shell_Bar;
    [SerializeField] RawImage Cell_Bar;
    [SerializeField] RawImage Rock_Bar;

    [SerializeField] Text WeaponType;
    [SerializeField] Text AmmoCount;


    private void Start()
    {
        Clip_Bar.material = Instantiate<Material>(Clip_Bar.material);
        Shell_Bar.material = Instantiate<Material>(Shell_Bar.material);
        Cell_Bar.material = Instantiate<Material>(Cell_Bar.material);
        Rock_Bar.material = Instantiate<Material>(Rock_Bar.material);
    }
    public void UpdateHealth(float health)
    {
        HP_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(health, 0, 1));
    }
    public void UpdateArmor(float armor)
    {
        Armor_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(armor, 0, 1));
    }

    public void UpdateAmmo(float ammo, int count)
    {
        AmmoCount.text = count.ToString();
        Ammo_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(ammo, 0, 1));
    }
    public void UpdateReload(float reload)
    {
        Wep_Cooldown.material.SetFloat("_FillPercent", Mathf.Clamp(reload, 0, 1));
    }
    public void UpdateType(Weapons.WeaponType type)
    {
        WeaponType.text = type.ToString();
    }

    public void UpdateOverview(float clip, float shell, float cell, float rock)
    {
        Clip_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(clip, 0, 1));
        Shell_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(shell, 0, 1));
        Cell_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(cell, 0, 1));
        Rock_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(rock, 0, 1));
    }
}
