using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player or Forklift enter one trigger in front of the pallet places. Calls PalletEnterTriggerController, to ensure, the player ENTERS the Area WITH the forklift
public class PalletsEnterTriggerSingle : MonoBehaviour
{
    private PalletsEnterTriggerController controller;
    public TriggerPosition position;

    private void Start()
    {
        controller = GetComponentInParent<PalletsEnterTriggerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (position == TriggerPosition.Inner)
            {
                controller.OnPlayerEnterInner();
            }
            else
            {
                controller.OnPlayerEnterOuter();
            }
        }
        else if(other.CompareTag("Forklift"))
        {
            if (position == TriggerPosition.Inner)
            {
                controller.OnForkliftEnterInner();
            }
            else
            {
                controller.OnForkliftEnterOuter();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (position == TriggerPosition.Inner)
            {
                controller.OnPlayerLeftInner();
            }
            else
            {
                controller.OnPlayerLeftOuter();
            }
        }
        else if(other.CompareTag("Forklift"))
        {
            if (position == TriggerPosition.Inner)
            {
                controller.OnForkliftLeftInner();
            }
            else
            {
                controller.OnForkliftLeftOuter();
            }
        }
    }

    public enum TriggerPosition{ Inner, Outer};
}
