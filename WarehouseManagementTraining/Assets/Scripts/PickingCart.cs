using System.Collections;
using System.Collections.Generic;
using Orders;
using UnityEngine;

public class PickingCart : MonoBehaviour
{
    [SerializeField]
    private PickingCartLocation fragil;
    
    [SerializeField]
    private PickingCartLocation robust;
    
    [SerializeField]
    private PickingCartLocation unknown;

    public GameObject orderNumberSign;

    private OrderManager orderManager;

    public GameObject OrderItemsCanvas;
    
    public GameObject Barcode;

    [HideInInspector]
    public Dictionary<Item, int> allNeededItems;
    private Dictionary<string, int> allNeededItemsStringId;
    
    private int orderNumberIntern;
    private int orderNumberExtern=-1;
   

    /// <summary>
    /// check if all items in this picking cart are correctly placed
    /// </summary>
    /// <returns></returns>
    public bool AllItemsCorectInPickuingCart()
    {
        return unknown.ReturnAllItemsCorrectInPickingCartLocation() &&
               robust.ReturnAllItemsCorrectInPickingCartLocation() &&
               fragil.ReturnAllItemsCorrectInPickingCartLocation();
    }

    /// <summary>
    /// Initialize for the order
    /// </summary>
    /// <param name="orderManager"></param>
    /// <param name="orderNumberIntern"></param>
    /// <param name="ordernumberExtern"></param>
    public void SetOrderManagerAndNumber(OrderManager orderManager, int orderNumberIntern, int ordernumberExtern)
    {
        this.orderManager = orderManager;
        this.orderNumberIntern = orderNumberIntern;
        this.orderNumberExtern = ordernumberExtern;
    }

    public int GetOrderNumberIntern()
    {
        return orderNumberIntern;
    }
    public int GetOrderNumberExtern()
    {
        return orderNumberExtern;
    }

    public Dictionary<string, int> GetNeededItemsStringId()
    {
        return allNeededItemsStringId;
    }

    /// <summary>
    /// Initialize the items
    /// </summary>
    /// <param name="allNeededItems"></param>
    /// <param name="bigItems"></param>
    public void SetAllNeededItems(Dictionary<Item,int> allNeededItems,List<BigItemInOrder> bigItems)
    {
        allNeededItemsStringId = new Dictionary<string, int>();
        this.allNeededItems = allNeededItems;

        Dictionary<string, int> itemStringsFragile = new Dictionary<string, int>();
        Dictionary<string, int> itemStringsRobust =new Dictionary<string, int>();
        Dictionary<string, int> itemStringsUnknown = new Dictionary<string, int>();
        
        foreach (var item in allNeededItems)
        {
            allNeededItemsStringId.Add(item.Key.id,item.Value);
            if (((SmallItem) item.Key).itemCategory == SmallItem.ItemCategory.fragile)
            {
                itemStringsFragile.Add(item.Key.id,item.Value);
            }
            else if (((SmallItem) item.Key).itemCategory == SmallItem.ItemCategory.robust)
            {
                itemStringsRobust.Add(item.Key.id,item.Value);
            }
            else if (((SmallItem) item.Key).itemCategory == SmallItem.ItemCategory.unknown)
            {
                itemStringsUnknown.Add(item.Key.id,item.Value);
            }
        }
        
        robust.SetNeededItems(itemStringsRobust);
        fragil.SetNeededItems(itemStringsFragile);
        unknown.SetNeededItems(itemStringsUnknown);

        foreach (var bi in bigItems)
        {
            allNeededItemsStringId.Add(bi.id,1);
        }
    }
}
