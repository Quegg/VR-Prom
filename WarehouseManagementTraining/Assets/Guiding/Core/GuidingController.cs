using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Guiding;
using Guiding.Core;
using Guiding.Core.BpmnParser;
using Guiding.LoggingEvents;
using PMLogging;
using UnityEditor;
using Valve.VR.InteractionSystem;

public class GuidingController : MonoBehaviour
{
    private BpmnProcess process;

    [Header("Activate Guiding"),
     Tooltip(
         "Deactivate if the simulation should only collect the data. Activate if all stubs are completed to use the guiding system.")]
    public bool activateGuiding;
    [Header("Player GameObject")] public GameObject playerObject;
    private Player player;
    
    [Header("Paths for generated stubs")]
    public string taskStubFolder="/Guiding/ProcessDetails/Tasks";
    public string loggingEventFolder="/Guiding/LoggingEvents";

    
    public string tasksNamesFile = "tasks";
    //public string feedbackEventFolder="/Core/FeedbackEvents";
    public string errorEventsPath="/Guiding/LoggingEvents/ErrorEvents";
    public string errorHelpPath="/Guiding/ProcessDetails/ErrorHelp";
    public string errorHelpNamesFile = "errorHelps";
    public string gatewayStubFolder="/Guiding/ProcessDetails/Gateways";
    public string gatewayNamesFile = "gateways";
    public string processFolder="/StreamingAssets";
    
    
    
    [Header("Name of the new error event")]
    public string errorEventName;

    private List<string> taskClassNames; 
    private List<string> gatewayClassNames;
    private List<string> errorClassNames;
    private Dictionary<string, System.Type> classTypesDict;

    private HUDController hudController;
    private StateMachieneController stateMachine;
    private IShowHelp currentHelp;
    private VoiceRecognizer voiceRecognizer;

    public VoiceRecognizer VoiceRecognizer => voiceRecognizer;

    private HelpHintController helpHintController;
    private bool requestedFeedback;
    private string feedbackSection;

    private XesLogger logger;

    private string nameRawOfCurrentTask;
    private string nameCutOfCurrentTask;

    public string NameCutOfCurrentTask => nameCutOfCurrentTask;

    private BpmnElement currentTask;
    private IUserManagement userManagement;

    public IUserManagement UserManagement => userManagement;

    private bool useUserManagement;
    public bool UseUserManagement => useUserManagement;
#if UNITY_EDITOR

    [ContextMenu("Test Guide")]
    public void TestGuide()
    {
        hudController.ShowProcessInformation(stateMachine.LastState);
    }
    // ++++++++ Editor Functions to prepare the simulation ++++++++

    

    /// <summary>
    /// Parse .bpmn file and generate class stubs and logging events for tasks and gateways 
    /// </summary>
    public void ParseAndGenerateStubs()
    {
        string file = GetBpmnFilePath();
        DetailStubGenerator detailStubGenerator = new DetailStubGenerator();
        Debug.Log("Parsing file: "+file);
        BPMNParser parser = new BPMNParser();
        process = parser.ParseBPMN(file);
        taskClassNames= new List<string>();
        gatewayClassNames = new List<string>();

        foreach (var task in process.allTasks)
        {
            Debug.Log("Generating stubs for task: name: "+task.nameRaw+", id: "+task.id);
            string taskName = task.nameRaw.First().ToString().ToUpper() + task.nameRaw.Substring(1);
            if (!taskClassNames.Contains(taskName))
            {
                taskClassNames.Add(taskName);
            }
            detailStubGenerator.GenerateTask(Application.dataPath+taskStubFolder, taskName);
            detailStubGenerator.GenerateLoggingEvent(Application.dataPath + loggingEventFolder, task.nameCut,false);
        }
        foreach (var gateway in process.allGateways)
        {
            Debug.Log("Generating stub for gateway: name: "+gateway.nameRaw+", id: "+gateway.id);
            //TODO ignore gatewayCondition if no gateway follows
            string gatewayName = gateway.nameRaw.First().ToString().ToUpper() + gateway.nameRaw.Substring(1);
            if (!gatewayClassNames.Contains(gatewayName))
            {
                gatewayClassNames.Add(gatewayName);
            }
            detailStubGenerator.GenerateExclusiveGateway(Application.dataPath+gatewayStubFolder, gatewayName);
        }
        AssetDatabase.Refresh();
        LoadCustomClassTypes();
    }


