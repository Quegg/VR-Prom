using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LineRendererPositionHelper : MonoBehaviour
{
    public Transform from;
    public Transform to;
    private LineRenderer line;

    public GameObject speechBubblePrefab;
    public GameObject speechBubbleFlippedPrefab;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        line.SetPositions(new[]{from.position,to.position});
    }

    public void SetLineInformation(string informationText)
    {
        Vector3 middle= Vector3.Lerp(from.position,to.position,0.5f);
        //arrow goes upwards
        if (from.position.y <= to.position.y)
        {
            Instantiate(speechBubblePrefab, middle, transform.rotation, transform).GetComponentInChildren<TextMeshProUGUI>().text=informationText;
        }
        else
        {
            Instantiate(speechBubbleFlippedPrefab, middle, transform.rotation, transform).GetComponentInChildren<TextMeshProUGUI>().text=informationText;
        }
    }

    [ContextMenu("Spawn Help")]
    public void SpawnHelp()
    {
        SetLineInformation("Test");
    }
}
