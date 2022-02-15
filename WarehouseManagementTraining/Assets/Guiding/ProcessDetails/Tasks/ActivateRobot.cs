using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;

namespace Guiding.ProcessDetails.Tasks
{
    public class ActivateRobot : MonoBehaviour, ITaskDetails {
        private GuidingController myGuidingController;
        private string classNameRaw="ActivateRobot";
        
        public GameObject robotSwitch;
        private StartRobotButton robotButton;
        [SerializeField] private int numberOfMistakesBeforeFirstHelp;
        private int mistakesHappened = 0;

        [SerializeField] private float minutesBeforeHelp;

        [SerializeField] private PathToPoint wayHelp;
        
        //ist help active? so we need to deactivate it, when the task is done
        private bool helpShown=false;
        private Coroutine delayCoroutine;


        //Return the class' name
        public string GetNameRaw()
        {
            return classNameRaw;
        }
        
        private void Start()
        {
            robotButton = robotSwitch.GetComponentInChildren<StartRobotButton>();
            
        }
        
        
        //Show custom help for this task
        [ContextMenu("Show Help")]
        public void ShowHelp()
        {
            mistakesHappened = 0;
            helpShown = true;
            robotSwitch.GetComponentInChildren<OutlineController>().StartOutlineAnimation();
            wayHelp.ShowPath();
            StopCoroutine(delayCoroutine);
        }

        
        private void HideHelp()
        {
            
            helpShown = false;
            mistakesHappened = 0;
            robotSwitch.GetComponentInChildren<OutlineController>().HideOutline();
            wayHelp.HidePath();

        }
        
        //Is help offered in this moment?
        public bool HelpAvailable()
        {
            mistakesHappened++;
            return (mistakesHappened > numberOfMistakesBeforeFirstHelp);
        }
        
        
        
        //return, if the current task is done to check if the user can go to the next
        public bool IsDone()
        {
            //function gets called, if the event "ActivateRobot" occurs. after the evaluation is done, the field is set to true.
            //to be able to use "CheckConditions" of gateway1 and is done, the robotActive field is set after the evalutaion
            return !robotButton.IsRobotActive();
        }
        
        //Is called, when the user starts to execute this task
        public void TaskStarted()
        {
            delayCoroutine=StartCoroutine(DelayToActivateHelp());
        }
        
        //Is called, when the user starts to execute this task
        public void TaskEnded()
        {
            if (helpShown)
            {
                HideHelp();
            }
            StopCoroutine(delayCoroutine);
        }
        
        //Gets called by the GuidingController when loading the class);
        public void Initialize(GuidingController guidingController)
        {
            myGuidingController = guidingController;
            
        }

        IEnumerator DelayToActivateHelp()
        {
            yield return new WaitForSeconds(minutesBeforeHelp*60);
            myGuidingController.ShowHelpHint(this,true);
        }
    }
}
