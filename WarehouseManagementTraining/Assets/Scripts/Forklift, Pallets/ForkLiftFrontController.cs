using System;
using System.Collections;
using System.Collections.Generic;
using Guiding.LoggingEvents;
using PMLogging;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

/// <summary>
/// Control Fork movement
/// </summary>
public class ForkLiftFrontController : MonoBehaviour
{
    public Transform endPointObject;

    private Transform pallet;
   
    private Vector3 startPoint;
    private Vector3 endPoint;
    public ForkliftMovementController mainForkliftController;
    
    
    public AudioSource liftAudio;

    [HideInInspector] public bool touchingPalletTop=false;
    private GuidingController guidingController;

    // Time to move from sunrise to sunset position, in seconds.
    public float liftingTime = 1.0f;

    // The time at which the animation started.
    private float startTime;

    //-1=down, 1=up, 0, nothing
    private int lifting=0;


    public void SetPallet(Transform newPallet)
    {
        this.pallet = newPallet;
    }


    private LiftState liftstate=LiftState.Down;

    void Start()
    {
        // Note the time at the start of the animation.
        startTime = Time.time;
        guidingController=FindObjectOfType<GuidingController>();
        startPoint = transform.localPosition;
        endPoint= new Vector3(startPoint.x,endPointObject.localPosition.y,startPoint.z);
        
    }
    public enum LiftState
    {
        Up,Down
    }

    void Update()
    {
        //lifting up
        if (lifting == 1)
        {
            if (!(pallet is null) && touchingPalletTop)
            {
                pallet.parent = transform;
                
                
            }
            float fracComplete = (Time.time - startTime) / liftingTime;
            transform.localPosition = Vector3.Slerp(startPoint, endPoint, fracComplete);
            //reached up position
            if (fracComplete > 1)
            {
                if (!(pallet is null))
                {
                    pallet.GetComponent<Rigidbody>().isKinematic = true;
                    guidingController.UserEvent(new LiftUpBigItem(mainForkliftController.forklift.position,DateTime.Now, XesLifecycleTransition.complete));
                    
                }
                lifting = 0;
                liftstate = LiftState.Up;
                
                transform.localPosition = endPoint;
            }
        }
        //lifting down
        if (lifting == -1)
        {
            float fracComplete = (Time.time - startTime) / liftingTime;
            transform.localPosition = Vector3.Slerp(endPoint, startPoint, fracComplete);
            
            //pallet is on ground
            if (fracComplete > 1)
            {
                lifting = 0;
                liftstate=LiftState.Down;
            }
        }
    }

    /// <summary>
    /// Trigger fork movements
    /// </summary>
    /// <param name="state"></param>
    public void SetLiftState(LiftState state)
    {
        //can only trigger the movement, if the fork is not moving
        if (lifting==0)
        {
            //cannot trigger movenment, if the fork is already in the desired position
            if (liftstate != state)
            {
                //determine the direction and lift
                if (state == LiftState.Up)
                {
                    lifting = 1;
                    startTime = Time.time;
                    liftAudio.Play();
                    if(pallet!=null)
                        pallet.gameObject.GetComponentInChildren<PalletController>().mainForkliftController = mainForkliftController;
                }
                else
                {
                    
                    liftAudio.Play();
                    lifting = -1;
                    startTime = Time.time;
                   
                    if (!(pallet is null))
                    {
                        pallet.parent = null;
                        pallet.GetComponent<Rigidbody>().isKinematic = false;
                        
                    }
                    else  //ensure that the pallet gets removed from forklift in every case, there were cases in which the pallet was stuck on the forklift
                    {
                        PalletController p = GetComponentInChildren<PalletController>();
                        if (!(p is null))
                        {
                            p.transform.parent.parent = null;
                        }
                    }
                }
            }
        }
        
    }

    [ContextMenu("LiftUp")]
    public void LiftUp()
    {
        SetLiftState(LiftState.Up);
    }
    
    
    
    [ContextMenu("LiftDown")]
    public void LiftDown()
    {
        SetLiftState(LiftState.Down);
    }

    [ContextMenu("ShowState")]
    public void ShowState()
    {
        Debug.Log(liftstate);
    }
    
}
