using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "ItemData/Objects", order = 1)]
public class O_Data : ScriptableObject
{
    [Header("Object Flags")]
    public bool loopAnimation;
    public bool walkable;
    public bool attackable;

    [Header("Object Properties")]

    public Props.PropType type;
    public Vector3 itemSize;
    [Range(0, 20f)] public float animationSpeed = 1f;

    [Header("Sprite Data")]
    public Texture[] item_textures;

    [Header("Sound Data")]
    public AudioClip[] audioClips;
}
