using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
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

        DemonController[] enemies = FindObjectsOfType<DemonController>();

        foreach (DemonController enemy in enemies)
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

        //distance = Vector3.Distance(transform.position, GameController.Instance.DoomGuy.transform.position);
        //if (distance < areaOfEffect)
        //{
        //    var newHittable = GameController.Instance.DoomGuy.GetComponent<IHittable>();
        //    float damage = CalculateDamage(distance);
        //    newHittable.ApplyDamage(damage);
        //}

        Destroy(gameObject);
    }
    //float CalculateDamage(float distance)
    //{
    //    float finalDam = Mathf.FloorToInt(UnityEngine.Random.Range(damMin, damMax));
    //    finalDam = finalDam / distance;
    //    return finalDam;
    //}

    //void CheckIfHitTarget()
    //{
    //    projectileOrigin = GameController.Instance.DoomGuy.transform.position;
    //    Vector3 forward = GameController.Instance.DoomGuy.transform.forward;

    //    RaycastHit hit;

    //    Physics.Raycast(projectileOrigin, forward, out hit, Mathf.Infinity, hittablesLayer);

    //    if (hit.collider != null)
    //    {
    //        DemonController[] enemies = FindObjectsOfType<DemonController>();
    //        List<IHittable> viableTargets = new List<IHittable>();

    //        foreach (DemonController enemy in enemies)
    //        {
    //            if (Vector3.Distance(enemy.transform.position, hit.point) < areaOfEffect)
    //            {
    //                IHittable newHittable = enemy.GetComponent<IHittable>();

    //                if (newHittable != null)
    //                    viableTargets.Add(newHittable);
    //            }
    //        }

    //        foreach (IHittable target in viableTargets)
    //        {
    //            target.ApplyDamage(damage);
    //        }
    //    }
    //}
}