    /// <summary>
    /// create class stubs and logging events for the provided error event
    /// </summary>
    public void CreateErrorEvent()
    {
        if (!errorClassNames.Contains(errorEventName))
        {
            errorClassNames.Add(errorEventName);
        }
        Debug.Log("Generating stub for error event: "+errorEventName);
        DetailStubGenerator detailStubGenerator = new DetailStubGenerator();
        detailStubGenerator.GenerateLoggingEvent(Application.dataPath+errorEventsPath, errorEventName,true);
        detailStubGenerator.GenerateErrorHelp(Application.dataPath+errorHelpPath, errorEventName);
        AssetDatabase.Refresh();
        LoadCustomClassTypes();
    }

    
    [ContextMenu("Clear Complete Cache")]
    public void ClearCache()
    {
        taskClassNames.Clear();
        gatewayClassNames.Clear();
        errorClassNames.Clear();
    }
    
    [ContextMenu("Clear Error Help Class Cache")]
    public void ClearErrorHelpClasses()
    {
        errorClassNames.Clear();
    }

    /// <summary>
    /// Assingns all class stubs to the corresponding gameobject to have the loaded and active in the scene
    /// </summary>
    public void AssignStubs()
    {
        //LoadCustomClassTypes();
        //-------------------------------------------------
        //create Gameobjects and parse all classes to be added. After parsing, later, we load the types and assign the scripts
        Transform tasksTransform = transform.Find("Tasks");
        GameObject tasksObject;
        if (tasksTransform  is null)
        {
            tasksObject = new GameObject("Tasks");
            tasksObject.transform.SetParent(transform);
        }
        else
        {
            tasksObject = tasksTransform.gameObject;
        }

        List<string> taskNames = GetClassesInDirectory(Application.dataPath + taskStubFolder);

        using (StreamWriter outfileTasks =
            new StreamWriter(Application.dataPath + "/Resources/" + tasksNamesFile + ".txt"))
        {

            foreach (var taskClassName in taskNames)
            {
                outfileTasks.WriteLine(taskClassName);
            }
        }

        


        Transform gatewayTransform = transform.Find("Gateways");
        GameObject gatewaysObject;
        if (gatewayTransform  is null)
        {
            gatewaysObject=new GameObject("Gateways");
            gatewaysObject.transform.SetParent(transform);
        }
        else
        {
            gatewaysObject = gatewayTransform.gameObject;
        }

        List<string> gatewayNames = GetClassesInDirectory(Application.dataPath + gatewayStubFolder);

        using (StreamWriter outfileGateway =
            new StreamWriter(Application.dataPath + "/Resources/" + gatewayNamesFile + ".txt"))
        {

            foreach (var gatewayClassName in gatewayNames)
            {
                outfileGateway.WriteLine(gatewayClassName);
            }
        }


        Transform errorHelpTransform = transform.Find("ErrorHelp");
        GameObject errorHelpObject;
        if (errorHelpTransform  is null)
        {
            errorHelpObject=new GameObject("ErrorHelp");
            errorHelpObject.transform.SetParent(transform);
        }
        else
        {
            errorHelpObject = errorHelpTransform.gameObject;
        }

        List<string> errorNames = GetClassesInDirectory(Application.dataPath + errorHelpPath);
        
        using (StreamWriter outfileError =
            new StreamWriter(Application.dataPath + "/Resources/" + errorHelpNamesFile + ".txt"))
        {
            foreach (var errorClassName in errorNames)
            {
                outfileError.WriteLine(errorClassName);
            }
        }
        
        //Load types to be able to assign them later
        LoadCustomClassTypes();
        
        
        
        foreach (var taskClassName in taskNames)
        {
            Type taskClassType = GetTypeFromName("Guiding.ProcessDetails.Tasks." + taskClassName);
            if (tasksObject.GetComponent(taskClassType)  is null)
            {
                tasksObject.AddComponent(taskClassType);
                Debug.Log("Added " + taskClassName + " to Gameobject");
            }
            else
            {
                Debug.Log("Class already on Gameobject: " + taskClassName);
            }
        }
        
        foreach (var gatewayClassName in gatewayNames)
        {
            Type gatewayClassType = GetTypeFromName("Guiding.ProcessDetails.Gateways." + gatewayClassName);
            if (gatewaysObject.GetComponent(gatewayClassType)  is null)
            {
                gatewaysObject.AddComponent(gatewayClassType);
                Debug.Log("Added " + gatewayClassName + " to GameObject");
            }
            else
            {
                Debug.Log("Class already on Gameobject: " + gatewayClassName);
            }
        }


        foreach (var errorClassName in errorNames)
        {
            try
            {
                Type errorClassType = GetTypeFromName("Guiding.ProcessDetails.ErrorHelp." + errorClassName);
                if (errorHelpObject.GetComponent(errorClassType)  is null)
                {
                    errorHelpObject.AddComponent(errorClassType);
                    Debug.Log("Added " + errorClassName + " to Gameobject");
                }
                else
                {
                    Debug.Log("Class already on Gameobject: " + errorClassName);
                }
            }
            catch (ArgumentException e)
            {
                Debug.LogError("ErrorHelp Class file not in dictionary: " + errorClassName);
            }
        }

    }
    
