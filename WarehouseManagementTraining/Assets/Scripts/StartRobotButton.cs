using System;
using System.Collections;
using System.Collections.Generic;
using Guiding.LoggingEvents;
using PMLogging;
using TMPro;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class StartRobotButton : MonoBehaviour
{
    private GuidingController guidingController;
    private bool robotIsActive = false;
    private TextMeshPro display;
    private float lastPress;

    [SerializeField] private GameObject teleportObj;
    [SerializeField] private GameObject displayObj;

    [Header("Time between two presses")]
    public float pressDelay;

    private Teleport teleport;

    private AudioSource beep;
    // Start is called before the first frame update
    void Start()
    {
        display = displayObj.GetComponent<TextMeshPro>();
        teleport = teleportObj.GetComponent<Teleport>();
        lastPress = Time.time;
        guidingController=FindObjectOfType<GuidingController>();
        beep = GetComponent<AudioSource>();
    }

   
    /// <summary>
    /// The Player pressed the button for activating the robot
    /// </summary>
    public void OnButtonPressed()
    {
        //wait some time, before the button can be pressed again
        if (Time.time>=(lastPress+pressDelay))
        {
            
            if (!robotIsActive)
            {
                guidingController.UserEvent(new ActivateRobot(DateTime.Now, XesLifecycleTransition.complete));
                lastPress = Time.time;
                robotIsActive = true;
                teleport.EnableRobot();
                display.SetText("Robot \n \n activated");
            }
            else
            {
                lastPress = Time.time;
                robotIsActive = false;
                teleport.DisableRobot();
                display.SetText("Robot \n \n deactivated");
                //logger.WriteEvent(new DeactivateCarryingRobot(DateTime.Now, XesLifecycleTransition.complete));
            }
            beep.Play();
        }
    }

    public void OnButtonUp()
    {
        Debug.Log("Button released");
    }
    
    public void OnButtonDown()
    {
        Debug.Log("Button down");
    }

    public bool IsRobotActive()
    {
        return robotIsActive;
    }
}
