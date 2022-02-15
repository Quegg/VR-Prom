using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;
using Orders;

namespace Guiding.ProcessDetails.Tasks
{
    public class ScanPalletPlace : MonoBehaviour, ITaskDetails {
        private GuidingController myGuidingController;
        private string classNameRaw="ScanPalletPlace";
        
        private OrderManager orderManager;
        private OutlineController scannerOutlineController;
        
        private OutlineController palletPlaceBarcode;
        [SerializeField] private GameObject palletPlaces;
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
            if (scannerOutlineController  is null)
            {
                scannerOutlineController = GameObject.FindObjectOfType<BarcodeScannerController>().gameObject.GetComponentInChildren<OutlineController>();
            }
            
            if (orderManager  is null)
            {
                orderManager = FindObjectOfType<OrderManager>();
            }

            
            //can be null, if the scanner is deactivated
            //TODO hint to activate Scanner in this case
            if (!(scannerOutlineController is null))
            {
                scannerOutlineController.StartOutlineAnimation();
            }
            BigItem item=null;
            
            
            foreach (var neededItem in orderManager.GetBigItemsOfCurrentOrder())
            {

                item = neededItem;
                break;
                
            } 

            foreach (var gControl in palletPlaces.GetComponentsInChildren<PalletGroundController>())
            {
                if (gControl.itemId == item.id)
                {
                    palletPlaceBarcode=gControl.gameObject.GetComponentInChildren<OutlineController>();
                    break;
                }
            }
            palletPlaceBarcode.StartOutlineAnimation();
            StopCoroutine(delayCoroutine);
            
        }
        //Is help offered in this moment?
        public bool HelpAvailable()
        {
            mistakesHappened++;
            return (mistakesHappened > numberOfMistakesBeforeFirstHelp);
        }
        private void HideHelp()
        {
            helpShown = false;
            mistakesHappened = 0;
            scannerOutlineController.HideOutline();
            palletPlaceBarcode.HideOutline();
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