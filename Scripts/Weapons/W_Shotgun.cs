using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Shotgun : MonoBehaviour, IWeapons
{
    Weapons.WeaponType type;
    WeaponController controller;
    
    AudioSource audio;
    Camera cam;

    [SerializeField] LayerMask hittablesLayer;

    bool isActiveWeapon = false;

    [SerializeField] Texture[] tex_weapon;
    [SerializeField] Texture[] tex_effect;
    [SerializeField] Texture[] tex_impact;

    float shotsPerMinute = 58;
    int damage = 5;
    int damageRolls = 7;
    int ammoPerShot = 1;
    int numberOfShots = 4;
    float projectileSpread = 1.5f; // In degrees

    Vector3 projectileOrigin;
    private void Start()
    {
        cam = Camera.main;
        type = Weapons.WeaponType.Shotgun;
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
            Vector3 forwardDirection = W_FindTarget.GetBulletSpread(controller.cam.transform, projectileSpread);
            FireProjectile(forwardDirection);
            
        }

        controller.wepAnimator.PlayShootAnimation(animationSpeed);
        controller.SetShotCompletionTime(animationSpeed);
    }
    private void FireProjectile(Vector3 forwardDirection)
    {
        projectileOrigin = controller.cam.transform.position;
        RaycastHit hit = W_FindTarget.WeaponHitScan(projectileOrigin, forwardDirection, Mathf.Infinity);

        if (hit.collider == null) return;

        #region Check Against Map Geometry
        if (W_FindTarget.HitMapGeometry(hit, controller.cam.transform.position, type))
            return;

        #endregion

        #region Check Against Attackbles
        IHittable target = W_FindTarget.AttackableFromCollider(hit);
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
        controller.Tex_Effect = tex_weapon;
        controller.Tex_Impact = tex_weapon;
    }

    public void SetController(WeaponController _controller)
    {
        controller = _controller;
    }

    public void SeizeFire()
    {

    }
}
