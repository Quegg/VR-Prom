using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOrdersToPick : MonoBehaviour
{
    public GameObject OrderSelectingCanvas;
    public GameObject Laserpointer;
    public bool stayActivated;

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Laserpointer.SetActive(true);
            //TODO fadein
            OrderSelectingCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //TODO fadeout
        if (!stayActivated && other.gameObject.CompareTag("Player"))
        {
            Laserpointer.SetActive(false);
            OrderSelectingCanvas.SetActive(false);
        }
    }
}
