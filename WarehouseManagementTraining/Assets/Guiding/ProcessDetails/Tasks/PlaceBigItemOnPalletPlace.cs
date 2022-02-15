using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;

namespace Guiding.ProcessDetails.Tasks
{
    public class PlaceBigItemOnPalletPlace : MonoBehaviour, ITaskDetails {
        private GuidingController myGuidingController;
        private string classNameRaw="PlaceBigItemOnPalletPlace";
        
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
        
        
        //Show custom help for this task
        public void ShowHelp()
        {
            mistakesHappened = 0;
            helpShown = true;
            wayHelp.ShowPath();
            StopCoroutine(delayCoroutine);
        }
        //Is help offered in this moment?
        public bool HelpAvailable()
        {
            return true;
        }
        
        private void HideHelp()
        {
            
            helpShown = false;
            mistakesHappened = 0;
            wayHelp.HidePath();

        }
        
        
        //return, if the current task is done to check if the user can go to the next
        public bool IsDone()
        {
            return true;
            //TODO check if the item was placed on the right pallet place
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
