//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//[Obsolete("Use WeaponAnimator", true)]
//public class WeaponAnimation : MonoBehaviour, IShootable
//{
//    RawImage weapon;
//    [SerializeField] RawImage weaponEffect;

//    MovePlayer pMovement;
//    PlayerWeapon pWeapon;

//    public List<Texture> weaponSprites;
//    public List<Texture> weaponEffects;

//    public float animationLength;
//    float timeLeftOnAnimation;
//    float timePerAnimationFrame;
//    float timePerEffectFrame;
//    float timeLeftOnEffect;
//    int currentFrame = 0;
//    int currentEffectFrame = 0;
//    bool weaponUseEffects = false;

//    [Header("Weapon Wobble Settings")]
//    [SerializeField] [Range(0, 100)] float wobbleX;
//    [SerializeField] [Range(0, 50)] float wobbleY;
//    [SerializeField] [Range(0, 1f)] float wobbleSpeed;
//    [SerializeField] [Range(-100, 100f)] float weaponMoveHeightModifier;
//    [SerializeField] [Range(0, 5f)] float weaponLowerSpeed;
//    Vector2 initialPos;
//    float yModifier = 0;
//    float dirX = 1;

//    Vector2 screenSize;

//    void Start()
//    {
//        weapon = GetComponent<RawImage>();
//        //weaponEffect = GetComponentInChildren<RawImage>();
//        ToggleWeaponEffect("Start", false);

//        initialPos = weapon.transform.position;
//        GameController.Instance.weaponImage = GetComponent<RawImage>();

//        screenSize.x = Screen.width;
//        screenSize.y = Screen.height;

//        StartCoroutine("Initialize");
//    }



//    public void GetWeaponData()
//    {
//        weaponSprites = pWeapon.weapon_Sprites;
//        weaponEffects = pWeapon.weapon_Effects;

//        if (weaponEffects.Count > 0) { /*ToggleWeaponEffect("GetWeaponData", true);*/ weaponUseEffects = true; }

//        animationLength = 60 / pWeapon.weapon_Data[2];

//        if (pWeapon.State != Weapons.WeaponState.Swapping)
//        {
//            weapon.texture = weaponSprites[0];

//            if (weaponUseEffects)
//                weaponEffect.texture = weaponEffects[0];
//        }

//        timePerAnimationFrame = animationLength / weaponSprites.Count;
//        timeLeftOnAnimation = animationLength;

//        if (weaponUseEffects)
//        {
//            timePerEffectFrame = animationLength / weaponEffects.Count;
//            timeLeftOnEffect = animationLength;
//        }
//    }

//    [SerializeField] [Range(1, 100f)] float offset = 20f;
//    void Update()
//    {
//        Vector2 effectPos = weapon.transform.position;
//        effectPos.y += 30f;
//        weaponEffect.transform.position = effectPos;

//        if (pWeapon != null) // DEBUG_PRINT
//        {
//            string debug_text_1 = "Animation State: " + pWeapon.State.ToString();
//            string debug_text_2 = "Current animation frame: " + currentFrame.ToString();
//            string debug_text_3 = "Weapon X/Y pos: " + (int)weapon.transform.position.x + "/" + (int)weapon.transform.position.y;
//            string debug_text_4 = "EffectFrame: " + currentEffectFrame + " Using Effects: " + weaponUseEffects;
//            GameController.Instance.debug.SetText(1, debug_text_1);
//            GameController.Instance.debug.SetText(2, debug_text_2);
//            GameController.Instance.debug.SetText(3, debug_text_3);
//            GameController.Instance.debug.SetText(4, debug_text_4);

//        }

//        if (pMovement == null) return;

//        switch (pWeapon.State)
//        {
//            case Weapons.WeaponState.Free:

//                Vector2 wPos = weapon.transform.position;

//                if (pMovement.PlayerSpeed == 0) ResetWeapon(wPos);
//                else WobbleWeapon(wPos);

//                break;

//            case Weapons.WeaponState.Shoot:

//                PlayWeaponAnimation();

//                break;

//            case Weapons.WeaponState.Swapping:

//                PlaySwapAnimation();

//                break;
//        }

//    }

//    float remainingDuration;
//    float distanceToOffScreen;
//    string swapDirection = "";

//    private void PlaySwapAnimation()
//    {
//        float moveSpeed = (distanceToOffScreen / pWeapon.SwapDuration) * 2;

//        if (remainingDuration > pWeapon.SwapDuration * .5f)
//        {
//            swapDirection = "Down";
//            Vector2 wPos = weapon.transform.position;
//            wPos.y += moveSpeed * Time.deltaTime;
//            weapon.transform.position = wPos;
//        }
//        else
//        {
//            swapDirection = "Up";
//            Vector2 wPos = weapon.transform.position;
//            wPos.y -= moveSpeed * Time.deltaTime;
//            weapon.transform.position = wPos;
//        }

