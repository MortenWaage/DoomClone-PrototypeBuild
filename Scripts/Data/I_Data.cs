using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData/Items", order = 1)]
public class I_Data : ScriptableObject
{
    public GameObject itemPrefab;    
    
    [Header("Item Properties")]
    public Items.ItemType type;
    public Vector3 itemSize;
    [Header("Stat Modifiers")]
    public int health;
    public int armor;
    public int clip;
    public int shell;
    public int cell;
    public int rocket;
    public int backpack;
    public int keyBlue;
    public int keyYellow;
    public int keyRed;

    [Range(0, 5f)] public float animationSpeed = 1f;

    [Header("Sprite Data")]
    public Texture[] item_textures;

    [Header("Sound Data")]
    public AudioClip[] audioClips;
}
