using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Projectile : MonoBehaviour
{
    #region Properties
    Demons.Projectile type;
    MeshRenderer render;
    Texture[] sprites;

    bool initialized = false;
    int damage;
    int damageRolls;
    float projectileSpeed;
    float expirationTime = 30f;

    Vector3 direction;
    LayerMask targetLayer;
    #endregion

    #region Start and Update
    void Start()
    {
        direction = transform.forward;

        render = GetComponent<MeshRenderer>();
        if (render == null)
        {
            Debug.Log("Cannt get MeshRenderer");
            Debug.Break();
        }

        targetLayer = LayerMask.GetMask("DoomGuy");
    }
    void Update()
    {
        if (!initialized) return;

        transform.position += direction * projectileSpeed * Time.deltaTime;
    }
    #endregion

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.gameObject.name);

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer(Layers.Projectile)) return;

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer(Layers.DoomGuy))
        {
            P_Vitals vitals = collision.collider.GetComponent<P_Vitals>();
            if (vitals != null)
                P_CauseDamage(vitals);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer(Layers.Map))
        {
            Destroy(gameObject);
        } 
    }
    public void SetAttributes(Texture[] tex, int dam, int damRoll, float pSpeed)
    {
        damage = dam;
        damageRolls = damRoll;
        projectileSpeed = pSpeed;
        float projectileOffset = 5f; // pass this in or check against type. bigger monsters need bigger offset

        transform.position += (transform.forward * projectileOffset);

        StartCoroutine("DestroyAfterTime");

        initialized = true;
    }
    void P_CauseDamage(P_Vitals vitals)
    {
        int _damage = CalculateDamage();

        vitals.ApplyDamage(_damage);

        Destroy(gameObject);

        int CalculateDamage()
        {
            int _rng = GameController.Instance.Rntable.P_Random();
            int _damage = damage * (_rng % damageRolls + 1);
            return _damage;
        }
    }
    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(expirationTime);

        Destroy(gameObject);
    }
}
