using System;
using System.Collections;
using System.Collections.Generic;
using Guiding.LoggingEvents;
using PMLogging;
using TMPro;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HUDController : MonoBehaviour
{
    public GameObject vrCamera;
    public GuidingController controller;

    public GameObject helper;
    public GameObject helpTextObject;
    public GameObject background;
    public GameObject feedback;
    
    //public SteamVR_Action_Boolean helpPositive;
    public SteamVR_Action_Boolean helpNegative;

    public SteamVR_Input_Sources inputSource;

    private ProcessHelpContainer helperScript;

    private Coroutine hintCoroutine;
    private Valve.VR.InteractionSystem.Player player;
    private bool showHint;

    private TextMeshProUGUI helpText;

    private XesLogger logger;

    private string nameOfGuidedTask;
    private bool isGuiding;
    private bool showingFeedbackRequest;

    private string feedbackSection;
    
    

    // Start is called before the first frame update
    void Start()
    {
        logger=XesLogger.GetLogger;
        helpText = helpTextObject.GetComponent<TextMeshProUGUI>();
        helpText.text = "";
        
        
        TurnOff();
        
        player = GetComponentInParent<Valve.VR.InteractionSystem.Player>();
        
        
    }

    

    private void OnDestroy()
    {
        helpNegative.RemoveOnStateDownListener(OnNegativeFeedbackDown,inputSource);
        //helpPositive.RemoveOnStateDownListener(OnPositiveFeedbackDown,inputSource);
    }

    private void Awake()
    {
        if (controller  is null)
        {
            controller = FindObjectOfType<GuidingController>();
        }
        //helpPositive.AddOnStateDownListener(OnPositiveFeedbackDown,inputSource); 
        helpNegative.AddOnStateDownListener(OnNegativeFeedbackDown,inputSource);
    }

    
    

    /// <summary>
    /// turns on the canvas
    /// </summary>
    public void TurnOn()
    {
        background.SetActive(true);
        //feedback.SetActive(true);
    }
    
    /// <summary>
    /// turns off all objects for the canvas
    /// </summary>
    public void TurnOff()
    {
        helpTextObject.SetActive(false);
        helper.SetActive(false);
        background.SetActive(false);
        //feedback.SetActive(false);
    }
    

    
    /// <summary>
    /// Hide hint for closing canvas
    /// </summary>
    public void HideHints()
    {
        //ControllerButtonHints.HideTextHint( player.leftHand, helpPositive);
        ControllerButtonHints.HideTextHint( player.leftHand, helpNegative);
    }

    private void FixedUpdate()
    {
        //transform.LookAt(vrCamera.transform);
    }

    

    /// <summary>
    /// Show generic help (all possible following tasks)
    /// </summary>
    /// <param name="state">state in which the user is</param>
    public void ShowProcessInformation(BpmnElement state)
    {
        helpTextObject.SetActive(false);
        isGuiding = true;
        nameOfGuidedTask = state.nameCut;
        if (helperScript  is null)
        {
            helperScript=helper.GetComponent<ProcessHelpContainer>();
        }
        //gameObject.SetActive(false);
        transform.rotation=Quaternion.identity;
        helperScript.ShowProcessInformation(state);
        transform.LookAt(vrCamera.transform);
        var rotation = transform.rotation;
        rotation=Quaternion.Euler(new Vector3(0,rotation.eulerAngles.y,rotation.eulerAngles.z));
        transform.rotation = rotation;
        
        //gameObject.SetActive(true);
        helper.SetActive(true);
        ControllerButtonHints.ShowTextHint(player.leftHand,helpNegative,"Close");
        TurnOn();
    }

    //eventName: Name of the event which is guided trough this text
    /// <summary>
    /// Shows a help text to the user
    /// </summary>
    /// <param name="text">text to show</param>
    /// <param name="eventName">name of the event providing this help</param>
    public void ShowTextHelp(string text, string eventName)
    {
        TurnOff();
        isGuiding = true;
        nameOfGuidedTask = eventName;
        helpText.text = text;
        helpTextObject.SetActive(true);
        ControllerButtonHints.ShowTextHint(player.leftHand,helpNegative,"Close");
        //ShowFeedbackHints();
        TurnOn();


    }

    /*public void OnPositiveFeedbackDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (isGuiding)
        {
        
            logger.WriteEvent(new PositiveFeedback(nameOfGuidedTask,DateTime.Now, XesLifecycleTransition.complete));
            HideHints();
            isGuiding = false;
            TurnOff();
        }
        
        if(!isGuiding)
        {
            //controller.ShowGenericHelp();
        }
        
        
        Debug.Log("positive Down");
    }*/
    
    /// <summary>
    /// User pressed the button to close the hint/feedback canvas
    /// </summary>
    /// <param name="fromAction"></param>
    /// <param name="fromSource"></param>
    public void OnNegativeFeedbackDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (showingFeedbackRequest)
        {
            
            //logger.WriteEvent(new NegativeFeedback(nameOfGuidedTask,0,DateTime.Now, XesLifecycleTransition.complete));
            HideHints();
            showingFeedbackRequest = false;
        }

        if (isGuiding)
        {
            HideHints();
            isGuiding=false;
        }
        TurnOff();

        //Debug.Log("negative Down");
    }

    public void CloseFeedbackRequest()
    {
        HideHints();
        showingFeedbackRequest = false;
        TurnOff();
    }


    // [ContextMenu("Show Text")]
    // public void ShowText()
    // {
    //     ShowTextHelp("Du bist dumm. Deswegen brauchst du hilfe. Die bekommst du aber nicht lol.","lul");
    // }
    
    public void ShowFeedbackRequest(string section)
    {
        feedbackSection = section;
        showingFeedbackRequest = true;
        TurnOff();
        helpText.text = "Did the guidance help you so far? Say \"yes\" or \"no\". To ignore, press the button on your controller.";
        ControllerButtonHints.ShowTextHint(player.leftHand,helpNegative,"Skip Feedback");
        helpTextObject.SetActive(true);
        controller.VoiceRecognizer.StartListening();
        TurnOn();
    }
    
}
