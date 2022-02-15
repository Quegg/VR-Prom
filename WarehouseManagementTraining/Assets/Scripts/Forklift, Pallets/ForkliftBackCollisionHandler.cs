using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;

public class ForkliftBackCollisionHandler : MonoBehaviour
{
    public GameObject mainForkliftControllerObject;

    private ForkliftMovementController mainForkliftController;
    // Start is called before the first frame update
    void Start()
    {
        mainForkliftController = mainForkliftControllerObject.GetComponent<ForkliftMovementController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Wall"))
            mainForkliftController.SetBackStart(false);
        else if(other.gameObject.CompareTag("PalletCollision"))
            mainForkliftController.SetBackStart(true);
    }
}
