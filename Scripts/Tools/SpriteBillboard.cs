using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    Camera cam;
    GameObject player;
    bool initialized = false;

    private void Start()
    {
        cam = Camera.main;
        
        StartCoroutine("Initialize");
    }

    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();

        player = GameController.Instance.DoomGuy;
        initialized = true;
    }

    private void Update()
    {
        if (!initialized) return;

        Quaternion billBoardTransform = player.transform.rotation;
        //billBoardTransform.y = player.transform.rotation.y;
        transform.rotation = billBoardTransform;
    }
}
