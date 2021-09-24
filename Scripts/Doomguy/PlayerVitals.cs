using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitals : MonoBehaviour, IHittable
{
    [SerializeField] int health = 100;
    [SerializeField] int armor = 100;

    int maxHealth;
    int maxArmor = 200;

    MovePlayer mover;

    private void Start()
    {
        maxHealth = health;

        float healthPercentage = ((float)health / (float)maxHealth);
        GameController.Instance.Interface.UpdateHealth(healthPercentage);

        float armorPercentage = ((float)armor / (float)maxArmor);
        GameController.Instance.Interface.UpdateArmor(armorPercentage);

        mover = GetComponent<MovePlayer>();
    }

    public void ApplyDamage(int _damage)
    {
        int damage = _damage;

        float absorbed = (float)_damage / 3f;
        float absorbedDamage;

        if ((float)armor > absorbed) absorbedDamage = absorbed;
        else absorbedDamage = armor;

        damage -= (int)absorbedDamage;

        health -= damage;
        armor -= (int)absorbedDamage;

        float healthPercentage = ((float)health / (float)maxHealth);
        GameController.Instance.Interface.UpdateHealth(healthPercentage);

        float armorPercentage = ((float)armor / (float)maxArmor);
        GameController.Instance.Interface.UpdateArmor(armorPercentage);

        ApplyHurtEffect(damage);
        CheckIfDied();
    }

    private void ApplyHurtEffect(int damage)
    {
        GameController.Instance.PlayScreenGlow(damage);
        Debug.Log("Ouch!");
    }

    private void CheckIfDied()
    {
        if (health <= 0)
        {
            health = 0;
            mover.IsDead = true;
            GameController.Instance.portrait.PlayerDied();
        }
    }

    public void ResetVitals()
    {       
        health = maxHealth;
        armor = maxArmor / 2;

        mover.IsDead = false;

        float healthPercentage = ((float)health / (float)maxHealth);
        GameController.Instance.Interface.UpdateHealth(healthPercentage);

        float armorPercentage = ((float)armor / (float)maxArmor);
        GameController.Instance.Interface.UpdateArmor(armorPercentage);

        GameController.Instance.portrait.PlayerRespawn();
    }
}