//        remainingDuration -= 1 * Time.deltaTime;
//    }

   
//    private void PlayWeaponAnimation()
//    {      
//        if (timeLeftOnAnimation <= 0)
//        {
//            ResetWeaponAnimation();
//            return;
//        }
//        if (timeLeftOnEffect <= 0)
//        {
//            ResetWeaponEffectsAnimation();
//        }

//        timeLeftOnAnimation -= 1 * Time.deltaTime;
//        timeLeftOnEffect -= 1 * Time.deltaTime;

//        int frame = Mathf.Max(0, Mathf.FloorToInt(timeLeftOnAnimation / timePerAnimationFrame));
//        int effectFrame = Mathf.Max(0, Mathf.FloorToInt(timeLeftOnEffect / timePerEffectFrame));

//        if (frame != currentFrame)
//        {
//            currentFrame = frame;
//            weapon.texture = weaponSprites[frame];
//        }

//        if (weaponUseEffects && effectFrame != currentEffectFrame)
//        {
//            currentEffectFrame = effectFrame;
//            weaponEffect.texture = weaponEffects[effectFrame];
//            GameController.Instance.debug.SetText(4, "Effect Frame: " + effectFrame.ToString());
//        }
            
//    }

//    private void ToggleWeaponEffect(string callingFunction, bool isEnabled)
//    {
//        bool currentStatus = weaponEffect.enabled;
//        Debug.Log("Toggling status from " + callingFunction + ". Status from: " + currentStatus + " to " + isEnabled);
//        weaponEffect.enabled = isEnabled;
//    }

//    void ResetWeaponAnimation()
//    {
//        weapon.texture = weaponSprites[0];        
//        timeLeftOnAnimation = animationLength;
//    }
//    void ResetWeaponEffectsAnimation()
//    {
//        if (!weaponUseEffects) return;
        
//        weaponEffect.texture = weaponEffects[0];
//        timeLeftOnEffect = animationLength;
//    }


//    private void WobbleWeapon(Vector2 wPos)
//    {
//        wPos.x += wobbleSpeed * dirX * pMovement.PlayerSpeed;

//        if (wPos.x <= initialPos.x - wobbleX) dirX = 1;
//        else if (wPos.x >= initialPos.x + wobbleX) dirX = -1;

//        float wVal = initialPos.x - wPos.x;
//        float t = Mathf.Abs(wVal / wobbleX);

//        if (yModifier <= Mathf.Abs(weaponMoveHeightModifier) + weaponLowerSpeed)
//            yModifier += weaponLowerSpeed;

//        wPos.y = Mathf.Lerp(initialPos.y, initialPos.y + wobbleY, t) - yModifier;

//        weapon.transform.position = wPos;
//    }

//    private void ResetWeapon(Vector2 wPos)
//    {
//        if (yModifier >= 0 + weaponLowerSpeed)
//            yModifier -= weaponLowerSpeed;

//        if (wPos.x < initialPos.x)
//            wPos.x += wobbleSpeed * 1;
//        else if (wPos.x > initialPos.x)
//            wPos.x -= wobbleSpeed * 1;

//        if (wPos.y < initialPos.y)
//            wPos.y += wobbleSpeed * 1;
//        else if (wPos.y > initialPos.y)
//            wPos.y -= wobbleSpeed * 1;

//        weapon.transform.position = wPos;
//    }

//    public void Shoot()
//    {
//        bool effectStatus = weaponEffect.enabled;
//        if (!effectStatus) ToggleWeaponEffect("Shoot()", true);

//        ResetWeaponAnimation();
//    }

//    public void StopShoot()
//    {
//        ToggleWeaponEffect("StopShoot", false);
//    }

//    public void SwapWeapon(Weapons.WeaponType type)
//    {
//        ToggleWeaponEffect("SwapWeapon", false);

//        timeLeftOnAnimation = 0;
//        timeLeftOnEffect = 0;
        
//        remainingDuration = pWeapon.SwapDuration;
//        distanceToOffScreen = 0 - weapon.transform.position.y;

//        StartCoroutine("SwapWeaponTexture");
//    }

//    IEnumerator Initialize()
//    {
//        yield return new WaitForEndOfFrame();

//        pWeapon = GameController.Instance.DoomGuy.GetComponent<PlayerWeapon>();
//        pMovement = GameController.Instance.DoomGuy.GetComponent<MovePlayer>();

//        GetWeaponData();
//    }

//    IEnumerator SwapWeaponTexture()
//    {
//        yield return new WaitForSeconds(pWeapon.SwapDuration * 0.5f);

//        weapon.texture = weaponSprites[0];
//    }
//}

