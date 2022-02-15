using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MenuControllerInitializer : MonoBehaviour
{
    public SteamVR_Action_Boolean loadWarehouse;

    public SteamVR_Input_Sources inputSource;
    private MenuController menuController;
    private Coroutine delayForButtonHints;
    private bool hintsShown;
    private Player player;
    float startTime;

    public GameObject userInputUi;
    
    private UserManagement userManager;
    private GameObject laserPointer;
    void Awake()
    {
        menuController=MenuController.InitializeMenuController();
    }

    private void Start()
    {
        
        loadWarehouse.AddOnStateDownListener(LoadWarehouse,inputSource);

        //create the user management, if it does not exist, get it otherwise
        userManager = FindObjectOfType<UserManagement>();
        if (userManager is null)
        {
            menuController.gameObject.AddComponent<UserManagement>();
            userManager = menuController.GetComponent<UserManagement>();
            menuController.ActivateUserManagement(userManager);
        }

        laserPointer = FindObjectOfType<MyLaserPointer>().gameObject;
        laserPointer.SetActive(false);
        
        //delayForButtonHints = StartCoroutine(DelayForControllerHints());
        player = GetComponent<Player>();
        startTime = Time.time;
    }

    private void Update()
    {
        //wait some time before activating the controllerbuttonhints. If it would be activated instantly, it can be that they are not shown, 
        //as the controllers are not loaded yet, when the scene is activated.
        if (Time.time - startTime > 5f)
        {
            if(!userInputUi.activeSelf)
            {
                startTime = Time.time;
               
                ControllerButtonHints.ShowTextHint(player.leftHand,loadWarehouse, "Press to start the simulation");
            }
            
        }
    }

    private void OnDestroy()
    {
        loadWarehouse.RemoveOnStateDownListener(LoadWarehouse,inputSource); 
    }


    private void LoadWarehouse(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        ControllerButtonHints.HideTextHint(player.leftHand,loadWarehouse);
        if (userInputUi.activeSelf)
        {
            userInputUi.SetActive(false);
            laserPointer.SetActive(false);
        }
        else
        {
            userInputUi.SetActive(true);
            laserPointer.SetActive(true );
        }

    }

    public void Load(string userId)
    {
        if (!SceneManager.GetActiveScene().name.Equals("Warehouse"))
        {
            SteamVR_Fade.Start(Color.black, 1f);
           
            StartCoroutine(DelayToFade());
            userManager.UserId = userId;
        }
    }

    IEnumerator DelayToFade()
    {
        yield return new WaitForSeconds(1f);
        menuController.LoadWarehouse();
    }
    
}