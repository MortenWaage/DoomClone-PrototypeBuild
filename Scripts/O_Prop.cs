using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Prop : MonoBehaviour
{
    [SerializeField] Props.PropType type;

    O_Data propData;

    Texture[] textures;
    AudioClip[] audioClips;
    MeshRenderer rend;

    Vector3 itemSize;

    bool walkable;
    bool attackable;

    bool loopAnimation;
    float animationSpeed = 1f;
    float animationProgress;    

    void Start()
    {
        rend = GetComponentInChildren<MeshRenderer>();
         
        propData = GameController.Instance.props.SetItemTypeData(type);

        walkable = propData.walkable;
        attackable = propData.walkable;

        if (walkable) GetComponent<BoxCollider>().enabled = false;

        textures = propData.item_textures;

        loopAnimation = propData.loopAnimation;
        itemSize = propData.itemSize;
        animationSpeed = propData.animationSpeed;

        if (loopAnimation)
        {
            var _rng = GameController.Instance.Rntable.P_Random();
            var frameLength = 256 / textures.Length;
            animationProgress = _rng / frameLength;
        }
        else
            rend.material.SetTexture("_MainTex", textures[0]);

        transform.localScale = itemSize;
    }

    private void Update()
    {
        if (loopAnimation)
        {
            animationProgress += animationSpeed * Time.deltaTime;

            if (animationProgress >= textures.Length) animationProgress = 0;

            var index = Mathf.FloorToInt(animationProgress);
            rend.material.SetTexture("_MainTex", textures[index]);
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position, new Vector3(2f,2f,2f));
    }
}
