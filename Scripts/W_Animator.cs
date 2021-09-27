using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W_Animator : MonoBehaviour, IWeapons
{
    #region PROPERTIES
    W_Controller pWeapon;
    P_Movement pMovement;

    [SerializeField] Texture default_tex;
    [SerializeField] RawImage weaponDisplayTexture;
    [SerializeField] RawImage weaponEffectTexture;

    float frameDuration;
    int totalAnimationFrames;
    int currentFrame;

    [Header("Weapon Wobble Settings")]
    [SerializeField] [Range(0, 100)] float wobbleX = 30f;
    [SerializeField] [Range(0, 50)] float wobbleY = 4.2f;
    [SerializeField] [Range(-100, 100f)] float weaponMoveHeightModifier = 40;
    [SerializeField] [Range(0, 5f)] float weaponLowerSpeed = 0.85f;
    [SerializeField] [Range(0, 3f)] float wobbleSpeed = 1f;

    Vector2 initialPos;
    Vector2 wPos;
    float dirX = 1;
    float yModifier = 0;

    int swapDirection = 1;
    float distanceToOffScreen;
    #endregion

    private void Start()
    {
        pWeapon = GetComponent<W_Controller>();
        pMovement = GetComponent<P_Movement>();

        weaponDisplayTexture.texture = default_tex;
        weaponEffectTexture.texture = default_tex;

        initialPos = weaponDisplayTexture.transform.position;
    }

    private void Update()
    {
        switch (pWeapon.State)
        {
            case Weapons.WeaponState.Swapping:

                wPos = weaponDisplayTexture.transform.position;
                PlaySwapAnimation();

                break;

            case Weapons.WeaponState.Free:

                wPos = weaponDisplayTexture.transform.position;
                if (pMovement.PlayerSpeed == 0) ResetWeapon(wPos);
                else PlayWobbleAnimation(wPos);

                break;
        }
    }

    #region WEAPON_WOBBLE

    private void PlayWobbleAnimation(Vector2 wPos)
    {
        wPos.x += wobbleSpeed * dirX;

        if (wPos.x <= initialPos.x - wobbleX) dirX = 1;
        else if (wPos.x >= initialPos.x + wobbleX) dirX = -1;

        float wVal = initialPos.x - wPos.x;
        float t = Mathf.Abs(wVal / wobbleX);

        if (yModifier <= Mathf.Abs(weaponMoveHeightModifier) + weaponLowerSpeed)
            yModifier += weaponLowerSpeed;

        wPos.y = Mathf.Lerp(initialPos.y, initialPos.y + wobbleY, t) - yModifier;

        weaponDisplayTexture.transform.position = wPos;
    }
    private void ResetWeapon(Vector2 wPos)
    {
        float margin = wobbleSpeed;

        if (yModifier >= 0 + weaponLowerSpeed)
            yModifier -= weaponLowerSpeed;

        if ( (wPos.x + margin) < initialPos.x) wPos.x += wobbleSpeed * 1;
        else if ( (wPos.x - margin) > initialPos.x) wPos.x -= wobbleSpeed * 1;

        if ( (wPos.y + margin) < initialPos.y) wPos.y += wobbleSpeed * 1;
        else if ( (wPos.y - margin) > initialPos.y) wPos.y -= wobbleSpeed * 1;

        SetTexturePosition(wPos);
    }

    [SerializeField] Vector2 effectOffsetVector = Vector2.zero;
    private void SetTexturePosition(Vector2 wPos)
    {
        weaponDisplayTexture.transform.position = wPos;
        weaponEffectTexture.transform.position = wPos + effectOffsetVector;
    }
    #endregion

    #region INTERFACE_METHODS
    public void Shoot()
    {
        SetTexturePosition(initialPos);

        weaponDisplayTexture.texture = pWeapon.Tex_Weapon[0];
        weaponEffectTexture.texture = pWeapon.Tex_Effect[0];
    }
    public void SeizeFire()
    {
        
    }
    public void ChangeWeapon(Weapons.WeaponType type)
    {
        StopAllCoroutines();
        StartCoroutine("ChangeWeaponSwapDirection");

        distanceToOffScreen = 0 - ( weaponDisplayTexture.transform.position.y + 120 );
    }
    #endregion

    #region ANIMATIONS


    void PlaySwapAnimation()
    {
        float animationSpeed = (distanceToOffScreen / pWeapon.WeaponSwapDuration) * 2;

        wPos.y += animationSpeed * Time.deltaTime * swapDirection;

        weaponDisplayTexture.transform.position = wPos;
        weaponEffectTexture.transform.position = wPos;
    }
    public void PlayShootAnimation(float animationSpeed)
    {
        totalAnimationFrames = pWeapon.Tex_Weapon.Length;
        currentFrame = 0;

        frameDuration = animationSpeed / (pWeapon.Tex_Weapon.Length);

        weaponEffectTexture.transform.position = weaponDisplayTexture.transform.position;
        weaponEffectTexture.enabled = false;
        StartCoroutine("ShootAnimation", frameDuration);
    }

    #region Coroutines
    IEnumerator ChangeWeaponSwapDirection()
    {
        swapDirection = 1;
        yield return new WaitForSeconds(pWeapon.WeaponSwapDuration * 0.5f);

        weaponDisplayTexture.texture = pWeapon.Tex_Weapon[0];
        weaponEffectTexture.texture = pWeapon.Tex_Effect[0];

        swapDirection = -1;
    }
    IEnumerator ShootAnimation(float frameDuration)
    {
        if (currentFrame < totalAnimationFrames)
            weaponDisplayTexture.texture = pWeapon.Tex_Weapon[currentFrame];        

        currentFrame++;

        yield return new WaitForSeconds(frameDuration);

        if (currentFrame < totalAnimationFrames)
        {
            StartCoroutine("ShootAnimation", frameDuration);
        }
        else
        {
            weaponDisplayTexture.texture = pWeapon.Tex_Weapon[0];
            weaponEffectTexture.enabled = false;
        }
            
    }
    #endregion

    #endregion

    public void SetController(W_Controller _controller)
    {
        // DUMMY FUNCTION
    }
}
