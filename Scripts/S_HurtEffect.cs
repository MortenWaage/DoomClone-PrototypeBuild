using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_HurtEffect : MonoBehaviour, IScreenGlow
{
    RawImage glowEffect;
    Color screenGlowColor = new Color(0.85f, 0, 0, 1);
    float screenGlowTime;
    float screenMaxGlowTime = 0.75f;
    float glowAlpha;


    private void Start()
    {
        glowEffect = GetComponent<RawImage>();
    }

    
    void Update()
    {
        if (screenGlowTime <= 0) return;

        screenGlowTime = screenGlowTime > 0 ? screenGlowTime -= 1 * Time.deltaTime : 0;

        float t = screenGlowTime / screenMaxGlowTime;
        screenGlowColor.a = Mathf.Lerp(0, glowAlpha, t);
        glowEffect.color = screenGlowColor;

    }

    public void PlayScreenGlow(Color col, int damage)
    {
        screenGlowTime = screenMaxGlowTime;

        glowAlpha = Mathf.Clamp((0.5f + ((float)damage / 256)), 0.25f, 0.75f);

        screenGlowColor = col;
        screenGlowColor.a = glowAlpha;
    }
}
