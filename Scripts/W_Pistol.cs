using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Pistol : MonoBehaviour, IWeapons
{
    Weapons.WeaponType type;
    W_Controller controller;

    AudioSource audio;

    [SerializeField] LayerMask hittablesLayer;

    bool isActiveWeapon = false;

    [SerializeField] Texture[] tex_weapon;
    [SerializeField] Texture[] tex_effect;
    [SerializeField] Texture[] tex_impact;

    float shotsPerMinute = 150;
    int damage = 5;
    int damageRolls = 3;
    int ammoPerShot = 1;
    int numberOfShots = 1;
    float projectileSpread = 0; // In degrees

    Vector3 projectileOrigin;
    private void Start()
    {
        type = Weapons.WeaponType.Pistol;
        audio = GetComponent<AudioSource>();
        StartCoroutine("Initialize");
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

        controller.RemoveAmmo(ammoPerShot);

        for (int i = 0; i < numberOfShots; i++)
        {
            Vector3 forwardDirection = W_TargetMethods.GetBulletSpread(controller.cam.transform, projectileSpread);
            FireProjectile(forwardDirection);            
        }

        controller.wepAnimator.PlayShootAnimation(animationSpeed);
        controller.SetShotCompletionTime(animationSpeed);
    }
    private void FireProjectile(Vector3 forwardDirection)
    {
        projectileOrigin = controller.cam.transform.position;
        RaycastHit hit = W_TargetMethods.WeaponHitScan(projectileOrigin, forwardDirection, Mathf.Infinity);

        if (hit.collider == null) return;

        #region Check Against Map Geometry
        if (W_TargetMethods.HitMapGeometry(hit, controller.cam.transform.position, type))
            return;

        #endregion

        #region Check Against Attackbles
        IHittable target = W_TargetMethods.AttackableFromCollider(hit);
        if (target != null) target.ApplyDamage(CalculateDamage());

        #endregion

        int CalculateDamage()
        {
            int _rng = GameController.Instance.Rntable.P_Random();
            int _damage = damage * (_rng % damageRolls + 1);
            return _damage;
        }
    }
    public void ChangeWeapon(Weapons.WeaponType _type)
    {
        if (_type == type) isActiveWeapon = true;
        else { isActiveWeapon = false; return; }

        controller.Tex_Weapon = tex_weapon;
        controller.Tex_Effect = tex_effect;
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
