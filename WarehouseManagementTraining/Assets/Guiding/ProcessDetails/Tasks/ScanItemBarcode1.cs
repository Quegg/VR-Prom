using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;
using Orders;
using Valve.VR.InteractionSystem;

namespace Guiding.ProcessDetails.Tasks
{
    public class ScanItemBarcode1 : MonoBehaviour, ITaskDetails {
        private GuidingController myGuidingController;
        private string classNameRaw="ScanItemBarcode1";
        
        private Player player;
        
        [SerializeField] private int numberOfMistakesBeforeFirstHelp;
        private int mistakesHappened = 0;
        [SerializeField] private PathToPoint wayToPickingCart;

        [SerializeField] private float minutesBeforeHelp;

        
        //ist help active? so we need to deactivate it, when the task is done
        private bool helpShown=false;
        private Coroutine delayCoroutine;
        private OutlineController barcodeScannerOutline;
        private OrderManager orderManager;
        private OutlineController itemOutline;

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
            if (player  is null)
            {
                player = FindObjectOfType<Player>();
            }
            if (orderManager  is null)
            {
                orderManager = FindObjectOfType<OrderManager>();
            }
            
            Barcode barcode = null;
                
            foreach (var hand in player.hands)
            {
                if(!(hand.currentAttachedObject is null))
                    barcode = hand.currentAttachedObject.GetComponentInChildren<Barcode>();
                if (!(barcode is null))
                {
                    break;
                }
            }

            if (barcode  is null)
            {
                barcode = orderManager.LastPlacedSmallObject.GetComponentInChildren<Barcode>();
            }
            if (!(barcode is null))
            {
                itemOutline=barcode.gameObject.GetComponent<OutlineController>();
            }
            itemOutline.StartOutlineAnimation();
            if (barcodeScannerOutline  is null)
            {
                barcodeScannerOutline = GameObject.FindObjectOfType<BarcodeScannerController>().gameObject.GetComponentInChildren<OutlineController>();
            }
            barcodeScannerOutline.StartOutlineAnimation();
            //throw new NoCustomHelpException();
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
            wayToPickingCart.HidePath();
           barcodeScannerOutline.HideOutline();
           itemOutline.HideOutline();
            helpShown = false;
            mistakesHappened = 0;
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
