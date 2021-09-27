using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugInfo : MonoBehaviour
{
    int debugDisplayLength = 8;

    [SerializeField] Text text_1;
    [SerializeField] Text text_2;
    [SerializeField] Text text_3;
    [SerializeField] Text text_4;
    [SerializeField] Text text_5;
    [SerializeField] Text text_6;
    [SerializeField] Text text_7;
    [SerializeField] Text text_8;

    Text[] debugDisplays;

    private void Start()
    {
        debugDisplays = new Text[debugDisplayLength];
        debugDisplays[0] = text_1;
        debugDisplays[1] = text_2;
        debugDisplays[2] = text_3;
        debugDisplays[3] = text_4;
        debugDisplays[4] = text_5;
        debugDisplays[5] = text_6;
        debugDisplays[6] = text_7;
        debugDisplays[7] = text_8;

        int pos = 1;
        float height = 30f;
        foreach(Text text in debugDisplays)
        {
            Vector2 newHeight = text.transform.position;
            newHeight.y -= (pos * height) - 20;
            text.transform.position = newHeight;
            text.text = "";
            text.enabled = false;
            pos++;
        }
    }

    bool isEnabled = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            isEnabled = !isEnabled;
            foreach (Text text in debugDisplays)
            {
                text.enabled = isEnabled;
            }
        }
    }

    public void SetText(int number, string text)
    {
        debugDisplays[number - 1].text = text;
    }
}


