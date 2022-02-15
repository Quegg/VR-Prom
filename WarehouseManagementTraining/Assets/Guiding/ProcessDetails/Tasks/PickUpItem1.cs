using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;
using Orders;
using Valve.VR.InteractionSystem;

namespace Guiding.ProcessDetails.Tasks
{
    public class PickUpItem1 : MonoBehaviour, ITaskDetails {
        private GuidingController myGuidingController;
        private string classNameRaw="PickUpItem1";
        private List<OutlineController> outlines;

        private Robot robot;
        [SerializeField] private PathToPoint wayToPickingCart;

        [SerializeField] private int numberOfMistakesBeforeFirstHelp;
        private int mistakesHappened = 0;

        [SerializeField] private float minutesBeforeHelp;
        
        //ist help active? so we need to deactivate it, when the task is done
        private bool helpShown=false;
        private Coroutine delayCoroutine;
        private OrderManager orderManager;
        private Player player;

        //Return the class' name
        public string GetNameRaw()
        {
            return classNameRaw;
        }

        private void Start()
        {
            outlines= new List<OutlineController>();
        }


        //Show custom help for this task
        public void ShowHelp()
        {
            mistakesHappened = 0;
            wayToPickingCart.ShowPath();
            helpShown = true;
            if (robot  is null)
            {
                robot = FindObjectOfType<Robot>();
            }

            foreach (var carriageObject in robot.MyCarriage.AllObjectsInCarriage)
            {
                SmallItem itemScript = carriageObject.GetComponentInChildren<SmallItem>();
                if (!(itemScript is null))
                {
                    outlines.Add(itemScript.gameObject.GetComponentInChildren<OutlineController>());
                }
            }

            foreach (var outline in outlines)
            {
                outline.StartOutlineAnimation();
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
            wayToPickingCart.HidePath();
            helpShown = false;
            mistakesHappened = 0;
            foreach (var outline in outlines)
            {
                outline.HideOutline();
            }
            outlines.Clear();
        }


        //return, if the current task is done to check if the user can go to the next
        public bool IsDone()
        {
            if (orderManager  is null)
            {
                orderManager = FindObjectOfType<OrderManager>();
            }

            
            if (player  is null)
            {
                player = FindObjectOfType<Player>();
            }

          

            SmallItem item = null;
                
            foreach (var hand in player.hands)
            {
                try
                {
                    item = hand.currentAttachedObject.GetComponentInChildren<SmallItem>();
                    if (!(item is null))
                    {
                        break;
                    }
                }
                catch (NullReferenceException e)
                {
                    Debug.LogError("Hand NullReferenceException, hand:"+hand.name);
                }
                
            }

            if (item  is null)
            {
                item = orderManager.LastPlacedSmallObject.GetComponentInChildren<SmallItem>();
            }

            foreach (var orderItem in orderManager.ItemsNeededInCurrentOrder())
            {
                if (orderItem.Key is SmallItem)
                {
                    if (orderItem.Key.id.Equals(item.id))
                    {
                        return true;
                    }
                }
            }
            
            return false;
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
