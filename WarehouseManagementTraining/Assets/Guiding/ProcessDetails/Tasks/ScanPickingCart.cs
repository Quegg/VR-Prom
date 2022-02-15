using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;
using Orders;

namespace Guiding.ProcessDetails.Tasks
{
    public class ScanPickingCart : MonoBehaviour, ITaskDetails {
        private GuidingController myGuidingController;
        private string classNameRaw="ScanPickingCart";
        private OrderManager orderManager;
        private OutlineController pickingCartBarcodeOutline;
        private OutlineController barcodeScannerOutline;
        
        [SerializeField] private int numberOfMistakesBeforeFirstHelp;
        private int mistakesHappened = 0;
        
        [SerializeField] private PathToPoint wayToPickingCart;

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
            wayToPickingCart.ShowPath();
            if (orderManager  is null)
            {
                orderManager=FindObjectOfType<OrderManager>();
            }

            pickingCartBarcodeOutline = orderManager.GetPickingCartOfSelectedOrder().Barcode
                .GetComponentInChildren<OutlineController>();
            
            if (barcodeScannerOutline  is null)
            {
                barcodeScannerOutline = GameObject.FindObjectOfType<BarcodeScannerController>().gameObject.GetComponentInChildren<OutlineController>();
            }
            
            pickingCartBarcodeOutline.StartOutlineAnimation();
            barcodeScannerOutline.StartOutlineAnimation();
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
        
        private void HideHelp()
        {
            wayToPickingCart.HidePath();
            helpShown = false;
            mistakesHappened = 0;
            pickingCartBarcodeOutline.HideOutline();
            barcodeScannerOutline.HideOutline();

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
