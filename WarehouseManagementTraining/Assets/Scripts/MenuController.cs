using System;
using System.Collections;
using System.Collections.Generic;
using PMLogging;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class MenuController : MonoBehaviour
{
    private int walkthroughNumber;
    private XesLogger logger;
    private IUserManagement userManagement;
    private bool useUserManagement;
    
    
    

    public static MenuController instance;
    // Start is called before the first frame update
    void Awake()
    {
        
        logger = XesLogger.GetLogger;
        DontDestroyOnLoad(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GoToMenu();
            
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadWarehouse();
        }

        if (Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.X))
        {
            CloseApplication();
        }
    }

    public void GoToMenu()
    {
        if (useUserManagement)
        {
            userManagement.EndTrace();
        }
        SceneManager.LoadScene("Menu");
        logger.CloseWriter();
    }

    public void CloseApplication()
    {
        if (useUserManagement)
        {
            
            //userManagement.EndTrace();
        }
        logger.CloseWriter();
        logger.CombineTracesToLog();
        Application.Quit();
    }
    
    
    
    /// <summary>
    /// Initializes the Menu Controller if not done and returns the instance
    /// </summary>
    /// <returns></returns>
    public static MenuController InitializeMenuController()
    { 
        if (instance is null)
        {
            // Search for existing instance.
            instance = (MenuController)FindObjectOfType(typeof(MenuController));
 
            // Create new instance if one doesn't already exist.
            if (instance is null)
            {
                // Need to create a new GameObject to attach the singleton to.
                var singletonObject = new GameObject();
                instance = singletonObject.AddComponent<MenuController>();
                singletonObject.name = typeof(MenuController).ToString() + " (Singleton)";
 
                // Make instance persistent.
                DontDestroyOnLoad(singletonObject);
            }
        }
        return instance;
    }

    public void ActivateUserManagement(IUserManagement userManager)
    {
        useUserManagement = true;
        this.userManagement = userManager;
    }

    public void LoadWarehouse()
    {
        StartCoroutine(LoadYourAsyncScene());
    }
    
    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Warehouse");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (useUserManagement)
        {
            userManagement.StartTrace();
            FindObjectOfType<GuidingController>().ActivateUserManagement(userManagement);
        }
        DateTime dateTime = DateTime.Now;
        string traceName = dateTime.Day + "." + dateTime.Month + "." + dateTime.Year + "_" + dateTime.Hour + "." +
                           dateTime.Minute + "." + dateTime.Second;
        logger.StartNewTrace(new XesString("Walkthrough "+traceName));
        
    }
}
