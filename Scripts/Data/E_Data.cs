using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DemonData", menuName = "CreatureData/Demons", order = 1)]
public class E_Data : ScriptableObject
{
    [Header("Collision Properties")]
    public LayerMask groundLayer;
    public LayerMask hittableLayer;

    public GameObject projectilePrefab;

    [Header("Demon Properties")]
    [Range(0, 50f)] public float moveSpeed = 5f;
    public float obstacleTraversionHeight = 1f;
    public float groundMargin = 0.1f;
    public int health;
    public int painChance;
    public Vector3 size;

    public float attackRange;
    public float meleeRange;
    public float sightRange;
    public int damage;
    public int damageRolls;
    public int projectileSpread;
    public float projectileSpeed;
    public float attackSpeed;

    public Demons.AttackType attackType;
    public Demons.Projectile projectileType;

    [Range(0, 5f)] public float animationSpeed = 1f;
    [Range(0, 5f)] public float staggeredDuration = 0.3f;

    [Header("Sprite Data")]
    public Texture[] animation_textures;
    public Texture[] projectile_textures;


    [Header("Sound Data")]
    public AudioClip[] audioClips;
}
