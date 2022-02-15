using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Orders;
using UnityEngine;

public class ShowOrderMenuItems : MonoBehaviour
{
    private Order[] orders;
    private Dictionary<int,GameObject> ordersOnCanvas;
    private Dictionary<int, bool> ordersCompleted;

    [HideInInspector] public OrderManager orderManager;
    public GameObject Order;
    public GameObject startPoint;
    public GameObject menuItemPrefab;
    public float offsetY;

    public GameObject palletPlaces;

    void Awake()
    {
        ordersOnCanvas= new Dictionary<int, GameObject>();
        ordersCompleted = new Dictionary<int, bool>();
    }

    private void OnEnable()
    {
        if (GetComponentInChildren<OrderInMenu>() is null)
        {
            int itemsDisplayed = 0;
            orders = Order.GetComponentsInChildren<Order>();
            foreach (var order in orders)
            {
                int smallItemCount = 0;
                int bigItemCount = 0;
                foreach (var ao in order.articleOrders)
                {
                    if (ao.item.id.Contains("big"))
                    {
                        bigItemCount++;
                    }
                    else
                    {
                        smallItemCount++;
                    }
                }
                GameObject item = Instantiate(menuItemPrefab,
                    new Vector3(startPoint.transform.position.x,
                        startPoint.transform.position.y + itemsDisplayed * offsetY,
                        startPoint.transform.position.z), startPoint.transform.rotation, this.transform);
                //if(ordersOnCanvas!=null)
                item.GetComponent<OrderInMenu>().SetID(order.numberExtern);
                item.GetComponent<OrderInMenu>().bigItemPanels=palletPlaces;
                ordersOnCanvas.Add(order.numberExtern,item);
                ordersCompleted.Add(order.numberExtern,false);
                itemsDisplayed++;
                item.GetComponent<OrderInMenu>()
                    .SetText("Order Nr: " + order.numberExtern + " with " + smallItemCount + " different small items and "+bigItemCount+" big items");
            }
            
        }
    }

    private void OnDisable()
    {
        /*while (ordersOnCanvas.Count>0)
        {
            Destroy(ordersOnCanvas.First());
            ordersOnCanvas.Remove(ordersOnCanvas.First());
            //Debug.Log("count: "+ordersOnCanvas.Count);
        }*/
    }

    public void SetOrderCompletedPanel(int id, bool value)
    {
        
        
        ordersCompleted[id] = value;
        foreach (var order in ordersOnCanvas)
        {
            if (ordersCompleted[order.Key])
            {
                OrderInMenu orderScript = order.Value.GetComponent<OrderInMenu>();
                orderScript.setGrey(true);
                if(orderScript.autoGrey)
                    orderScript.setSelected(false);
               
            }
            else
            {
                order.Value.GetComponent<OrderInMenu>().setGrey(false);
            }
        }
    }

    public void OrderSelected(int id)
    {
        orderManager.selectedOrderIdExtern = id;
        foreach (var order in ordersOnCanvas)
        {
            if (order.Key != id)
            {
                order.Value.GetComponent<OrderInMenu>().setSelected(false);
            }
            else
            {
                order.Value.GetComponent<OrderInMenu>().setSelected(true);
            }
        }
    }
}
