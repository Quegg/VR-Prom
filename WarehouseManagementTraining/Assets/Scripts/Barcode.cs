using System;
using System.Collections;
using System.Collections.Generic;
using PMLogging;
using UnityEngine;
using Guiding.LoggingEvents;

public class Barcode : MonoBehaviour
{

    public BarcodeType type;
    public GameObject highlightHolder;
    private GuidingController guidingController;
    public Animator animator;

    /// <summary>
    /// The barcode scanner scanned the barcode. Based on the item type, the barcode determines what to do
    /// </summary>
    public void OnBarcodeScanned()
    {
        if (type == BarcodeType.SmallItem)
        {
            //logger.WriteEvent(new BarcodeScanned(DateTime.Now, XesLifecycleTransition.complete, "ScanItemBarcode",transform.parent.GetComponent<SmallItem>().GetName()));
            guidingController.UserEvent(new ScanItemBarcode(transform.parent.GetComponent<SmallItem>().GetName(),DateTime.Now, XesLifecycleTransition.complete));
        }
        else if (type == BarcodeType.PickingCart)
        {
        
            guidingController.UserEvent(new ScanPickingCart(transform.parent.GetComponent<PickingCart>().GetOrderNumberExtern().ToString(),DateTime.Now, XesLifecycleTransition.complete));
            //logger.WriteEvent(new BarcodeScanned(DateTime.Now, XesLifecycleTransition.complete, "ScanPickingCart","Order number: "+transform.parent.GetComponent<PickingCart>().GetOrderNumberExtern()));
            
        }
        else if(type == BarcodeType.Robot)
        {
            guidingController.UserEvent(new ScanRobot(DateTime.Now, XesLifecycleTransition.complete));
        }
        else if (type == BarcodeType.BigItem)
        {
            BigItem itemsScript = transform.parent.GetComponent<BigItem>();
            if (!(itemsScript is null))
            {
                guidingController.UserEvent(new ScanBigItemBarcode(itemsScript.GetId(), DateTime.Now,
                    XesLifecycleTransition.complete));
            }
            
        }
        else if (type == BarcodeType.PalletPlace)
        {
            guidingController.UserEvent(new ScanPalletPlace(transform.parent.parent.GetComponent<PalletGroundController>().palletPlaceNumber.ToString(),DateTime.Now, XesLifecycleTransition.complete));
        }

        else if(type == BarcodeType.Forklift)
        {
            guidingController.UserEvent(new ScanForklift(DateTime.Now, XesLifecycleTransition.complete));
        }
        
        else
        {
            guidingController.UserEvent(new ScanUndefinedBarcode(DateTime.Now, XesLifecycleTransition.complete));
        }
    }

    private void Start()
    {
        guidingController=FindObjectOfType<GuidingController>();
        
    }

    public string GetName()
    {
        if(type==BarcodeType.SmallItem)
            return transform.parent.GetComponent<SmallItem>().GetName();
        else if(type== BarcodeType.BigItem)
        {
            return transform.parent.GetComponentInChildren<BigItem>().id;
        }

        return "unknown";
    }

    /// <summary>
    /// gets the item type (if it is on a small item) or the correct pallet place id (if it is a big item)
    /// </summary>
    /// <returns></returns>
    public string GetTypeOrPalletPlace()
    {
        if (type == BarcodeType.SmallItem)
        {
            SmallItem.ItemCategory category = transform.parent.GetComponent<SmallItem>().itemCategory;
            if (category == SmallItem.ItemCategory.fragile)
            {
                return "fragile";
            }
            else if (category == SmallItem.ItemCategory.robust)
            {
                return "robust";
            }
        }
        else if (type== BarcodeType.BigItem)
        {
            BigItem item = transform.parent.GetComponent<BigItem>();
            if (!(item is null))
               return "place nr "+item.palletPlaceID;
        }

        return "unknown";
    }

    public enum BarcodeType
    {
        SmallItem, Robot, PickingCart, BigItem, PalletPlace, Forklift
    }

    [ContextMenu("StartAnimation")]
    public void StartHighlightAnimation()
    {
        highlightHolder.SetActive(true);
        animator.SetTrigger("Start");
    }

    [ContextMenu("EndAnimation")]
    public void EndHighlightAnimation()
    {
        highlightHolder.SetActive(false);
        animator.SetTrigger("End");
    }
}
