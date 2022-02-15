using System;
using System.Collections;
using System.Collections.Generic;
using Guiding.LoggingEvents;
using PMLogging;
using TMPro;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BarcodeScannerController : MonoBehaviour
{
    public SteamVR_Action_Boolean handMode;
    public SteamVR_Action_Boolean scannerTriggerd;

    public SteamVR_Input_Sources handType;

    public GameObject barcodeScannerPrefab;

    public float scanningRange;

    public AudioSource scannerBeep;

    private GameObject barcodeScanner;
    private GameObject scannerLightObject;
    
    private bool buttonDown = false;
    private bool ScannerActive = false;

    private bool scannerTriggerDown=false;
    private bool scanning=false;
    private bool scannedObject;

    private Transform lightTransform;
    
    private Hand hand;

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI typeText;
    private GameObject visuals;

    private GuidingController guidingController;


    // Start is called before the first frame update
    void Start()
    {
        hand = GetComponent<Hand>();
        barcodeScanner=Instantiate(barcodeScannerPrefab, transform);
        scannerLightObject = barcodeScanner.GetComponentInChildren<Light>().gameObject;
        lightTransform = barcodeScanner.GetComponent<BarcodeScannerRaycastDirection>().lightStart.transform;
        nameText = barcodeScanner.GetComponent<BarcodeScannerRaycastDirection>().name;
        typeText = barcodeScanner.GetComponent<BarcodeScannerRaycastDirection>().type;
        visuals = barcodeScanner.GetComponent<BarcodeScannerRaycastDirection>().visuals;
        lightTransform.gameObject.SetActive(false);
        barcodeScanner.SetActive(false);
        handMode.AddOnStateDownListener(OnButtonDown,handType);
        handMode.AddOnStateUpListener(OnButtonUp,handType);
        scannerTriggerd.AddOnStateDownListener(OnTriggerDown,handType);
        scannerTriggerd.AddOnStateUpListener(OnTriggerUp,handType);
        
        guidingController=FindObjectOfType<GuidingController>();
        
        
    }

    private void OnDestroy()
    {
        handMode.RemoveOnStateDownListener(OnButtonDown,handType);
        handMode.RemoveOnStateUpListener(OnButtonUp,handType);
        scannerTriggerd.RemoveOnStateDownListener(OnTriggerDown,handType);
        scannerTriggerd.RemoveOnStateUpListener(OnTriggerUp,handType);
    }

    /// <summary>
    /// Perform the scanning. Do a raycast from the middle of the barcode scanner and see, if it hits a barcode
    /// </summary>
    void FixedUpdate()
    {
        if (scanning&&!scannedObject)
        {
            Debug.DrawLine(lightTransform.position,transform.position+lightTransform.forward*scanningRange,Color.green,0.1f);  
            RaycastHit hit;
            if (Physics.Raycast(lightTransform.position, lightTransform.forward, out hit, scanningRange))
            {
                if (hit.collider.CompareTag("Barcode"))
                {
                    scannerBeep.Play();
                    scannedObject = true;
                    Barcode bc =hit.collider.gameObject.GetComponent<Barcode>();
                    bc.OnBarcodeScanned();
                    if (bc.type == Barcode.BarcodeType.SmallItem||bc.type==Barcode.BarcodeType.BigItem)
                    {
                        visuals.SetActive(true);
                        nameText.text = bc.GetName();
                        typeText.text = bc.GetTypeOrPalletPlace();
                    }
                }
            }
        }
    }

    
    /// <summary>
    /// The user pressed the button for activating the barcode scanner
    /// </summary>
    /// <param name="fromAction"></param>
    /// <param name="fromSource"></param>
    public void OnButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!buttonDown)
        {
            buttonDown = true;
            if (!ScannerActive)
            {
                guidingController.UserEvent(new SetHandmodeToBarcodeScanner(DateTime.Now, XesLifecycleTransition.complete));
                barcodeScanner.SetActive(true);
                hand.Hide();
                //barcodeScanner.transform.position = hand.mainRenderModel.transform.position;
                barcodeScanner.transform.rotation = hand.mainRenderModel.transform.rotation;
                ScannerActive = true;
            }
            else
            {
                barcodeScanner.SetActive(false);
                
                hand.Show();
                ScannerActive = false;
                guidingController.UserEvent(new SetHandmodeToHand(DateTime.Now, XesLifecycleTransition.complete));
            }
        }
    }
    
    public void OnButtonUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (buttonDown)
        {
            buttonDown = false;
        }
    }

    /// <summary>
    /// The user pressed the trigger
    /// </summary>
    /// <param name="fromAction"></param>
    /// <param name="fromSource"></param>
    public void OnTriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (barcodeScanner.activeSelf&&!scannerTriggerDown)
        {
            scannerTriggerDown = true;
            scanning = true;
            scannerLightObject.SetActive(true);
        }
    }

    /// <summary>
    /// The trigger is released
    /// </summary>
    /// <param name="fromAction"></param>
    /// <param name="fromSource"></param>
    public void OnTriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (scannerTriggerDown)
        {
            if(!scannedObject)
                guidingController.UserEvent(new ScannedWithoutBarcode(DateTime.Now, XesLifecycleTransition.complete));
            //Debug.Log("Trigger UP!!!!");
            scannerTriggerDown = false;
            scanning = false;
            scannerLightObject.SetActive(false);
            scannedObject = false;
            visuals.SetActive(false);
        }
    }

    public void ShowButtonHintsForBarcodeScanner()
    {
        ControllerButtonHints.ShowTextHint(hand,handMode,"Switch to barcode scanner");
    }

    public void HideButtonHints()
    {
        ControllerButtonHints.HideTextHint(hand,handMode);
    }
    
    public void ShowButtonHintsForHand()
    {
        ControllerButtonHints.ShowTextHint(hand,handMode,"Switch to Hand");
    }

    
}
