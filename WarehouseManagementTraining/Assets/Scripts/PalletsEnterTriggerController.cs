using System;
using System.Collections;
using System.Collections.Generic;
using Guiding.LoggingEvents;
using PMLogging;
using UnityEngine;

/// <summary>
/// Is used to determine, when the player arrived at the pallets with the forklift, so it triggers the corresponding event
/// It should only be triggered, if the player enters the trigger ON the forklift.
/// It uses two trigger. If the player AND the forklift, first inter the  outer trigger and then the inner, the event is recorded
/// </summary>
public class PalletsEnterTriggerController : MonoBehaviour
{

    private List<GameObject> spheres;
    private GuidingController guidingController;

    private bool playerWentIn;
    private bool forkliftWentIn;
    private bool isPlayerInner;

    private bool isPlayerOuter;
    
    
    private bool isForkliftInner;

    private bool isForkliftOuter;
    private float elapsed;
    public bool debug;
    

    private void Start()
    {
        guidingController=FindObjectOfType<GuidingController>();
        spheres=new List<GameObject>();
    }

    public void OnForkliftEnterInner()
    {
        isForkliftInner = true;
        if (isForkliftOuter)
        {
            forkliftWentIn = true;
            Check();
        }

        
    }

    public void OnForkliftLeftInner()
    {
        isForkliftInner = false;
    }
    
    public void OnForkliftEnterOuter()
    {
        isForkliftOuter = true;
        if (isForkliftInner)
        {
            forkliftWentIn = false;
            Check();
        }
    }

    public void OnForkliftLeftOuter()
    {
        isForkliftOuter = false;
    }
    
    public void OnPlayerEnterInner()
    {
        isPlayerInner = true;
        if (isPlayerOuter)
        {
            playerWentIn = true;
            Check();
        }
    }

    public void OnPlayerLeftInner()
    {
        isPlayerInner = false;
    }
    
    public void OnPlayerEnterOuter()
    {
        isPlayerOuter = true;
        if (isPlayerInner)
        {
            playerWentIn = false;
            Check();
        }
    }

    public void OnPlayerLeftOuter()
    {
        isPlayerOuter = false;
    }

    void Check()
    {
        
        if (playerWentIn && forkliftWentIn)
        {
            Entered();
        }
    }

    private void Update()
    {
        if (debug)
        {
            elapsed += Time.deltaTime;
            if (elapsed > 0.5f)
            {
                
                foreach (var s in spheres)
                {
                    Destroy(s);
                }

                spheres.Clear();
                elapsed -= 0.5f;
                if (playerWentIn)
                {
                    GameObject o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    o.transform.parent = transform;
                    o.transform.localPosition = Vector3.forward;
                    spheres.Add(o);
                }

                if (forkliftWentIn)
                {
                    GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    o.transform.parent = transform;
                    o.transform.localPosition = Vector3.forward * -1;
                    spheres.Add(o);
                }
            }
        }
    }


    void Entered()
    {
        guidingController.UserEvent(new DriveForkliftToPallets(DateTime.Now, XesLifecycleTransition.complete));
        
    }
    
    

    
    
}
