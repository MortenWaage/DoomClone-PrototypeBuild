using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Billboarding Monsters Sprite is used for Billboarding")]
public class BillBoardSprite : MonoBehaviour
{   
    [ExecuteAlways]
    void Update()
    {
        if (Camera.current != null)
            transform.rotation = Camera.current.transform.rotation;          
    }
}