    /// <summary>
    /// Gets all classnames of all .cs files in the given directory
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <returns></returns>
    private List<string> GetClassesInDirectory(string directoryPath)
    {
        List<string> classNames = new List<string>();
        foreach (var path in Directory.GetFiles(directoryPath, "*.cs", SearchOption.TopDirectoryOnly))
        {
            //the returned file system has the following structure: folder/folder/folder\filename.cs. So we need to split at '/' as well as at '\'
            string[] pathSplit = path.Split('/');
            string fileSystemIsStrange = pathSplit[pathSplit.Length - 1].Split('\\')[1];
            classNames.Add(fileSystemIsStrange.Substring(0, fileSystemIsStrange.Length - 3));
        }

        return classNames;
    }
    
#endif

    public void ActivateUserManagement(IUserManagement userManagement)
    {
        useUserManagement = true;
        this.userManagement = userManagement;

    }
    private void Start()
    {
        LoadCustomClassTypes();
        hudController = playerObject.GetComponentInChildren<HUDController>();
        player = playerObject.GetComponent<Player>();
        //hudController.gameObject.SetActive(false);
        if (activateGuiding)
        {
            stateMachine = new StateMachieneController();
            stateMachine.Initialize(GetBpmnFilePath(),false);
            CalculateNextTask();
            
            logger=XesLogger.GetLogger;
            logger.InitializeForGuiding(this);
            InitializeCustomScripts();

            helpHintController = playerObject.GetComponentInChildren<HelpHintController>();
            helpHintController.GuidingController = this;
        
            voiceRecognizer = new VoiceRecognizer(this);
            
            GetTaskDetailScriptByTaskName(nameRawOfCurrentTask).TaskStarted();
            
            
        }
        
        
    }
    
    /// <summary>
    /// Gets filepath of the bpmn file
    /// </summary>
    /// <returns>filepath of bpmn file in bpmn file directory</returns>
    /// <exception cref="Exception"></exception>
    private string GetBpmnFilePath()
    {
        string[] bpmnFiles =Directory.GetFiles(Application.dataPath + processFolder, "*.bpmn", SearchOption.TopDirectoryOnly);
        if (bpmnFiles.Length > 1)
        {
            throw new Exception("found "+bpmnFiles.Length+" files, may only be one");
        }

        return bpmnFiles[0];
        
    }
    
