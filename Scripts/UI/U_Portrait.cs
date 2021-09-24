using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class U_Portrait : MonoBehaviour, IPortrait
{
    [SerializeField] Texture[] portraitTextures;
    [SerializeField] RawImage portrait;

    int portraitIndex = 1;
    float portraitChangeFrequency = 1f;
    float portraitTimer = 1f;

    bool isDead = false;
        
    void Update()
    {
        if (isDead) return;

        portraitTimer = portraitTimer > 0 ? portraitTimer - ( 1 * Time.deltaTime ) : portraitChangeFrequency;

        if (portraitTimer > 0) return;

        if (portraitIndex != 1) portraitIndex = 1;
        else
        {
            var _rng = GameController.Instance.Rntable.P_Random();

            if (_rng > 128) portraitIndex = 2;
            else portraitIndex = 0;
        }

        portrait.texture = portraitTextures[portraitIndex];
    }

    public void PlayerDied()
    {
        portrait.texture = portraitTextures[3];
        isDead = true;
    }

    public void PlayerRespawn()
    {
        portrait.texture = portraitTextures[1];
        isDead = false;
    }
}
