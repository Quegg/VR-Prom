using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionSummary : MonoBehaviour
{
    public TextMeshProUGUI iUserId;
    public TextMeshProUGUI iDuration;
    public TextMeshProUGUI iErrors;
    public TextMeshProUGUI iFeedbackSummary;
    public GameObject canvas;


    private void Start()
    {
        canvas.SetActive(false);
    }


    [ContextMenu("Show Summary")]
    public void Show()
    {
        ShowSummary("000112","10 Stunden","4003","supergut");
    }
    
    public void ShowSummary(string userId, string duration, string errors, string feedback)
    {
        iUserId.text = userId;
        iDuration.text = duration;
        iErrors.text = errors;
        iFeedbackSummary.text = feedback;
        canvas.SetActive(true);
    }
}