    /// <summary>
    /// Finds the System.Type of the given classname
    /// </summary>
    /// <param name="typeName">name of the class you want to find the type of</param>
    /// <returns>System.Type of given classname</returns>
    private static System.Type FindType(string typeName)
    {
        if (string.IsNullOrEmpty(typeName)) return null;
            
        StringComparison e = StringComparison.Ordinal;
        foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var t in assembly.GetTypes())
            {
                if (string.Equals(t.FullName, typeName, e)) return t;
            }
        }
        return null;
    }

    /// <summary>
    /// Loads all System.Types of all class names saved in resources to a dictionary, so it can be accessed fast at runtime
    /// </summary>
    [ContextMenu("Load Types")]
    private void LoadCustomClassTypes()
    {
        classTypesDict= new Dictionary<string, Type>();
        
        TextAsset taskNamesAsset = (TextAsset) Resources.Load(tasksNamesFile);
        string[] taskNames = taskNamesAsset.text.Split(new []{ '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in taskNames)
        {
            
            classTypesDict.Add("Guiding.ProcessDetails.Tasks."+line,FindType("Guiding.ProcessDetails.Tasks."+line));
        }
                
            
        TextAsset gatewayNamesAsset = (TextAsset) Resources.Load(gatewayNamesFile);
        string[] gatewayNames = gatewayNamesAsset.text.Split(new []{ '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in gatewayNames)
        {
            classTypesDict.Add("Guiding.ProcessDetails.Gateways."+line,FindType("Guiding.ProcessDetails.Gateways."+line));
            
            
        }
        
        TextAsset errorHelpAsset = (TextAsset) Resources.Load(errorHelpNamesFile);
        string[] errorHelps = errorHelpAsset.text.Split(new []{ '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in errorHelps)
        {
            classTypesDict.Add("Guiding.ProcessDetails.ErrorHelp."+line,FindType("Guiding.ProcessDetails.ErrorHelp."+line));
            
            
        }
    }

    /// <summary>
    /// returns the System.Type of the given className
    /// </summary>
    /// <param name="className">name of the class you want the type of</param>
    /// <returns>System.Type of the given class name</returns>
    private Type GetTypeFromName(string className)
    {
        if (classTypesDict  is null)
        {
            LoadCustomClassTypes();
        }
        Debug.Log(className);
        try
        {
            return classTypesDict[className];
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogError("not found type of class: "+className+" in dictionary");
            throw new KeyNotFoundException(className);
        }
        
    }

    // ++++++++ Runtime functions ++++++++
    
    /// <summary>
    /// logger registered a event done by the user (everything except feedback events)
    /// </summary>
    /// <param name="eventName">name of the event</param>
    /// <param name="isError">is this an error even?</param>
    public void  UserEvent(XesEvent eventToLog)
    {
        if (activateGuiding)
        {
            //Check if the event is an error event
            bool isError = false;
            FieldInfo isErrorField = eventToLog.GetType().GetField("isError");
            if (!(isErrorField is null))
            {
                isError = (bool) isErrorField.GetValue(eventToLog);
            }

            string eventName;
            FieldInfo eventNameField = eventToLog.GetType().GetField("eventName");
            if (eventNameField is null)
            {
                throw new Exception("Event needs a name");
            }
            else
            {
                eventName = ((XesConceptName) eventNameField.GetValue(eventToLog)).value.ToString();
            }

            bool executedAtRightTime;

            Debug.Log("Event occured: " + eventName);
            if (isError)
            {
                UserEventError(eventName);
                //errors should never occur
                executedAtRightTime = false;
            }
            else
            {
                executedAtRightTime = UserEventTask(eventName);
            }


            logger.WriteEvent(eventToLog, executedAtRightTime);
            if (useUserManagement)
            {
                userManagement.EventOccured(eventToLog,executedAtRightTime);
            }
        }
        else
        {
            logger.WriteEvent(eventToLog);
        }

        
    }
    [ContextMenu("eventTest")]
    public void EventTest()
    {
        UserEventTask("PickUpItem");
    }
    
    

    /// <summary>
    /// task event occured. check if it was the correct action and offer help, if not
    /// </summary>
    /// <param name="eventName">name of the event occured</param>
    /// <exception cref="Exception"></exception>
    private bool UserEventTask(string eventName)
    {
        //check, if the current task is done
        if (!nameRawOfCurrentTask.Equals("EndEvent"))
        {
            ITaskDetails taskScript = GetTaskDetailScriptByTaskName(nameRawOfCurrentTask);
            if (nameCutOfCurrentTask.Equals(eventName))
            {
                if (taskScript.IsDone())
                {
                    taskScript.TaskEnded();
                    hudController.TurnOff();
                    hudController.HideHints();
                    currentHelp = null;
                    stateMachine.NextState(stateMachine.GetElementByName(nameRawOfCurrentTask).id);
                    CalculateNextTask();
                    if (!nameRawOfCurrentTask.Equals("EndEvent"))
                    {
                        try
                        {
                            GetTaskDetailScriptByTaskName(nameRawOfCurrentTask).TaskStarted();
                        }
                        catch (KeyNotFoundException e)
                        {
                            Debug.LogError("Not Found: " + nameRawOfCurrentTask);
                            throw;
                        }
                    }

                    return true;
                }
            }

            if (taskScript is null)
            {
                throw new Exception("TaskScript :" + nameRawOfCurrentTask + " not on Tasks GameObject");
            }
            else
            {
                ShowHelpHint(taskScript, false);
            }
        }

        return false;
    }

    /// <summary>
    /// Calculates the name of the current task based on the gateway's conditions
    /// </summary>
    private void CalculateNextTask()
    {
        
        //task done by the user is the next task to do. therefor check next task or gateways. 
        List<BpmnSequenceFlow> nextElementsFlows = stateMachine.LastState.outgoing;
        if (nextElementsFlows.Count > 1)
        {
            Debug.LogError("A Task may only have one outgoing Sequenceflow. Task "+stateMachine.LastState.nameRaw + "has "+stateMachine.LastState.outgoing.Count);
        }
        BpmnElement nextElement = nextElementsFlows[0].target;
        nameRawOfCurrentTask="undefined";
            
        //The next element is a task
        if (nextElement is BpmnTask)
        {
            nameCutOfCurrentTask = nextElement.nameCut;
            nameRawOfCurrentTask = nextElement.nameRaw;
        }
        //next element is gateway, so determine the next task based on the given conditions
        else if (nextElement is BpmnExclusiveGateway)
        {
            string[] names = GetNameOfTheNextTaskWithCurrentConditions((BpmnExclusiveGateway) nextElement);
            nameRawOfCurrentTask = names[0];
            nameCutOfCurrentTask = names[1];
        }
        else if (nextElement is BpmnEndEvent)
        {
            nameRawOfCurrentTask = "EndEvent";
        }

    }
    
    

    [ContextMenu("GetLastState")]
    public void GetLastState()
    {
        Debug.Log(stateMachine.LastState.nameRaw);
    }

    /// <summary>
    /// error event occured. check if it was the correct action and offer help, if not
    /// </summary>
    /// <param name="eventName">name of the event occured</param>
    /// <exception cref="Exception"></exception>
    private void UserEventError(string eventName)
    {
        IErrorHelp errorHelpScript=null;
        foreach (var script in transform.Find("ErrorHelp").GetComponents<IErrorHelp>())
        {
            if (script.GetName().Equals(eventName))
            {
                errorHelpScript = script;
                break;
            }
        }
        if (errorHelpScript  is null)
        {
            throw new Exception("ErrorHelp :"+eventName+" not on ErrorHelp GameObject");
        }
        else
        {
            ShowHelpHint(errorHelpScript,false);
        }
    }

    /// <summary>
    /// activates the exclamation mark so that the user knows he can get help if wanted
    /// </summary>
    /// <param name="helpScript">script which offers the help</param>
    /// <param name="executeDirectly">execute without asking if available according to the script</param>
    public void ShowHelpHint(IShowHelp helpScript, bool executeDirectly)
    {
        
        if (!(helpScript is null) && (executeDirectly || helpScript.HelpAvailable()))
        {
            currentHelp = helpScript;
            helpHintController.ShowNotification();
        }
        else
        {
            //helpHintController.HideNotification();
        }
    }

    /// <summary>
    /// the user requested help, call showhelp for the last computed help
    /// </summary>
    /// <exception cref="NoCustomHelpException"></exception>
    public void UserRequestedShowHelp()
    {
        if (requestedFeedback)
        {
            hudController.ShowFeedbackRequest(feedbackSection);
            requestedFeedback = false;
        }
        else
        {
            currentHelp?.ShowHelp();
            ShowGenericHelp();
        }
        
    }

    

    

    ///<summary>
    ///starts at the current task and checks which task is next by evaluating the gateways conditions.
    ///the gateway returns, which element comes next. if its a task, return, else check the next gateway
    ///</summary>
    
    private string[] GetNameOfTheNextTaskWithCurrentConditions(BpmnExclusiveGateway gateway)
    {
        while (true)
        {
            List<string> nextElementsAsString = new List<string>();
            foreach (var sqf in gateway.outgoing)
            {
                nextElementsAsString.Add(sqf.target.nameRaw);
            }

            //Gets the script of the gateway to retrieve the name of the next element based on the conditions in the simulation. Then gets the bpmnElement to this name
            BpmnElement nextElement = stateMachine.GetElementByName(GetGatewayDetailScriptByGatewayName(gateway.nameRaw)
                .CheckConditions(nextElementsAsString));
            if (nextElement is BpmnTask)
            {
                return new []{nextElement.nameRaw,nextElement.nameCut };
            }

            if (nextElement is BpmnEndEvent)
            {
                return new[] {"EndEvent", "EndEvent"};
            }

            if (nextElement is BpmnExclusiveGateway)
            {
                gateway = (BpmnExclusiveGateway) nextElement;    
                continue;
            }

            return new []{"",""};
            break;
        }
    }

    /// <summary>
    /// shows generic help for the current state
    /// </summary>
    [ContextMenu("showProcess")]
    public void ShowGenericHelp()
    {
        hudController.ShowProcessInformation(stateMachine.LastState);
    }

    /// <summary>
    /// Can be called from custom class to show a text help
    /// </summary>
    /// <param name="text">text to show</param>
    /// <param name="eventName">name of the event which is guided</param>
    /*
     public void ShowTextHelp(string text, string eventName)
    {
        //only show help, if the class says it
        
        hudController.ShowTextHelp(text,eventName);
    }
    */

    
    /// <summary>
    /// ask the user for feedback. Called by taskscript when a process section was finished
    /// </summary>
    /// <param name="section">name of the section you want the feedback for</param>
    public void AskForFeedback(string section)
    {
        feedbackSection = section;
        requestedFeedback = true;
        helpHintController.ShowNotification();
        
        //TODO check for voice commands here
        
    }

    /// <summary>
    /// Received feedback by the user via voice command, save the feedback
    /// </summary>
    /// <param name="feedbackPositive">was the feedback positive?</param>
    public void FeedbackResult(bool feedbackPositive)
    {
        XesEvent feedback;
        Debug.Log("Received Feedback. isPositiv: "+feedbackPositive);
        if (feedbackPositive)
        {
            feedback=new PositiveFeedback(feedbackSection,DateTime.Now, XesLifecycleTransition.complete);
        }
        else
        {
            feedback=new NegativeFeedback(feedbackSection,DateTime.Now, XesLifecycleTransition.complete);
        }
        //feedback events are always executed at the right time, so they do not count as mistakes
        logger.WriteEvent(feedback,true);
        UserManagement.EventOccured(feedback,true);
        helpHintController.ShowFeedbackSaved();
        voiceRecognizer.EndListening();
        hudController.CloseFeedbackRequest();
    }

    [ContextMenu("AskFor Feedback")]
    public void AskForFeedbackTest()
    {
        AskForFeedback("test");
    }

    
   

    

    /// <summary>
    /// Get the requested task detail script attached to the task gameobject
    /// </summary>
    /// <param name="taskClassName">name of the class you want the script of</param>
    /// <returns></returns>
    private ITaskDetails GetTaskDetailScriptByTaskName(string taskClassName)
    {
        Debug.Log("Searching script for: "+taskClassName);
        Transform tasksTransform = transform.Find("Tasks");
        GameObject tasksObject = tasksTransform.gameObject;
        
        Type taskClassType = GetTypeFromName("Guiding.ProcessDetails.Tasks." + taskClassName);
        return (ITaskDetails) tasksObject.GetComponent(taskClassType);
    }
    
    /// <summary>
    /// get the requested gateway detail script attached to the gateways gameobject
    /// </summary>
    /// <param name="gatewayClassName">name of the class you want the script of</param>
    /// <returns></returns>
    private IGatewayDetails GetGatewayDetailScriptByGatewayName(string gatewayClassName)
    {
        Debug.Log("Searching script for: "+gatewayClassName);
        Transform gatewayTransform = transform.Find("Gateways");
        GameObject gatewaysObject = gatewayTransform.gameObject;
        
        Type gatewayClassType = GetTypeFromName("Guiding.ProcessDetails.Gateways." + gatewayClassName);
        return (IGatewayDetails) gatewaysObject.GetComponent(gatewayClassType);
    }
    /// <summary>
    /// get the requested error help script attached to the error gameobject
    /// </summary>
    /// <param name="errorHelpClassName">name of the class you want the script of</param>
    /// <returns></returns>
    private IErrorHelp GetErrorHelpScriptByName(string errorHelpClassName)
    {
        Transform errorHelpTransform = transform.Find("ErrorHelp");
        GameObject errorHelpObject = errorHelpTransform.gameObject;
        
        Type errorHelpClassType = GetTypeFromName("Guiding.ProcessDetails.ErrorHelp." + errorHelpClassName);
        return (IErrorHelp) errorHelpObject.GetComponent(errorHelpClassType);
    }

    
    /// <summary>
    /// Initializes all custom scripts(tasks, gatways, errors) at the start
    /// </summary>
    private void InitializeCustomScripts()
    {
        GameObject tasks = transform.Find("Tasks").gameObject;
        foreach (var task in tasks.GetComponents<ITaskDetails>())
        {
            task.Initialize(this);
        }
        
        GameObject errors = transform.Find("ErrorHelp").gameObject;
        foreach (var error in errors.GetComponents<IErrorHelp>())
        {
            error.Initialize(this);
        }
        
    }

    private void OnDestroy()
    {
        voiceRecognizer.Deactivate();
    }
}