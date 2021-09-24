using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float timer = 1f;
    float animationTime = 0;

    MeshRenderer shader;

    private void Start()
    {
        shader = GetComponent<MeshRenderer>();        
    }

    private void Update()
    {
        timer -= 1 * Time.deltaTime;

        animationTime++;
        shader.material.SetFloat("_CurrentTime", timer);

        if (timer <= 0) Destroy();
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
