using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;
using Orders;

namespace Guiding.ProcessDetails.Tasks
{
    public class ScanBigItemBarcode1 : MonoBehaviour, ITaskDetails {
        private GuidingController myGuidingController;
        private string classNameRaw="ScanBigItemBarcode1";
        
        private OrderManager orderManager;
        private Stock.Stock stock;
        private OutlineController scannerOutlineController;
        
        private List<OutlineController> bigItemOutlinesOfCurrentOrder;
        
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

        private void Start()
        {
            bigItemOutlinesOfCurrentOrder= new List<OutlineController>();
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

            if (stock  is null)
            {
                stock = FindObjectOfType<Stock.Stock>();
            }

            //can be null, if the scanner is deactivated
            //TODO hint to activate Scanner in this case
            if (!(scannerOutlineController is null))
            {
                scannerOutlineController.StartOutlineAnimation();
            }
            
            foreach (var neededItem in orderManager.GetBigItemsOfCurrentOrder())
            {
               
                //cache outlineControllers, so we do not need to search again for deactivating
                bigItemOutlinesOfCurrentOrder.AddRange(stock.BigItemOutlines[neededItem.id]);
                
            }

            foreach (var itemOutline in bigItemOutlinesOfCurrentOrder)
            {
                itemOutline.StartOutlineAnimation();
            }
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
            if (!(scannerOutlineController is null))
            {
                scannerOutlineController.HideOutline();
            }
            
            foreach (var outline in bigItemOutlinesOfCurrentOrder)
            {
                outline.HideOutline();
            }
            bigItemOutlinesOfCurrentOrder.Clear();
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
