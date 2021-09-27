using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Projectile_Enemy : MonoBehaviour
{
    #region Properties
    Demons.Projectile type;
    Renderer rend;
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

        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.Log("Cannot get MeshRenderer");
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
        StartCoroutine("SetTexture");
        damage = dam;
        damageRolls = damRoll;
        projectileSpeed = pSpeed;
        float projectileOffset = 5f; // pass this in or check against type. bigger monsters need bigger offset

        transform.position += (transform.forward * projectileOffset);

        sprites = tex;

        StartCoroutine("DestroyAfterTime");

        initialized = true;
    }
    IEnumerator SetTexture()
    {
        yield return new WaitForEndOfFrame();

        if (rend == null) Debug.Break();
        if (sprites[0] == null) Debug.Log("no texture");
        rend.material.SetTexture("_MainTex", sprites[0]);
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
