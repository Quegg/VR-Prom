using System;
using System.Collections;
using System.Collections.Generic;
using Orders;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

public class PalletController : MonoBehaviour
{
    private Vector3 lastPosition;

    private List<GameObject> palletGroundControllers;

    [HideInInspector] public ForkliftMovementController mainForkliftController;
    

    
    private Transform[] verticies;
    //[HideInInspector]
    //public GameObject orderManagerObject;
    private OrderManager orderManager;

    private BigItem myBigItemScript;
    private PalletGroundController.MarkerState lastState=PalletGroundController.MarkerState.Empty;
    
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        palletGroundControllers = new List<GameObject>();
        verticies = GetComponentsInChildren<Transform>();
        //orderManager = orderManagerObject.GetComponent<OrderManager>();
        myBigItemScript = transform.parent.gameObject.GetComponentInChildren<BigItem>();
    }

    // Update is called once per frame
    void Update()
    {
        //pallet has been moved
        
        if (palletGroundControllers.Count!=0&&transform.position != lastPosition)
        {
            lastPosition = transform.position;
            CheckBounds(); 
            //Debug.Log("Checked Bounds");
            
        }
    }

    /// <summary>
    /// Check if the pallet entered a pallet place, or hit a wall
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PalletGround"))
        {
            palletGroundControllers.Add(other.gameObject);
            
        }
        else if(other.gameObject.CompareTag("Wall"))
        {
            if(mainForkliftController!=null)
                mainForkliftController.SetBackStart(false);
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("PalletGround"))
        {
            palletGroundControllers.Remove(other.gameObject);
            other.gameObject.GetComponent<PalletGroundController>().SetMarkerState(PalletGroundController.MarkerState.Empty);
            //lastState = PalletGroundController.MarkerState.Empty;

        }
    }


    /// <summary>
    /// Check, if the pallet is completely in a pallet place
    /// </summary>
    private void CheckBounds()
    {
        
        foreach (var controller in palletGroundControllers)
        {
            //Debug.Log("controller Loop");
            bool isIntersecting = false;
            bool isCompletelyIn = true;
            foreach (var vertex in verticies)
            {
                //Debug.Log("vertex Loop");
                if (controller.GetComponent<BoxCollider>().bounds.Contains(vertex.position))
                {
                    //;
                    isIntersecting = true;
                    //Debug.Log("is intersecting");
                }
                else
                {
                    isCompletelyIn = false;
                }
            }
            //Debug.Log("is intersecting: "+isIntersecting);

            //Inform the Pallet place if the pallet is in or not
            if (isCompletelyIn)
            {
                PalletGroundController palletGroundController=controller.GetComponent<PalletGroundController>();
                //if (lastState == PalletGroundController.MarkerState.Partly)
                {
                    //lastState = PalletGroundController.MarkerState.Completely;
                    palletGroundController.SetMarkerState(PalletGroundController.MarkerState.Completely);
                    palletGroundController.PalletEntered(myBigItemScript);
                }
            }
            else if(isIntersecting)
            {
                PalletGroundController palletGroundController=controller.GetComponent<PalletGroundController>();
                //if (lastState == PalletGroundController.MarkerState.Completely|| lastState==PalletGroundController.MarkerState.Empty)
                {
                    //lastState = PalletGroundController.MarkerState.Partly;
                    palletGroundController.SetMarkerState(PalletGroundController.MarkerState.Partly);
                    palletGroundController.PalletLeft(myBigItemScript);
                }
                
            }

        }
        
    }
}
