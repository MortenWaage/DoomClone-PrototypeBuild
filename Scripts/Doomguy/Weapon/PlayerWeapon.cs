//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[Obsolete("Use WeaponController", true)]
//public class PlayerWeapon : MonoBehaviour, IShootable
//{
//    Weapons.WeaponState state;
//    public Weapons.WeaponState State { get => state; }

//    AudioSource weaponAudioPlayer;
//    AudioClip weaponEffect;
//    WeaponAnimation animations;

//    [SerializeField] GameObject w_Fists;
//    [SerializeField] GameObject w_Pistol;
//    [SerializeField] GameObject w_Shotgun;
//    [SerializeField] GameObject w_Chaingun;
//    ILoadWeaponData i_Fists;
//    ILoadWeaponData i_Pistol;
//    ILoadWeaponData i_Shotgun;
//    ILoadWeaponData i_Chaingun;

//    public List<float> weapon_Data;
//    public List<Texture> weapon_Sprites;
//    public List<Texture> weapon_Effects;

//    [SerializeField] [Range(0, 5f)] float weaponSwapDuration;
//    public float SwapDuration { get => weaponSwapDuration; }

//    private void Start()
//    {
//        weaponAudioPlayer = GetComponent<AudioSource>();

//        state = Weapons.WeaponState.Free;

//        if (i_Fists != null) i_Fists = w_Fists.GetComponent<ILoadWeaponData>();
//        if (w_Pistol != null) i_Pistol = w_Pistol.GetComponent<ILoadWeaponData>();
//        if (w_Shotgun != null) i_Shotgun = w_Shotgun.GetComponent<ILoadWeaponData>();
//        if (w_Chaingun != null) i_Chaingun = w_Chaingun.GetComponent<ILoadWeaponData>();

//        // Load the Pistol as default weapon
//        weapon_Data = i_Pistol.LoadWeaponData();
//        weapon_Sprites = i_Pistol.LoadWeaponSpriteData();
//        weaponEffect = i_Pistol.LoadWeaponSound();
//        weaponAudioPlayer.clip = weaponEffect;
//    }

//    public void SwapWeapon(Weapons.WeaponType _weapon)
//    {
//        if (_weapon == Weapons.WeaponType.Fists)
//        {
//            if (i_Fists == null) return;

//            weapon_Data = i_Fists.LoadWeaponData();
//            weapon_Sprites = i_Fists.LoadWeaponSpriteData();
//            weaponEffect = i_Fists.LoadWeaponSound();
//            weapon_Effects = i_Fists.LoadWeaponEffectData();
//            weaponAudioPlayer.clip = weaponEffect;

//            weaponFiringMode = Weapons.FiringMode.Single;

//            ChangeState(Weapons.WeaponState.Swapping);
//        }
//        if (_weapon == Weapons.WeaponType.Pistol)
//        {
//            if (i_Pistol == null) return;

//            weapon_Data = i_Pistol.LoadWeaponData();
//            weapon_Sprites = i_Pistol.LoadWeaponSpriteData();
//            weaponEffect = i_Pistol.LoadWeaponSound();
//            weapon_Effects = i_Pistol.LoadWeaponEffectData();
//            weaponAudioPlayer.clip = weaponEffect;

//            weaponFiringMode = Weapons.FiringMode.Single;

//            ChangeState(Weapons.WeaponState.Swapping);
//        }
//        else if (_weapon == Weapons.WeaponType.Shotgun)
//        {
//            if (i_Shotgun == null) return;

//            weapon_Data = i_Shotgun.LoadWeaponData();
//            weapon_Sprites = i_Shotgun.LoadWeaponSpriteData();
//            weaponEffect = i_Shotgun.LoadWeaponSound();
//            weapon_Effects = i_Shotgun.LoadWeaponEffectData();
//            weaponAudioPlayer.clip = weaponEffect;

//            weaponFiringMode = Weapons.FiringMode.Single;

//            ChangeState(Weapons.WeaponState.Swapping);
//        }
//        else if (_weapon == Weapons.WeaponType.Chaingun)
//        {
//            if (i_Chaingun == null) return;

//            Debug.Log("Swapping to Chaingun");

//            weapon_Data = i_Chaingun.LoadWeaponData();
//            weapon_Sprites = i_Chaingun.LoadWeaponSpriteData();
//            weaponEffect = i_Chaingun.LoadWeaponSound();
//            weapon_Effects = i_Chaingun.LoadWeaponEffectData();
//            weaponAudioPlayer.clip = weaponEffect;

//            weaponFiringMode = Weapons.FiringMode.Continuous;

//            ChangeState(Weapons.WeaponState.Swapping);
//        }
//    }

//    void ChangeState(Weapons.WeaponState _state)
//    {
//        if (_state == Weapons.WeaponState.Swapping) StartCoroutine("UnlockWeaponSwap");

//        state = _state;
//    }

//    IEnumerator UnlockWeaponSwap()
//    {
//        yield return new WaitForSeconds(weaponSwapDuration);

//        ChangeState(Weapons.WeaponState.Free);
//    }
//    public void Shoot()
//    {
//        if (state == Weapons.WeaponState.Shoot) return;

//        ChangeState(Weapons.WeaponState.Shoot);

//        float min = weapon_Data[0];
//        float max = weapon_Data[1];

//        float damage = Mathf.Round(UnityEngine.Random.Range(min, max+1));


//        FireBullets(damage);

//        if (weaponAudioPlayer.isPlaying) weaponAudioPlayer.Stop();
//        weaponAudioPlayer.Play();

//        StartCoroutine("UnlockWeaponAfterTime");
//    }

//    [SerializeField] LayerMask hittableLayer;
//    private void FireBullets(float _damage)
//    {
//        RaycastHit hit;

//        Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, hittableLayer);

//        if (hit.collider != null)
//        {
//            string dmgTxt = "Hit "+ hit.collider.name + " for " + _damage.ToString() + " damage";
//            GameController.Instance.debug.SetText(5, dmgTxt);

//            IHittable hittable = hit.collider.GetComponent<IHittable>();
//            if (hittable != null)
//                hittable.ApplyDamage(CalculateDamage());
//        }

//        int CalculateDamage()
//        {
//            int _rng = GameController.Instance.Rntable.P_Random();

//            int _damage = damage * (_rng % damageRolls + 1);

//            return _damage;
//        }
//    }

//    IEnumerator UnlockWeaponAfterTime()
//    {
//        float weaponCooldown = 60 / weapon_Data[2];
//        yield return new WaitForSeconds(weaponCooldown);

//        state = Weapons.WeaponState.Free;
//    }

//    public void GetWeaponData()
//    {
//        // Dummy function - Serves no purpose
//    }

//    Weapons.FiringMode weaponFiringMode;
//    public void StopShoot()
//    {
//        if (weaponFiringMode == Weapons.FiringMode.Continuous)
//            weaponAudioPlayer.Stop();
//    }
//}
