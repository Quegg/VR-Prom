using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;

namespace Guiding.ProcessDetails.Tasks
{
    public class SubmitOrders : MonoBehaviour, ITaskDetails {
        private GuidingController myGuidingController;
        private string classNameRaw="SubmitOrders";

        [SerializeField] private GameObject laserPointer;
        [SerializeField] private GameObject submitOrdersButton;
        [SerializeField] private PathToPoint wayToSubmitButton;
        
        [SerializeField] private int numberOfMistakesBeforeFirstHelp;
        private int mistakesHappened = 0;

        [SerializeField] private float minutesBeforeHelp;
        
        //ist help active? so we need to deactivate it, when the task is done
        private bool helpShown=false;
        private Coroutine delayCoroutine;
        
        //Return the class' name
        public string GetNameRaw()
        {
            return classNameRaw;
        }
        
        
        //Show custom help for this task
        public void ShowHelp()
        {
            mistakesHappened = 0;
            helpShown = true;
            submitOrdersButton.GetComponentInChildren<OutlineController>().StartOutlineAnimation();
            wayToSubmitButton.ShowPath();
            StopCoroutine(delayCoroutine);
            
        }
        
        private void HideHelp()
        {
            
            helpShown = false;
            mistakesHappened = 0;
            submitOrdersButton.GetComponentInChildren<OutlineController>().HideOutline();
            wayToSubmitButton.HidePath();

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
            return true;
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
            if(!(delayCoroutine is null))
                StopCoroutine(delayCoroutine);

            SessionSummary summaryShow = FindObjectOfType<SessionSummary>();
            UserManagement manager = FindObjectOfType<UserManagement>();
            WalkthroughSummary summaryData = manager.CreateSummary();
            float feedbackValue=0;
            foreach (var feedback in summaryData.feedback)
            {
                feedbackValue += feedback.GetFeedbackSum();
            }

            string feedbackString;
            if (feedbackValue >= 1)
            {
                feedbackString = "positive";
            }
            else if (feedbackValue < 1 && feedbackValue > -1)
            {
                feedbackString = "neutral";
            }
            else
            {
                feedbackString = "negative";
            }
            summaryShow.ShowSummary(summaryData.userId,TimeSpan.FromSeconds(summaryData.durationSeconds).ToString(),(summaryData.numberOfWrongExecutedTasks+summaryData.numberOfErrorsWithoutForkliftHit).ToString(),feedbackString);
            laserPointer.SetActive(true);
            
            //ensure that the laserpointer will not be deactivasted, if the user leaves the radius at the laptop
            FindObjectOfType<ShowOrdersToPick>().stayActivated = true;

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
