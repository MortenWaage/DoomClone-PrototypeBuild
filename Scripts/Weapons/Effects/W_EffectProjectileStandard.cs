using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_EffectProjectileStandard : MonoBehaviour
{
    float timeOut = .5f;
    float smokeRiseSpeed = 3f;

    void Update()
    {
        timeOut -= 1 * Time.deltaTime;

        transform.position += Vector3.up * smokeRiseSpeed * Time.deltaTime;

        if (timeOut <= 0) Destroy(gameObject);
    }
}
