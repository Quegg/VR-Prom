using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls, if the forks are touching the pallet
/// </summary>
public class ForkliftPalletCollisionDetection : MonoBehaviour
{
    
    
    

    private ForkLiftFrontController frontController;
    // Start is called before the first frame update
    void Start()
    {
        
        frontController = GetComponentInParent<ForkLiftFrontController>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PalletTopCollision"))
        {
            frontController.touchingPalletTop = true;
            
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("PalletTopCollision"))
        {
            frontController.touchingPalletTop = false;
            
            
        }
    }
}
