using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Guiding.LoggingEvents;
using PMLogging;
using UnityEditor;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SmallItem : Item
{
    

    [SerializeField] private string name;

    [Header("How many items can be placed on one rack plate?")] [SerializeField]
    private int countX;

    [SerializeField] private int countY;
    [SerializeField] private int countZ;

    [Header("How much space is needed for the item?")] [SerializeField]
    private float offsetX;

    [SerializeField] private float offsetY;
    [SerializeField] private float offsetZ;

    [Header("Is the item fragile or robust?")] [SerializeField]
    public ItemCategory itemCategory=ItemCategory.fragile;

    [Header("Breaking threshold for fragile items")]
    public float minMagnitudeToBreak = 1f;

    public float upVectorTolerance = 0.998f;


    private bool wasSelected = false;

    private GuidingController guidingController;

    //null if not in any pickingCart
    public PickingCartLocation pickingCartLocation;

  
    private Interactable interactable;

    private void Start()
    {
        interactable = this.GetComponent<Interactable>();
        guidingController=FindObjectOfType<GuidingController>();
        
    }

    

    /// <summary>
    /// Is called by the PickingCartLocation to check, if this item is placed correctly inside the picking cart
    /// </summary>
    /// <returns></returns>
    public bool IsPlacedInCorrectDirection()
    {
        if(itemCategory==ItemCategory.fragile)
        {
            if (transform.up.y < upVectorTolerance)
            {
                return false;
            }
        }
        return true;
    }
    
    
    

    public enum ItemCategory
    {
        fragile,
        robust,
        unknown
    }


    public string GetId()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public int GetCountX()
    {
        return countX;
    }

    public int GetCountY()
    {
        return countY;
    }

    public int GetCountZ()
    {
        return countZ;
    }

    public float GetOffsetX()
    {
        return offsetX;
    }

    public float GetOffsetY()
    {
        return offsetY;
    }

    public float GetOffsetZ()
    {
        return offsetZ;
    }


    /// <summary>
    /// Check if item falls on the ground or was thrown
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (itemCategory != ItemCategory.fragile && !(interactable is null) && !(interactable.attachedToHand is null)
        ) 
            return;

        if (collision.impulse.magnitude > minMagnitudeToBreak)
        {
            guidingController.UserEvent(new ItemFallOnGround(name, DateTime.Now, XesLifecycleTransition.complete));
            //TODO Sound for falling down
        }
    }

    
    
}