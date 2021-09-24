using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour, IHittable
{
    #region Properties

    #region Enums
    [SerializeField] Demons.MonsterState spawnState;
    Demons.MonsterState state;
    [SerializeField] Demons.DemonType type;    
    public Demons.DemonType Type { get => type; private set { } }
    Demons.AttackType attackType;
    Demons.Projectile projectileType;
    #endregion

    #region Components and Prefabs
    DemonData demonData;
    DemonAnimator animator;
    Collider collider;
    MeshRenderer renderer;
    GameObject projectilePrefab;
    #endregion

    #region Vectors and Layer Masks
    public Vector3 initialSize { get; private set; }
    public Vector3 LookDirection { get => currentDirection; }
    Vector3 ground;
    Vector3 currentDirection;
    Vector3 spawnPosition;
    Vector3[] directions;

    [SerializeField] LayerMask doomGuyLayer;
    LayerMask groundLayer;
    LayerMask hittableLayer;
    #endregion

    #region Int, Float, Bool
    #region Bool
    public bool isInitialized { get; private set; } = false;
    bool S_Pain = false;
    #endregion
    #region Int
    int health;
    int maxHealth;
    int painChance;
    int damage;
    int damageRolls;
    #endregion
    #region Float
    public float PainDuration { get; private set; } = 0;
    public float maxPainDuration;
    float moveSpeed;
    float obstacleTraversionHeight;
    float groundMargin;
    float animationSpeed;
    float attackRange;
    float meleeRange;
    float sightRange;
    float attackSpeed;
    float remainingAttackSpeed;
    float reactionTime;
    float maxReactionTime = 3f;
    float staggeredDuration = 0;
    #endregion
    #endregion

    #region Audio and Textures
    AudioSource demonAudio;
    AudioClip aud_hurt;

    Texture[] projectileSprites;
    #endregion

    #region Other
    public bool A_Face_Target { get; private set; } = false;
    public float AnimationSpeed { get => animationSpeed; private set { } }
    public bool FoundEnemy { private get; set; } = false;
    public GameObject CurrentTarget { get; set; }
    public Vector3 MovementDirection
    {
        get {
            if (state == Demons.MonsterState.Attacking) return (CurrentTarget.transform.position - transform.position).normalized;
            else return currentDirection;
        }
    }
    #endregion
    #endregion

    #region Inspector Flags
    [SerializeField] bool enableMovementOnAwake = true;
    [SerializeField] bool useMovement = true;
    [SerializeField] bool isPassive = false;

    #endregion

    #region Start and Update
    private void Start()
    {
        demonData = GameController.Instance.demons.SetDemonTypeData(type);
        animator = GetComponentInChildren<DemonAnimator>();

        spawnPosition = transform.position;

        collider = GetComponentInChildren<Collider>();
        renderer = GetComponentInChildren<MeshRenderer>();
        demonAudio = GetComponent<AudioSource>();

        StartCoroutine("Initialize");
    }
    void Update()
    {
        if (!isInitialized || PlayerIsDead()) return;

        ground = GetGroundHeight();
        IncreaseReactionTime();

        #region State Machine
        switch (state)
        {
            #region Case.Wander:
            case Demons.MonsterState.Wander:

                Wander();

                if (animator.IsQueueEmpty())
                {
                    animator.QueueAnimation(Animation.Type.Walk, true, 0, true);
                }

                if (A_AttackChecks())
                    A_DecideAttackType();

                break;
            #endregion
            #region Case.Dead:
            case Demons.MonsterState.Dead:

                break;
            #endregion
            #region Case.Staggered:
            case Demons.MonsterState.Staggered:

                PainDuration = PainDuration > 0 ? (PainDuration - 1 * Time.deltaTime) : 0;
                
                if (PainDuration <= 0) ChangeState(Demons.MonsterState.Wander);                   

                break;
            #endregion
            #region Case.Attacking:
            case Demons.MonsterState.Attacking:

                remainingAttackSpeed -= 1 * Time.deltaTime;
                if (remainingAttackSpeed <= 0)
                {
                    animator.ClearAnimations();
                    A_FaceTarget(false);
                    ChangeState(Demons.MonsterState.Wander);
                }

                break;
            #endregion
        }
        #endregion
    }
    #endregion

    #region COMBAT

    #region OFFENSIVE_METHODS
    bool A_AttackChecks()
    {
        if (isPassive) return false;

        if (!IsInRange) return false;

        if (!InLineOfSight) return false;

        if (!In_Attack_Range) return false;

        if (enableMovementOnAwake) EnableMovement();

        if (S_Pain)
        {
            S_Pain = false;
            return true;
        }

        if (!CanReact) return false;

        return true;
    }

    private void EnableMovement()
    {
        if (!enableMovementOnAwake) return;

        enableMovementOnAwake = false;
        useMovement = true;
        currentDirection = GetNewDirection();
    }
    void A_DecideAttackType()
    {
        reactionTime = maxReactionTime;
        float distance = W_FindTarget.GetDistanceToTarget(transform.position, CurrentTarget.transform.position) - 64;

        // if enemy does not have ranged attack
        if (attackRange == -1) distance -= 128;
        distance = Mathf.Min(distance, 200);

        var _rng = GameController.Instance.Rntable.P_Random();

        if (_rng < distance) return;


        if (In_Melee_Range)
            AttackMelee();
        else
            AttackRanged();
    }
    void AttackMelee()
    {

    }
    void AttackRanged()
    {
        ChangeState(Demons.MonsterState.Attacking);
        animator.QueueAnimation(Animation.Type.AttackRange, true, 0, true);
        
        A_FaceTarget(true);
        A_Execute_Ranged_Attack();

        remainingAttackSpeed = attackSpeed;
    }
    void A_Execute_Ranged_Attack()
    {
        if (PlayerIsDead()) return;

        if (attackType == Demons.AttackType.Hitscanner)     A_Projectile_Instant();
        else                                                A_Projectile_Travel();

        void A_Projectile_Travel()
        {
            var _lookRotation = Quaternion.LookRotation(CurrentTarget.transform.position - transform.position).normalized;

            GameObject newProjectile = Instantiate(projectilePrefab, transform.position, _lookRotation);
            E_Projectile projectile = newProjectile.GetComponent<E_Projectile>();

            projectile.SetAttributes(projectileSprites, damage, damageRolls, demonData.projectileSpeed);
        }

        void A_Projectile_Instant()
        {
            Vector3 direction = CurrentTarget.transform.position - transform.position;

            RaycastHit hit;

            Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, doomGuyLayer);

            if (hit.collider != null)
            {
                PlayerVitals vitals = hit.collider.GetComponent<PlayerVitals>();

                if (vitals == null) { Debug.Log("No Vitals found on target"); return; }

                int damage = CalculateDamage();
                vitals.ApplyDamage(damage);
            }
            else Debug.Log("Cannot find physics layer");

            int CalculateDamage()
            {
                int _rng = GameController.Instance.Rntable.P_Random();
                int _damage = damage * (_rng % damageRolls + 1);
                return _damage;
            }
        }
    }
    void A_FaceTarget(bool isFacing)
    {
        A_Face_Target = isFacing;
    }
    #endregion

    #region DEFENSIVE_METHODS
    public void ApplyDamage(int damage)
    {
        if (state == Demons.MonsterState.Dead) return;

        if (demonAudio.clip != null) demonAudio.Play();

        ApplyHurtEffect();

        health -= damage;

        CheckIfDead();
    }

    private void ApplyHurtEffect()
    {
        Vector3 impactPoint = W_FindTarget.GetRandomImpactOffset(transform.position, initialSize.x * 0.2f, initialSize.y * 0.2f);
        Vector3 offset = (transform.position - GameController.Instance.DoomGuy.transform.position).normalized * 2f;
        GameController.Instance.w_effects.PlayDamageEffect(impactPoint - offset, type);
    }

    private void CheckIfDead()
    {
        if (state == Demons.MonsterState.Dead) return;

        if (health <= 0)
        {
            ChangeState(Demons.MonsterState.Dead);
            animator.QueueAnimation(Animation.Type.Death, false, 0, true);

            Vector3 newPos = ground;
            newPos.y += 1f;
            transform.position = newPos;

            collider.enabled = false;

            A_FaceTarget(false);

            return;
        }


        var _rng = GameController.Instance.Rntable.P_Random();
        if (_rng <= painChance)
        {
            ChangeState(Demons.MonsterState.Staggered);
            PainDuration = staggeredDuration;
            S_Pain = true;
            animator.QueueAnimation(Animation.Type.Pain, false, 0, true);
            //Debug.Log("Staggered");
            //StopCoroutine("Stagger");
            //StartCoroutine("Stagger");
        }
    }
    #endregion

    #region ATTACK_CHECK_PROPERTIES
    bool In_Melee_Range
    {
        get
        {
            if (Vector3.Distance(transform.position, CurrentTarget.transform.position) < meleeRange) return true;
            else return false;
        }
        set { }
    }
    bool In_Attack_Range
    {
        get
        {
            if (Vector3.Distance(transform.position, CurrentTarget.transform.position) < attackRange) return true;
            else return false;
        }
        set { }
    }
    bool IsInRange
    {
        get
        {
            if (Vector3.Distance(transform.position, CurrentTarget.transform.position) < sightRange) return true;
            else return false;
        }
        set { }
    }
    bool InLineOfSight
    {
        get
        {
            if (CurrentTarget == null) return false;
            return W_FindTarget.InLineOfSight(transform.position, CurrentTarget.transform.position);
        }
        set { }
    }
    bool CanReact
    {
        get
        {
            return reactionTime <= 0;
        }
        set { }
    }
    #endregion
    #endregion

    #region MOVEMENT
    private void Wander()
    {
        if (!useMovement) return;

        Vector3 newPos = transform.position;
        newPos.y = ground.y + (collider.bounds.extents.y + groundMargin);
        transform.position = newPos;

        if (HitObstacle() || OnLedge())
        {
            currentDirection = GetNewDirection();
            return;
        }

        transform.position += currentDirection * moveSpeed * Time.deltaTime;

        MoveUpSlope();

    }
    bool HitObstacle()
    {
        Vector3 direction = currentDirection.normalized;
        Vector3 underDemon = transform.position + (Vector3.down * (collider.bounds.extents.y + 0.1f));
        Vector3 rayStartPosition = underDemon + (Vector3.up * obstacleTraversionHeight);

        bool hitObstacle = (Physics.Raycast(rayStartPosition, direction, 2f, groundLayer));

        return hitObstacle;
    }
    [SerializeField] [Range(0, 10f)] float stairsTraversionHeight = 4f;
    bool OnLedge()
    {
        Vector3 direction = currentDirection.normalized;
        Vector3 rayStartPosition = transform.position + direction;
        float groundCheckLength = stairsTraversionHeight + (collider.bounds.extents.y + 0.1f);

        bool groundIsForward = (Physics.Raycast(rayStartPosition, Vector3.down, groundCheckLength, groundLayer));

        return !groundIsForward;
    }
    Vector3 GetNewDirection()
    {
        int index = Mathf.FloorToInt(GameController.Instance.Rntable.P_Random() / 64);
        return Directions.All[index];
    }
    Vector3 GetGroundHeight()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer);

        return hit.point;
    }
    void MoveUpSlope()
    {
        RaycastHit hit;

        bool insideGround = (Physics.Raycast(transform.position, Vector3.down, out hit, collider.bounds.extents.y, groundLayer));

        if (insideGround)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + collider.bounds.extents.y + groundMargin;
            transform.position = newPosition;
        }
    }
    #endregion

    #region HELPER_FUNCTIONS
    void ChangeState(Demons.MonsterState _state)
    {
        state = _state;
    }
    public void ResetMonster()
    {
        health = maxHealth;
        ChangeState(Demons.MonsterState.Wander);
        collider.enabled = true;

        transform.position = spawnPosition;
    }
    bool PlayerIsDead()
    {
        if (GameController.Instance.input.IsDead)
        {
            ChangeState(Demons.MonsterState.Wander);
            FoundEnemy = false;
            return true;
        }
        else return false;
    }
    private void IncreaseReactionTime()
    {
        reactionTime = reactionTime > 0 ? (reactionTime - 1 * Time.deltaTime) : 0;
    }
    #endregion

    #region COROUTINES
    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();

        state = spawnState;

        #region Set Textures
        animator.animationSprites = demonData.animation_textures;
        projectileSprites = demonData.projectile_textures;

        #endregion

        #region Set Movement Properties
        moveSpeed = demonData.moveSpeed;
        obstacleTraversionHeight = demonData.obstacleTraversionHeight;
        groundMargin = demonData.groundMargin;
        animationSpeed = demonData.animationSpeed;
        #endregion

        #region Set Defensive Properties
        health = demonData.health;
        maxHealth = health;
        painChance = demonData.painChance;
        staggeredDuration = demonData.staggeredDuration;
        #endregion

        #region Set Combat Properties
        projectilePrefab = demonData.projectilePrefab;
        attackType = demonData.attackType;
        projectileType = demonData.projectileType;

        CurrentTarget = GameController.Instance.DoomGuy;
        attackRange = demonData.attackRange;
        meleeRange = demonData.meleeRange;
        sightRange = demonData.sightRange;

        damage = demonData.damage;
        damageRolls = demonData.damageRolls;
        attackSpeed = demonData.attackSpeed;
        #endregion

        #region Layer Masks
        groundLayer = demonData.groundLayer;
        hittableLayer = demonData.hittableLayer;
        #endregion

        #region Audio
        if (demonData.audioClips[0] != null) demonAudio.clip = demonData.audioClips[0];
        #endregion

        #region Initialization
        initialSize = demonData.size;
        transform.localScale = initialSize;
        currentDirection = GetNewDirection();

        isInitialized = true;
        
        #endregion
    }
    #endregion
}