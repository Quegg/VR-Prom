using System;
using System.Collections;
using System.Collections.Generic;
using Guiding.LoggingEvents;
using Orders;
using PMLogging;
using UnityEngine;
/*
 * Class to handle Logger calls made by Event system invocations
 */
public class ItemMoveHelper : MonoBehaviour
{
    private GuidingController guidingController;
    private SmallItem smallItem;
    public OrderManager orderManager;
    public  float secondsToWaitBeforeChecking=0.45f;
    private void Start()
    {
        guidingController=FindObjectOfType<GuidingController>();
        smallItem = GetComponent<SmallItem>();
        //orderManager = FindObjectOfType<OrderManager>();
    }

    /// <summary>
    /// The user picked up this item
    /// </summary>
    public void LogPickup()
    {
        guidingController.UserEvent(new PickUpItem(smallItem.transform.position,smallItem.GetName(),DateTime.Now, XesLifecycleTransition.complete));
    }

    /// <summary>
    /// The user placed this item
    /// </summary>
    public void LogDetach()
    {
        if (orderManager is null)
        {
            orderManager = FindObjectOfType<OrderManager>();
        }
        orderManager.LastPlacedSmallObject = gameObject;
        //logger.WriteEvent(new PlaceItem(smallItem.transform.position,DateTime.Now,XesLifecycleTransition.complete, new XesString(smallItem.GetName())));
        StartCoroutine(WaitToCheckCorrectness());
    }

   
    private IEnumerator WaitToCheckCorrectness()
    {
       yield return new WaitForSeconds(secondsToWaitBeforeChecking);
        CheckForCorrectness();
        
    }

    /// <summary>
    /// Check, if the item was placed correctly. Needed to detect the error that the item was placed wrong in the picking cart
    /// </summary>
    /// <returns></returns>
    private void CheckForCorrectness()
    {
        if (!(smallItem.pickingCartLocation is null))
        {
            if (smallItem.itemCategory == smallItem.pickingCartLocation.category)
            {
                if (smallItem.IsPlacedInCorrectDirection())
                {
                    guidingController.UserEvent(new PlaceItemInPickingCartCorrectly(smallItem.GetName(),smallItem.pickingCartLocation.category.ToString(),DateTime.Now, XesLifecycleTransition.complete));
                    return;
                }
            }
            guidingController.UserEvent(new ItemEnteredPickingCartWrong(smallItem.GetName(),smallItem.pickingCartLocation.category.ToString(),DateTime.Now, XesLifecycleTransition.complete));
        }
    }
    
}
