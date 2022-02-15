 using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;
using Orders;
using Valve.VR.InteractionSystem;

namespace Guiding.ProcessDetails.ErrorHelp
{
    public class ItemEnteredPickingCartWrong : MonoBehaviour, IErrorHelp {
        
        private string className="ItemEnteredPickingCartWrong";
        private GuidingController myGuidingController;
        
        [SerializeField] private int numberOfMistakesBeforeFirstHelp;
        private int mistakesHappened = 0;
        private OrderManager orderManager;
        private OutlineController outline;
        private ItemInPickingCartPreviewController pickingCartHelp;
        private Player player;

        [SerializeField] private float minutesBeforeHelp;
        private bool helpShown;

        private void Start()
        {
            orderManager = FindObjectOfType<OrderManager>();
        }

        //Return the class' name
        public string GetName()
        {
            return className;
        }

        //Gets called by the GuidingController when loading the class);
        public void Initialize(GuidingController guidingController)
        {
            myGuidingController = guidingController;
            
        }

        //Show Help, if this error occurs. Else Help for current task is shown
        public void ShowHelp()
        {
            helpShown = true;
            SmallItem lastUsedItem = null;
            if (player  is null)
            {
                player = FindObjectOfType<Player>();
            }
                
            //gets the last item, the player had in his hands
            foreach (var hand in player.hands)
            {
                if (!(hand is null))
                {
                    try
                    {
                        lastUsedItem = hand.currentAttachedObject.GetComponentInChildren<SmallItem>();
                    }
                    catch (NullReferenceException e)
                    {
                    }
                    
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
            //get designated picking cart for the item
            pickingCartHelp = orderManager.GetPickingCartOfSelectedOrder().GetComponentInChildren<ItemInPickingCartPreviewController>();
            
            //activate the animation
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

        }
        //Is help offered in this moment?
        public bool HelpAvailable()
        {
            mistakesHappened++;
            return (mistakesHappened > numberOfMistakesBeforeFirstHelp);
        }

        public void Deactivate()
        {
            if (helpShown)
            {
                helpShown = false;
                helpShown = false;
                mistakesHappened = 0;
            
                pickingCartHelp.EndAnimation();
                outline.HideOutline();
            }
        }
        
    }
}