using System;
using System.Collections;
using System.Collections.Generic;
using Guiding.LoggingEvents;
using Orders;
using PMLogging;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderInMenu : MonoBehaviour
{
    [SerializeField] private GameObject textObj;
    [SerializeField] private GameObject greyPanel;
    [SerializeField] private GameObject selectedPanel;
    public GameObject bigItemPanels;
    private GuidingController guidingController;
    

    [Header("If activated, the order will turn grey when completed")]
    public bool autoGrey;

    private int id = -1;
    private int orderNrIntern;
    private TextMeshProUGUI text;
    private ShowOrderMenuItems parent;

    private void Start()
    {
        parent = GetComponentInParent<ShowOrderMenuItems>();
        guidingController=FindObjectOfType<GuidingController>();

    }

    public void SetID(int mID)
    {
        id = mID;
        
    }

    public void SetOrderNumberIntern(int orderNumberIntern)
    {
        this.orderNrIntern = orderNumberIntern;
    }

    public int GetID()
    {
        return id;
    }

    private void Awake()
    {
        text = textObj.GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }

    public void setGrey(bool value)
    {
        if(autoGrey)
            greyPanel.SetActive(value);
    }
    
    public void setGreyByButton(bool value)
    {
        greyPanel.SetActive(value);
    }
    
    public void setSelected(bool value)
    {
        selectedPanel.SetActive(value);
    }

    [ContextMenu("SelectItem")]
    public void ButtonSelectedPressed()
    {
        guidingController.UserEvent(new SelectOrderInMenu(id.ToString(),DateTime.Now,XesLifecycleTransition.complete));
        //logger.WriteEvent(new ProcessSmallItems(DateTime.Now,XesLifecycleTransition.start ));
        parent.OrderSelected(id);



        foreach (var box in parent.Order.GetComponent<OrderManager>().GetPickingCarts())
        {
            if (box.GetOrderNumberExtern() == id)
                box.OrderItemsCanvas.SetActive(true);
            else
            {
                box.OrderItemsCanvas.SetActive(false);
            }
        }
        /*
         Was needed to show only active palletPlaces
        foreach (var groundControllerT in bigItemPanels.GetComponentsInChildren<PalletGroundController>())
        {
            if (groundControllerT.orderNumberExtern == id)
            {
                groundControllerT.visuals.SetActive(true);
                groundControllerT.visuals.GetComponentInChildren<TextMeshProUGUI>().text =
                    groundControllerT.itemId;
            }
            else
            {
                groundControllerT.visuals.SetActive(false);
            }
            */

        /*
        Debug.Log("checked pallets: other id: " + groundControllerT.orderNumberExtern + "myid: " + id);
        if (groundControllerT.orderNumberExtern == id)
        {
            groundControllerT.visuals.SetActive(true);
        }
        else 
            groundControllerT.visuals.SetActive(false);
            */
    //}
}
    
    public void ButtonDonePressed()
    {
        greyPanel.SetActive(!greyPanel.activeSelf);
        guidingController.UserEvent(new MarkOrderAsComplete(id,DateTime.Now, XesLifecycleTransition.complete));
    }
}
