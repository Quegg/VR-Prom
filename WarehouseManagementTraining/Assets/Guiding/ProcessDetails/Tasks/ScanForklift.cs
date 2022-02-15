using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;

namespace Guiding.ProcessDetails.Tasks
{
    public class ScanForklift : MonoBehaviour, ITaskDetails {
        private GuidingController myGuidingController;
        private string classNameRaw="ScanForklift";
        [SerializeField] private GameObject barcodeObject;
        private OutlineController barcodeOutline;
        
        [SerializeField] private int numberOfMistakesBeforeFirstHelp;
        private int mistakesHappened = 0;
        private OutlineController barcodeScannerOutline;

        [SerializeField] private float minutesBeforeHelp;
        
        
        //ist help active? so we need to deactivate it, when the task is done
        private bool helpShown=false;
        private Coroutine delayCoroutine;

        private void Start()
        {
            barcodeOutline = barcodeObject.GetComponentInChildren<OutlineController>();
        }

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
            if (barcodeScannerOutline  is null)
            {
                barcodeScannerOutline = GameObject.FindObjectOfType<BarcodeScannerController>().gameObject.GetComponentInChildren<OutlineController>();
            }
            barcodeOutline.StartOutlineAnimation();
            //can be null, if the scanner is deactivated
            //TODO hint to activate Scanner in this case
            if (!(barcodeScannerOutline is null))
            {
                barcodeScannerOutline.StartOutlineAnimation();
            }
            StopCoroutine(delayCoroutine);
        }
        
        private void HideHelp()
        {
            
            helpShown = false;
            mistakesHappened = 0;
            barcodeOutline.HideOutline();
            barcodeScannerOutline.HideOutline();

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
