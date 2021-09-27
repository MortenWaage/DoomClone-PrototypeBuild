using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Vitals : MonoBehaviour, IHittable
{
    [SerializeField] int health = 100;
    [SerializeField] int armor = 100;

    int maxHealth;
    int maxArmor = 200;

    bool environmentRecentlyApplied = false;
    float environmentTimerCurrent;
    float environmentTimerMax = 1.5f;

    P_Movement pMovement;

    private void Start()
    {
        environmentTimerCurrent = environmentTimerMax;
        maxHealth = health;

        pMovement = GetComponent<P_Movement>();
        UpdateUserInterface();
    }
    
    void Update()
    {
        if (!environmentRecentlyApplied) return;

        environmentTimerCurrent -= 1 * Time.deltaTime;
        if (environmentTimerCurrent > 0) return;
        
        environmentRecentlyApplied = false;
        environmentTimerCurrent = environmentTimerMax;
    }

    public void ApplyDamage(int _damage)
    {
        if (Cheat.Code.IsGodMode) return;

        int damage = _damage;
        float absorbed = (float)_damage / 3f;
        float absorbedDamage;

        if ((float)armor > absorbed) absorbedDamage = absorbed;
        else absorbedDamage = armor;

        damage -= (int)absorbedDamage;
        health -= damage;
        armor -= (int)absorbedDamage;

        UpdateUserInterface();
        ApplyHurtEffect(damage);
        CheckIfDied();
    }

    public void ApplyDamageEnvironment(int _damage)
    {
        if (Cheat.Code.IsGodMode) return;

        if (environmentRecentlyApplied) return;

        environmentRecentlyApplied = true;
        health -= _damage;
        UpdateUserInterface();
        ApplyAcidEffect(_damage);
        CheckIfDied();
    }

    void ApplyAcidEffect(int damage)
    {
        GameController.Instance.PlayScreenGlow(Color.green, damage);
        Debug.Log("Nuclear Waste bad!");
    }
    private void ApplyHurtEffect(int damage)
    {
        GameController.Instance.PlayScreenGlow(Color.red, damage);
        Debug.Log("Ouch!");
    }

    private void CheckIfDied()
    {
        if (health <= 0)
        {
            health = 0;
            pMovement.IsDead = true;
            GameController.Instance.portrait.PlayerDied();
        }
    }

    public void ResetVitals()
    {       
        health = maxHealth;
        armor = maxArmor / 2;

        pMovement.IsDead = false;

        UpdateUserInterface();

        GameController.Instance.portrait.PlayerRespawn();
    }

    public void AddVitals(int _health, int _armor)
    {
        health = Math.Min(health + _health, maxHealth);
        armor = Math.Min(armor + _armor, maxArmor);

        UpdateUserInterface();
    }

    private void UpdateUserInterface()
    {

        float healthPercentage = ((float)health / (float)maxHealth);
        GameController.Instance.Interface.UpdateHealth(healthPercentage);

        float armorPercentage = ((float)armor / (float)maxArmor);
        GameController.Instance.Interface.UpdateArmor(armorPercentage);
    }

    public void MaxHealthAmmo()
    {
        health = maxHealth;
        armor = maxArmor;
        UpdateUserInterface();
    }
}
