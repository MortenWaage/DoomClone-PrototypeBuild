using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Not for use")]
public class texDirection : MonoBehaviour
{
    float iteration = 0;
    float speed = 0.1f;
    int multi = 1;

    MeshRenderer rend;
    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        iteration += speed * multi * Time.deltaTime;

        if (iteration > 1) multi = -1;
        else if (iteration <= 0) multi = 1;

        iteration = Mathf.Clamp(iteration, 0, 1);

        Debug.Log(iteration);
        rend.material.SetFloat("Direction", iteration);
    }
}
