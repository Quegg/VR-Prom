using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDMovementController : MonoBehaviour
{
    public GameObject hudObject;
    public GameObject helpHintObject;

    private BoxCollider hudCollider;
    public BoxCollider hintCollider;
    //private SphereCollider hudCollider;
    void Start()
    {
        hudCollider = GetComponent<BoxCollider>();
    }

  

    private void Update()
    {
        
        if (!hudCollider.bounds.Contains(hudObject.transform.position))
        {
            hudObject.transform.position = hudCollider.bounds.ClosestPoint(hudObject.transform.position);
            hudObject.transform.LookAt(transform.parent);
            var rotation = hudObject.transform.rotation;
            rotation=Quaternion.Euler(new Vector3(0,rotation.eulerAngles.y,rotation.eulerAngles.z));
            hudObject.transform.rotation = rotation;
        }
        
        
        
        if (!hintCollider.bounds.Contains(helpHintObject.transform.position))
        {
            helpHintObject.transform.position = hintCollider.bounds.ClosestPoint(helpHintObject.transform.position);
            helpHintObject.transform.LookAt(transform.parent);
            
        }
        transform.LookAt(transform.parent);
        var thisRotation = transform.rotation;
        thisRotation=Quaternion.Euler(new Vector3(0,thisRotation.eulerAngles.y,thisRotation.eulerAngles.z));
        transform.rotation = thisRotation;
    }
}