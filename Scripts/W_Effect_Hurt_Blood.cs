using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Effect_Hurt_Blood : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 10f)] float timeOut = 1f;
    [SerializeField] [Range(-30f, 30f)] float bloodFallSpeed = -20f;
    [SerializeField] [Range(0,20f)] float bloodVelocity = 8f;
    [SerializeField] [Range(0, 20f)] float directionalSpeed = 5f;
    float direction;
    Vector3 splatterDirection;

    private void Awake()
    {
        var _rng = GameController.Instance.Rntable.P_Random();
        if (_rng < 128) direction = 1;
        else direction = -1;

        int index = Mathf.FloorToInt(GameController.Instance.Rntable.P_Random() / 128);
        splatterDirection = Directions.Sides[index] * direction;
    }   
    void Update()
    {
        timeOut -= 1 * Time.deltaTime;
        bloodVelocity += bloodFallSpeed * Time.deltaTime;

        transform.position += Vector3.up * bloodVelocity * Time.deltaTime;
        transform.position += splatterDirection * directionalSpeed * Time.deltaTime;

        if (timeOut <= 0) Destroy(gameObject);
    }
}