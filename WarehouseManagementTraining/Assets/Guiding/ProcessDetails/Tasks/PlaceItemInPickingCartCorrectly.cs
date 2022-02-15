using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;
using Guiding.ProcessDetails.ErrorHelp;
using Orders;
using Valve.VR.InteractionSystem;

namespace Guiding.ProcessDetails.Tasks
{
    public class PlaceItemInPickingCartCorrectly : MonoBehaviour, ITaskDetails {
        private GuidingController myGuidingController;
        private string classNameRaw="PlaceItemInPickingCartCorrectly";
        [SerializeField] private int numberOfMistakesBeforeFirstHelp;
        private int mistakesHappened = 0;
        private OrderManager orderManager;
        private OutlineController outline;
        private ItemInPickingCartPreviewController pickingCartHelp;
        private Player player;

        [SerializeField] private float minutesBeforeHelp;

        [SerializeField] private ItemEnteredPickingCartWrong errorHelp;
        
        
        //ist help active? so we need to deactivate it, when the task is done
        private bool helpShown=false;
        private Coroutine delayCoroutine;

        private void Start()
        {
            orderManager = FindObjectOfType<OrderManager>();
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
            SmallItem lastUsedItem = null;
            if (player  is null)
            {
                player = FindObjectOfType<Player>();
            }
                
            foreach (var hand in player.hands)
            {
                try
                {
                    lastUsedItem = hand.currentAttachedObject.GetComponentInChildren<SmallItem>();
                }
                catch (NullReferenceException e)
                {
                }
                
                if (!(lastUsedItem is null))
                {
                    break;
                }
            }

            if (lastUsedItem  is null)
            {
                lastUsedItem = orderManager.LastPlacedSmallObject.GetComponentInChildren<SmallItem>();
            }
            pickingCartHelp = orderManager.GetPickingCartOfSelectedOrder()
                .GetComponentInChildren<ItemInPickingCartPreviewController>();
            if (lastUsedItem.itemCategory ==
                SmallItem.ItemCategory.fragile)
            {
                pickingCartHelp.StartAnimationFragile();
            }
            else
            {
                pickingCartHelp.StartAnimationRobust();
            }
                
            outline=lastUsedItem.gameObject.GetComponentInChildren<OutlineController>();
            outline.StartOutlineAnimation();
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
            //Event is triggered if item was placed correctly
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
            errorHelp.Deactivate();
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
            
            pickingCartHelp.EndAnimation();
            outline.HideOutline(); 
            

        }
        IEnumerator DelayToActivateHelp()
        {
            yield return new WaitForSeconds(minutesBeforeHelp*60);
            myGuidingController.ShowHelpHint(this,true);
        }
    }
}