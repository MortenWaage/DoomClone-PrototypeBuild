using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Lootable_Object : MonoBehaviour
{
    [SerializeField] Items.ItemType item_type;
    I_Data itemData;

    MeshRenderer rend;
    MeshCollider coll;
    AudioSource aud;

    Texture[] item_textures;
    AudioClip[] audioClips;

    bool isInitialized = false;
    
    float animationSpeed;
    float currentAnimationProgress;  
    int texture_index;
    int health;
    int armor;
    int clip;
    int shell;
    int cell;
    int rocket;
    int backpack;
    int keyBlue;
    int keyYellow;
    int keyRed;

    void Start()
    {
        GameController.Instance.map_Items.Add(gameObject);
        rend = GetComponent<MeshRenderer>();
        coll = GetComponentInChildren<MeshCollider>();
        aud = GetComponent<AudioSource>();
        itemData = GameController.Instance.items.SetItemTypeData(item_type);
        
        StartCoroutine("Initialize");
    }

    void Update()
    {
        if (!isInitialized) return;

        Animate();
    }

    void Animate()
    {
        currentAnimationProgress -= animationSpeed * Time.deltaTime;

        if (currentAnimationProgress <= 0)
        {
            currentAnimationProgress = 1;
            texture_index = texture_index < item_textures.Length-1 ? texture_index + 1 : 0;
            rend.material.SetTexture("_MainTex", item_textures[texture_index]);
        }
    }

    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();

        rend.transform.localScale = itemData.itemSize;
        item_textures = itemData.item_textures;
        animationSpeed = itemData.animationSpeed;
        audioClips = itemData.audioClips;
        
        texture_index = 0;

        health = itemData.health;
        armor = itemData.armor;
        clip = itemData.clip;
        shell = itemData.shell;
        cell = itemData.cell;
        rocket = itemData.rocket;
        backpack = itemData.backpack;
        keyBlue = itemData.keyBlue;
        keyYellow = itemData.keyYellow;
        keyRed = itemData.keyRed;

        isInitialized = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var inventory = collision.collider.GetComponent<P_Inventory>();
        if (inventory == null) return;

        if (keyBlue > 0)    inventory.AddKey(Keys.KeyType.Blue);
        if (keyYellow > 0)  inventory.AddKey(Keys.KeyType.Yellow);
        if (keyRed > 0)     inventory.AddKey(Keys.KeyType.Red);
        inventory.AddAmmo(clip, shell, cell, rocket, backpack);
        inventory.AddVitals(health, armor);

        if (audioClips.Length > 0)
        {
            aud.clip = audioClips[0];
            aud.Play();
        }

        ToggleObject(false);
    }

    public void ToggleObject(bool toggle)
    {
        rend.enabled = toggle;
        coll.enabled = toggle;
    }
}
