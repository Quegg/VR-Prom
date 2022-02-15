using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Displays : MonoBehaviour
{


    [Header(("Displays"))] [SerializeField]
    private List<GameObject> displays;

    private List<TextMeshPro> TMPs;

    public void SetTexts(string text)
    {
        foreach (var TMP in TMPs)
        {
            TMP.SetText(text);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        TMPs = new List<TextMeshPro>();
        foreach (var display in displays)
        {
            TextMeshPro[] texts = display.GetComponentsInChildren<TextMeshPro>();
            if (texts.Length != 0)
            {
                foreach (var text in texts)
                {
                    TMPs.Add(text);
                }

            }
            else
            {
                throw new Exception("Display Object does not contain a TextMeshPro component");
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
