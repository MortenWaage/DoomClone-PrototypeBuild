using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Rockets : MonoBehaviour, IWeapons
{
    [SerializeField] GameObject RocketPrefab;

    Weapons.WeaponType type;
    W_Controller controller;

    AudioSource audio;
    Camera cam;

    [SerializeField] LayerMask hittablesLayer;

    bool isActiveWeapon = false;

    [SerializeField] Texture[] tex_weapon;
    [SerializeField] Texture[] tex_effect;
    [SerializeField] Texture[] tex_impact;

    float shotsPerMinute = 96;
    int damage = 20;
    int damageRolls = 8;
    int ammoPerShot = 1;
    int numberOfShots = 1;
    float projectileSpread = 5f;
    float projectileSpeed = 50f;
    float areaOfEffect = 20f;

    

    Vector3 projectileOrigin;
    private void Start()
    {
        cam = Camera.main;
        type = Weapons.WeaponType.Rocket;
        audio = GetComponent<AudioSource>();
        //StartCoroutine("Initialize");
    }

    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();
    }

    
    public void Shoot()
    {
        if (!isActiveWeapon) return;

        float animationSpeed = 60 / shotsPerMinute;

        if (!audio.isPlaying) audio.Play();

        

        
        for (int i = 0; i < numberOfShots; i++)
        {
            FireProjectile(i);
            controller.RemoveAmmo(ammoPerShot);
        }

        controller.wepAnimator.PlayShootAnimation(animationSpeed);
        controller.SetShotCompletionTime(animationSpeed);
    }

    
    private void FireProjectile(int shotNumber)
    {
        Vector3 firingPos = cam.transform.position;
        Quaternion firingRot = cam.transform.rotation;
        var _finalSpeed = projectileSpeed;

        if (shotNumber > 0 )
            _finalSpeed *= 1 - ( projectileSpeed / (GameController.Instance.Rntable.P_Random() + 1) );

        GameObject newProjectile = Instantiate(RocketPrefab, firingPos, firingRot);//GameController.Instance.DoomGuy.transform.position, GameController.Instance.DoomGuy.transform.rotation);
        Rocket projectile = newProjectile.GetComponent<Rocket>();

        projectile.SetAttributes(damage, damageRolls, areaOfEffect, _finalSpeed);
    }

    public void ChangeWeapon(Weapons.WeaponType _type)
    {
        if (_type == type) isActiveWeapon = true;
        else { isActiveWeapon = false; return; }

        controller.Tex_Weapon = tex_weapon;
        controller.Tex_Effect = tex_weapon;
        controller.Tex_Impact = tex_weapon;
    }

    public void SetController(W_Controller _controller)
    {
        controller = _controller;
    }

    public void SeizeFire()
    {
        
    }
}
