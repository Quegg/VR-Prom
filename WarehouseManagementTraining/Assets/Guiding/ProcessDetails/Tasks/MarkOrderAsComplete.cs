using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;

namespace Guiding.ProcessDetails.Tasks
{
    public class MarkOrderAsComplete : MonoBehaviour, ITaskDetails {
        private GuidingController myGuidingController;
        private string classNameRaw="MarkOrderAsComplete";
        
        [SerializeField] private PathToPoint wayHelp;
        [SerializeField] private GameObject laptop;
        
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
            laptop.GetComponentInChildren<OutlineController>().StartOutlineAnimation();
            wayHelp.ShowPath();
            StopCoroutine(delayCoroutine);
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
            //TODO check if the right order was marked and adjust help
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
        private void HideHelp()
        {
            
            helpShown = false;
            mistakesHappened = 0;
            laptop.GetComponentInChildren<OutlineController>().HideOutline();
            wayHelp.HidePath();

        }
        
        IEnumerator DelayToActivateHelp()
        {
            yield return new WaitForSeconds(minutesBeforeHelp*60);
            myGuidingController.ShowHelpHint(this,true);
        }
    }
}
