using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Not for use")]
public class HellFireScript : MonoBehaviour
{
    MeshRenderer rend;
    [SerializeField] [Range(1,20f)] float t = 0f;
    int dir = 1;
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (t >= 1) dir = -1;
        if (t <= 0) dir = 1;

        t += .5f * Time.deltaTime * dir;

        t = Mathf.Clamp(t, 0, 1);        

        float w = Mathf.Lerp(-50, 50, t);
        rend.material.SetFloat("fireWobble", w);
    }
}
