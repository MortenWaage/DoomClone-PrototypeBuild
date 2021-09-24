using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class E_Animator : MonoBehaviour
{
    Animation.Type currentAnimation;
    
    E_EnemyController demon;
    MeshRenderer rend;
    public AnimationQueue animations { get; private set; }
    public Texture[] animationSprites;

    #region Bool, Int, Float
    bool isLoop;       
    int priority { get; set; }
    int spriteIndexStart;
    int spriteIndexEnd;
    int spriteIndexLength;
    int numberOfDirections;
    int spritesPerDirection;
    float currentAnimationFrame = 0;
    float animationDuration;
    float spriteX;
    float spriteZ;
    #endregion

    #region Start and Update
    void Start()
    {      
        animations = new AnimationQueue();
        rend = GetComponent<MeshRenderer>();
        demon = GetComponentInParent<E_EnemyController>();

        currentAnimation = Animation.Type.NONE;
    }
    void Update()
    {
        if (animations == null) return;

        AnimateDemon();
    }
    #endregion

    #region Animation Methods
    private void AnimateDemon()
    {
        if (!demon.isInitialized) return;
        if (!animations.HasMoreAnimations) return;

        if (currentAnimation != animations.AType) {

            currentAnimation = animations.AType;
            isLoop = animations.ALoop;
            priority = animations.APriority;

            GetAnimationData(animations.AType);
        }

        if (currentAnimation != Animation.Type.NONE)
            PlayAnimation(currentAnimation);
    }
    void GetAnimationData(Animation.Type type)
    {
        spriteIndexStart = Animation.AnimationData(demon.Type, type)[0];
        spriteIndexEnd = Animation.AnimationData(demon.Type, type)[1];
        numberOfDirections = Animation.AnimationData(demon.Type, type)[2];
        spritesPerDirection = Animation.AnimationData(demon.Type, type)[3];
        animationDuration = (float)Animation.AnimationData(demon.Type, type)[4] * 0.001f; // convert miliseconds to seconds 
        spriteX = (float)Animation.AnimationData(demon.Type, type)[5] / 100f;
        spriteZ = (float)Animation.AnimationData(demon.Type, type)[6] / 100f;
        spriteIndexLength = spriteIndexEnd - spriteIndexStart + 1;
    }
    void PlayAnimation(Animation.Type _type)
    {        
        currentAnimationFrame += 1 * Time.deltaTime; 
        float framesPerSprite = animationDuration / spritesPerDirection;
        int animationFrame = Mathf.FloorToInt(currentAnimationFrame / framesPerSprite);

        if (currentAnimationFrame >= spritesPerDirection) animationFrame = spritesPerDirection-1;

        if (currentAnimationFrame >= animationDuration)
        {
            currentAnimationFrame = 0;
            if (!isLoop) animations.FinishedAnimation();
        }
        else
        {
            int direction = 0;
            if (numberOfDirections > 1) direction = GetDirectionInteger();

            int animationSprite = (spritesPerDirection * direction) + animationFrame;
            int finalIndex = spriteIndexStart + animationSprite;
            
            if (finalIndex >= animationSprites.Length)
            {
                Debug.Log("finalIndex exceeds animation length");
                Debug.Log($"SpritesPerDirection: {spritesPerDirection}. Direction: {direction} AnimationFrame: {animationFrame}");
                Debug.Log($"Animation Frame: {animationSprite}. FinalIndex: {finalIndex}");
                Debug.Break();
            }

            SetTexture(animationSprites[finalIndex]);
        } 
    }
    int GetDirectionInteger()
    {
        if (demon.A_Face_Target) return 0;

        Vector3 _direction = (transform.position - GameController.Instance.DoomGuy.transform.position).normalized;

        float signedAngleDiff = Vector3.SignedAngle(_direction, demon.MovementDirection, Vector3.up);
        float absDiff = 180 + signedAngleDiff;
        absDiff = 360 - absDiff;

        int dir_index = Mathf.Clamp(Mathf.FloorToInt(numberOfDirections - (absDiff / 45)), 0, (numberOfDirections - 1));
        if (absDiff >= 0 && absDiff < ( (360 / numberOfDirections) -45 /** 0.5 /*  Should be 0.5f but the entire direction seem to be shifted 22.5 degrees clockwise? */ )) dir_index = 1;

        return dir_index;
    }
    void SetTexture(Texture newTexture)
    {

        Vector3 _scale = Vector3.one;
        _scale.x *= spriteX;
        _scale.z *= spriteZ;
        rend.transform.localScale = _scale;

        rend.material.SetTexture("_MainTex", newTexture);
    }
    #endregion

    #region AnimationQueue calls
    public void ClearAnimations()
    {
        animations.ClearAnimations();
    }
    public void QueueAnimation(Animation.Type _animation, bool _isLooping, int _priority, bool _clearQueue)
    {
        animations.QueueAnimation(_animation, _isLooping, _priority, _clearQueue);
    }

    public bool IsQueueEmpty()
    {
        return !animations.HasMoreAnimations;
    }
    #endregion
}
public class AnimationQueue
{
    public AnimationQueue()
    {
        ClearAnimations();
    }

