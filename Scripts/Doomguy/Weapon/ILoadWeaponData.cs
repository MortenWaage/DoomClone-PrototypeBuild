using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoadWeaponData
{
    public List<float> LoadWeaponData();

    public List<Texture> LoadWeaponSpriteData();

    public AudioClip LoadWeaponSound();

    public List<Texture> LoadWeaponEffectData();
}
