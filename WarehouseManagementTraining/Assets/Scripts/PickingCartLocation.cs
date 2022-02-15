using System;
using System.Collections;
using System.Collections.Generic;
using Guiding.LoggingEvents;
using PMLogging;
using UnityEngine;

public class PickingCartLocation : MonoBehaviour
{
    public SmallItem.ItemCategory category;

    //TODO give needed items to pickingcartlocation

    [HideInInspector]
    public Dictionary<string, int> neededItemCounts;

    private GuidingController guidingController;
    private Dictionary<string, int> itemCounts;

    private List<SmallItem> itemScriptObjects;
    // Start is called before the first frame update
    void Start()
    {
        guidingController=FindObjectOfType<GuidingController>();
        
    }
    
    /// <summary>
    /// Initialize the items for this box
    /// </summary>
    /// <param name="items"></param>
    public void SetNeededItems(Dictionary<string,int> items)
    {
        itemCounts = new Dictionary<string, int>();
        itemScriptObjects = new List<SmallItem>();
        neededItemCounts = items;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            SmallItem smallItemScript = other.GetComponent<SmallItem>();
            itemScriptObjects.Add(smallItemScript);
            if (itemCounts.ContainsKey(smallItemScript.GetId()))
            {
                itemCounts[smallItemScript.GetId()]++;
                
            }
            else
            {
                itemCounts.Add(smallItemScript.GetId(),1);
            }
            itemScriptObjects.Add(smallItemScript);
            smallItemScript.pickingCartLocation = this;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            SmallItem smallItemScript = other.GetComponent<SmallItem>();
            smallItemScript.pickingCartLocation = null;
            itemScriptObjects.Remove(smallItemScript);
            itemCounts[smallItemScript.GetId()]--;
            if (itemCounts[smallItemScript.GetId()] == 0)
                itemCounts.Remove(smallItemScript.GetId());
            itemScriptObjects.Remove(smallItemScript);
            guidingController.UserEvent(new ItemLeftPickingCart(smallItemScript.GetName(),DateTime.Now, XesLifecycleTransition.complete));
        }
    }

    /// <summary>
    /// Return, if all needed items are in this box, and if they are placed correctly
    /// </summary>
    /// <returns></returns>
    public bool ReturnAllItemsCorrectInPickingCartLocation()
    {
        //not necessary, return early if incorrect
        if (itemCounts.Keys.Count != neededItemCounts.Keys.Count)
            return false;
        foreach (var needed in neededItemCounts)
        {
            
            if (!itemCounts.ContainsKey(needed.Key))
                return false;
            
            if (itemCounts[needed.Key] != needed.Value)
                return false;

        }
        foreach (var item in itemScriptObjects)
        {
            if (!item.IsPlacedInCorrectDirection())
                return false;
        }

        return true;
    }
}
