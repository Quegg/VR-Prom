using System;
using System.Collections;
using System.Collections.Generic;
using Orders;
using UnityEngine;

public class ShowOrderItemsShippingBox : MonoBehaviour
{
    public GameObject orderItemShippingBoxPrefab;
    private Dictionary<string,int> items;
    private Dictionary<string, GameObject> itemsOnCanvas;
    private Dictionary<string, int> itemStillNeeded;


    //public GameObject Order;
    public GameObject startPoint;
    public float offsetY;

    void Awake()
    {
        itemsOnCanvas= new Dictionary<string, GameObject>();
    }

    private void OnEnable()
    {
        if(GetComponentsInChildren<OrderInShippingBox>().Length==0)
            ShowItemList();
    }

    public void ShowItemList()
    {
        int itemsDisplayed = 0;
        items = transform.parent.GetComponent<PickingCart>().GetNeededItemsStringId();
        //itemStillNeeded = new Dictionary<string, int>(items);
        foreach (var item in items)
        {
            GameObject mItem = Instantiate(orderItemShippingBoxPrefab,
                new Vector3(startPoint.transform.position.x, startPoint.transform.position.y + itemsDisplayed * offsetY,
                    startPoint.transform.position.z), startPoint.transform.rotation, this.transform);
            //if(ordersOnCanvas!=null)
            itemsOnCanvas.Add(item.Key,mItem);
            itemsDisplayed++;
            mItem.GetComponent<OrderInShippingBox>().SetText(item.Key,item.Value.ToString());
        }
    }
    
    public void UpdateItemCount(string id, int change)
    {
        int newValue=Mathf.Max(0, itemStillNeeded[id] + change);
        itemStillNeeded[id] = newValue;
        itemsOnCanvas[id].GetComponent<OrderInShippingBox>().UpdateAmount(newValue.ToString());
        if (newValue == 0)
        {
            itemsOnCanvas[id].GetComponent<OrderInShippingBox>().setGrey(true);
        }
        else
        {
            itemsOnCanvas[id].GetComponent<OrderInShippingBox>().setGrey(false);
        }
    }
    
}
