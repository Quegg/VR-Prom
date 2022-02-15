using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletOnForkliftCollsionHandlerWithPalletOnGround : MonoBehaviour
{
    public PalletController palletController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("PalletCollision"))
        {
            if(palletController.mainForkliftController!=null)
                palletController.mainForkliftController.SetBackStart(false);
        }
    }
}
