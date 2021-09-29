using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W_MuzzleFlash : MonoBehaviour, IWeapons
{
    Weapons.WeaponType type;

    [SerializeField] RawImage Muzzle_Image;
    [SerializeField] RawImage Weapon_Position;

    Texture[] animations;
    [SerializeField] Texture[] pistol_flash;
    [SerializeField] Texture[] shotgun_flash;
    [SerializeField] Texture[] chaingun_flash;
    [SerializeField] [Range(0, 20f)] float animationSpeed = 1f;
    [SerializeField] Vector2[] flash_offsets;
    int weapon_offset_index;

    float animationProgress = 0;
    

    bool isPlaying = false;
    void Update()
    {
        if (!isPlaying) return;

        animationProgress += animationSpeed * Time.deltaTime;
        if (animationProgress >= animations.Length)
        {
            isPlaying = false;
            Muzzle_Image.enabled = false;
            return;
        }

        var index = Mathf.FloorToInt(animationProgress);
        Muzzle_Image.texture = animations[index];        
    }


    public void Shoot()
    {
        PlayFlashAnimation();
    }

    private void PlayFlashAnimation()
    {
        Vector2 flashPosition = new Vector2(Weapon_Position.transform.position.x + flash_offsets[weapon_offset_index].x,
                                            Weapon_Position.transform.position.y + flash_offsets[weapon_offset_index].y);

        Muzzle_Image.transform.position = flashPosition;

        Muzzle_Image.texture = animations[0];
        animationProgress = 0;
        Muzzle_Image.enabled = true;
        isPlaying = true;
    }

    public void SeizeFire() { }
    public void ChangeWeapon(Weapons.WeaponType _type)
    {
        switch (_type)
        {
            case Weapons.WeaponType.Pistol      : type = _type; animations = pistol_flash; weapon_offset_index = 0; break;
            case Weapons.WeaponType.Shotgun     : type = _type; animations = shotgun_flash; weapon_offset_index = 1; break;
            case Weapons.WeaponType.Chaingun    : type = _type; animations = chaingun_flash; weapon_offset_index = 2; break;
            default                             : type = _type; break; 
        }        
    }
    public void SetController(W_Controller wControl) { }
}
