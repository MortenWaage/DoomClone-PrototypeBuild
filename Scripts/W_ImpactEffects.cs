using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_ImpactEffects : MonoBehaviour
{
    [SerializeField] GameObject effectProjectileStandard;
    [SerializeField] GameObject effectDamageBlood;

    public void PlayImpactEffect(Vector3 hitPoint, Weapons.WeaponType w_type)
    {
        switch (w_type)
        {
            case Weapons.WeaponType.Pistol      :   Instantiate(effectProjectileStandard, hitPoint, Quaternion.identity); break;
            case Weapons.WeaponType.Shotgun     :   Instantiate(effectProjectileStandard, hitPoint, Quaternion.identity); break;
            case Weapons.WeaponType.Chaingun    :   Instantiate(effectProjectileStandard, hitPoint, Quaternion.identity); break;
            default                             :   Instantiate(effectProjectileStandard, hitPoint, Quaternion.identity); break;
        }
    }
    public void PlayDamageEffect(Vector3 hitPoint, Demons.DemonType d_type)
    {
        switch (d_type)
        {
            case Demons.DemonType.Rifleman      : Instantiate(effectDamageBlood, hitPoint, Quaternion.identity); break;
            case Demons.DemonType.Imp           : Instantiate(effectDamageBlood, hitPoint, Quaternion.identity); break;
            case Demons.DemonType.Pinky         : Instantiate(effectDamageBlood, hitPoint, Quaternion.identity); break;
            default                             : Instantiate(effectDamageBlood, hitPoint, Quaternion.identity); break;
        }
    }
}
