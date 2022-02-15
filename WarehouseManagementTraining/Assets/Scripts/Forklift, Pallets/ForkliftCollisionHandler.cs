using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

public class ForkliftCollisionHandler : MonoBehaviour
{
    private ForkliftMovementController forkliftControl;

    private bool pallet1 = false;

    private bool pallet2 = false;

    public GameObject front;
    

    private ForkLiftFrontController frontController;
    // Start is called before the first frame update
    void Start()
    {
        forkliftControl = GetComponentInChildren<ForkliftMovementController>();
        frontController = front.GetComponent<ForkLiftFrontController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    /// <summary>
    /// Collision on Fork/ check if forklift entered the pallet correctly
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall")||other.gameObject.CompareTag("Rack"))
        {
            forkliftControl.SetBackStart(false);
            
        }
        else if (other.gameObject.CompareTag("PalletCollision"))
        {
            forkliftControl.SetBackStart(true);
        }
        
        
        else if(other.gameObject.CompareTag("Pallet1"))
        {
            forkliftControl.DebugPrint("First");
            pallet1 = true;
            if (pallet2)
            {
                forkliftControl.inPallet = true;
                forkliftControl.DebugPrint("both");
                frontController.SetPallet(other.transform.parent.parent);
                
            }
            else
            {
                forkliftControl.inPallet = false;
                frontController.SetPallet(null);
            }
        }
        else if(other.gameObject.CompareTag("Pallet2"))
        {
            forkliftControl.DebugPrint("Second");
            pallet2 = true;
            if (pallet1)
            {
                forkliftControl.DebugPrint("both");
                forkliftControl.inPallet = true;
                frontController.SetPallet(other.transform.parent.parent);
                
            }
            else
            {
                frontController.SetPallet(null);
            }
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Pallet1"))
        {
            pallet1 = false;
            frontController.SetPallet(null);
                
            
        }
        else if(other.gameObject.CompareTag("Pallet2"))
        {
            pallet2 = false;
            frontController.SetPallet(null);
        }
        
    }
    
    
    
}
