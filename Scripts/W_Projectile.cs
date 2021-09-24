using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Projectile : MonoBehaviour
{
    Collider collider;
    AudioSource audio;

    [SerializeField] GameObject explosionPrefab;

    [SerializeField] Texture tex_rocket;
    [SerializeField] Texture tex_impact;

    int damage;
    int damageRolls;
    float areaOfEffect;
    float projectileSpeed;

    float rocketMaxTime = 20f;

    Vector3 direction;

    bool ready = false;

    void Start()
    {
        collider = GetComponent<Collider>();
        direction = transform.forward;
        //audio = GetComponent<AudioSource>();

        StartCoroutine("TimeOutRocket");
    }

    IEnumerator TimeOutRocket()
    {
        yield return new WaitForSeconds(rocketMaxTime);

        Destroy(gameObject);
    }

    void Update()
    {
        if (!ready) return;

        transform.position += direction * projectileSpeed * Time.deltaTime;
    }

    public void SetAttributes(int _damage, int _damageRolls, float _areaOfEffect, float _projectileSpeed)
    {
        damage = _damage;
        damageRolls = _damageRolls;
        areaOfEffect = _areaOfEffect;
        projectileSpeed = _projectileSpeed;

        ready = true;
    }

    void OnCollisionEnter(Collision collision) // TO_DO : DO PROPER RANGE CHECK INSTEAD OF GRABBING LITERLLY EVERY SINGLE MONSTER ON THE MAP. TERRIBLE IDEA
    {
        float distance = 0;

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer(Layers.DoomGuy)) return;

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer(Layers.Projectile)) return;

        Instantiate(explosionPrefab, transform.position, transform.rotation);

        E_EnemyController[] enemies = FindObjectsOfType<E_EnemyController>();

        foreach (E_EnemyController enemy in enemies)
        {
            distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance < areaOfEffect)
            {
                IHittable newHittable = enemy.GetComponent<IHittable>();

                if (newHittable != null)
                {
                    newHittable.ApplyDamage(CalculateDamage());
                }
            }
            int CalculateDamage()
            {
                int _rng = GameController.Instance.Rntable.P_Random();
                int _damage = damage * (_rng % damageRolls + 1);
                return _damage;
            }
        }

        Destroy(gameObject);
    }

}