    #region Properties
    List<Animation.Type> type;
    List<bool> isLooping;
    List<int> priority;

    public int AnimationRemainingQueue { get => type.Count; }
    public bool HasMoreAnimations { get => type.Count > 0; }
    public Animation.Type AType { get
        {
            if (type.Count < 1) return Animation.Type.NONE;
            else return type[0];
        }
        private set { } }
    public bool ALoop { get => isLooping[0]; private set { } }
    public int APriority { get => priority[0]; private set { } }
    #endregion

    public void QueueAnimation(Animation.Type _animation, bool _isLooping, int _priority, bool _clearQueue)
    {
        if ( _clearQueue ) ClearAnimations();

        type.Add(_animation);
        isLooping.Add(_isLooping);
        priority.Add(_priority);
    }
    public void FinishedAnimation()
    {
        type.RemoveAt(0);
        isLooping.RemoveAt(0);
        priority.RemoveAt(0);
    }
    public void ClearAnimations()
    {
        type = new List<Animation.Type>();
        isLooping = new List<bool>();
        priority = new List<int>();
    }
}
public static class Animation
{
    public enum Type
    {
        NONE,
        Walk, 
        Pain, 
        Death, 
        AttackRange, 
        AttackMelee,
    }

    public static int[] AnimationData(Demons.DemonType demon_type, Type type)
    {
        switch (demon_type)
        {
            case Demons.DemonType.Imp       : return AnimationsImp[type];
            case Demons.DemonType.Rifleman  : return AnimationsRifleman[type];
            case Demons.DemonType.Pinky     : return AnimationsPinky[type];
            default                         : return new int[] { };
        }
    }

    public static readonly Dictionary<Type, int[]> AnimationsImp = new Dictionary<Type, int[]>()
    {
        #region From Index, To Index, Directions, Animations Per Direction, AnimationDuration(in miliseconds, Width X, Width Z), 
        #endregion
        {Type.Walk, new int[] {0, 31, 8, 4, 400, 85, 150} },
        {Type.Pain, new int[] {32, 39, 8, 1, 300, 100, 100} },
        {Type.Death, new int[] {40, 44, 1, 5, 1000, 100, 40} },
        {Type.AttackRange, new int[] {45, 68, 8, 3, 400, 85, 100} },
    };

    public static readonly Dictionary<Type, int[]> AnimationsRifleman = new Dictionary<Type, int[]>()
    {
        #region From Index, To Index, Directions, Animations Per Direction, AnimationDuration(in miliseconds, Width X, Width Z), 
        #endregion
        {Type.Walk, new int[] {0, 31, 8, 4, 400, 100, 150} },
        {Type.Pain, new int[] {32, 39, 8, 1, 300, 85, 100} },
        {Type.Death, new int[] {40, 44, 1, 5, 1000, 100, 45,} },
        {Type.AttackRange, new int[] {45, 60, 8, 2, 400, 65, 100} },
    };

    public static readonly Dictionary<Type, int[]> AnimationsPinky = new Dictionary<Type, int[]>()
    {
        #region From Index, To Index, Directions, Animations Per Direction, AnimationDuration(in miliseconds, Width X, Width Z), 
        #endregion
        {Type.Walk, new int[] {0, 31, 8, 4, 400, 100, 70} },
        {Type.Pain, new int[] {32, 39, 8, 1, 300, 100, 70} },
        {Type.Death, new int[] {40, 45, 1, 5, 800, 100, 65,} },
        {Type.AttackMelee, new int[] {46, 69, 8, 3, 300, 100, 75} },
    };
}
