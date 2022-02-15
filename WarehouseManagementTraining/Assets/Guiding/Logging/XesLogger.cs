using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Guiding.LoggingEvents;
using UnityEngine;
using UnityEngine.SceneManagement;
using XesAttributes;

namespace PMLogging
{
    public sealed class XesLogger : MonoBehaviour
    {
        private static XesLogger instance;
        private static readonly object padlock = new object();


        public XesExtensionType[] supportedExtensions=new []{XesExtensionType.Concept,XesExtensionType.Cost,XesExtensionType.Identity,XesExtensionType.Lifecycle,XesExtensionType.Organizational,XesExtensionType.Semantic,XesExtensionType.Time}; 
    
        public string tracePrefix="trace";
        public string logPrefix = "log";

        Vector3 tmp=Vector3.zero;
        private XmlWriterSettings settings;

        private XmlWriter writer;


        public bool autoInitialize;

        private GuidingController myGuidingController;
        private bool informGuidingController = false;
        
        /// <summary>
        /// Initialize the Logger to work together with the GuidingController. Everytime an event gets logged, the guidingController is informed
        /// </summary>
        /// <param name="guidingController"></param>
        public void InitializeForGuiding(GuidingController guidingController)
        {
            this.myGuidingController = guidingController;
            informGuidingController = true;
        }
        
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            if (autoInitialize)
            {
                StartNewTrace(new XesString("0"));
            }
        }

        /// <summary>
        /// returns the current instance of the xes logger
        /// </summary>
        public static XesLogger GetLogger
        {
            get
            { lock (padlock)
                {
                    if (instance  is null)
                    {
                        // Search for existing instance.
                        instance = (XesLogger)FindObjectOfType(typeof(XesLogger));
 
                        // Create new instance if one doesn't already exist.
                        if (instance  is null)
                        {
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            instance = singletonObject.AddComponent<XesLogger>();
                            singletonObject.name = typeof(XesLogger).ToString() + " (Singleton)";
 
                            
                            DontDestroyOnLoad(singletonObject);
                        }
                    }
 
                    return instance;
                }
            }
        }
 
 
       
 
 
        private void OnDestroy()
        {
            writer?.Close();
        }
    

        /// <summary>
        /// takes all traces in the traces directory and combines it to a single log
        /// </summary>
        [ContextMenu("Combine To Log")]
        public void CombineTracesToLog()
        {
            writer?.Close();
            string tracesPath = Path.Combine(Application.persistentDataPath, tracePrefix + "s");
            string logPath = Path.Combine(Application.persistentDataPath, logPrefix + "s");

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            DateTime dateTime = DateTime.Now;
            string logFileName = logPrefix+"-"+dateTime.Day+"."+dateTime.Month+"."+dateTime.Year+"_"+dateTime.Hour+"."+dateTime.Minute+"."+dateTime.Second + ".xes";
            string logFilePath = Path.Combine(logPath, logFileName);

        
            string[] filePaths = Directory.GetFiles(tracesPath, "*.xml");
            using (StreamWriter sw = new StreamWriter(logFilePath))
            {
                sw.WriteLine("<?xml version =\"1.0\" encoding =\""+sw.Encoding.HeaderName+"\"?>");
                sw.WriteLine("<log xes.version=\"2.0\" xes.features =\"nested-attributes\">");
                foreach (var extension in supportedExtensions)
                {
                    switch (extension)
                    {
                        case XesExtensionType.Concept:
                            sw.WriteLine("<extension name =\"Concept\" prefix=\"concept\" uri =\"http://www.xes-standard.org/concept.xesext\" />");
                            break;
                        case XesExtensionType.Cost:
                            sw.WriteLine("<extension name =\"Cost\" prefix=\"cost\" uri =\"http://www.xes-standard.org/cost.xesext\" />");
                            break;
                        case XesExtensionType.Identity:
                            sw.WriteLine("<extension name =\"ID\" prefix=\"identity\" uri =\"http://www.xes-standard.org/identity.xesext\" />");
                            break;
                        case XesExtensionType.Lifecycle:
                            sw.WriteLine("<extension name =\"Lifecycle\" prefix=\"lifecycle\" uri =\"http://www.xes-standard.org/lifecycle.xesext\" />");
                            break;
                        case XesExtensionType.Organizational:
                            sw.WriteLine("<extension name =\"Organizational\" prefix=\"org\" uri =\"http://www.xes-standard.org/org.xesext\" />");
                            break;
                        case XesExtensionType.Semantic:
                            sw.WriteLine("<extension name =\"Semantic\" prefix=\"semantic\" uri =\"http://www.xes-standard.org/semantic.xesext\" />");
                            break;
                        case XesExtensionType.Time:
                            sw.WriteLine("<extension name =\"Time\" prefix=\"time\" uri =\"http://www.xes-standard.org/time.xesext\" />");
                            break;
                    }
                }
                
                foreach (var filePath in filePaths)
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string line;
                        while (!((line = sr.ReadLine()) is null))
                        {
                            sw.WriteLine("  "+line);
                        
                        }
                    }
                }
                sw.WriteLine("</log>");
            }
        }


        //[ContextMenu("Serialize Test")]
        public void SerializationTest()
        {
        
            XmlWriterSettings writerSettings = new XmlWriterSettings{Indent = true, OmitXmlDeclaration = true};
            DateTime dateTime = DateTime.Now;
            string fileName = tracePrefix+"-"+dateTime.Day+"."+dateTime.Month+"."+dateTime.Year+"_"+dateTime.Hour+"."+dateTime.Minute+"."+dateTime.Second + ".xml";
            string directoryPath = Path.Combine(Application.persistentDataPath, tracePrefix + "s");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        
            string filePath = Path.Combine(directoryPath, fileName);
            writer = XmlWriter.Create(filePath, writerSettings);
            writer.WriteStartElement("Log");
            writer.WriteAttributeString("xes.version","2.0");
            writer.WriteAttributeString("xes.features","nested-attributes");
            writer.WriteStartElement("trace");
            writer.WriteElementString("item","testing");
            writer.WriteStartElement("event");
            writer.WriteElementString("event","testing");
            writer.WriteEndElement();
            writer.Close();
        }

        
        /// <summary>
        /// Creates a new file and initializes the XmlWriter for a new trace
        /// </summary>
        /// <param name="traceName"></param>
        [ContextMenu("Start New Trace")]
        public void StartNewTrace(XesString traceName)
        {
            //closes writer, if there is a writer which is not closed
            writer?.Close();

            //initializes new XmlWriter for current log
            XmlWriterSettings writerSettings = new XmlWriterSettings{Indent = true, OmitXmlDeclaration = true};
            DateTime dateTime = DateTime.Now;
            string fileName = tracePrefix+"-"+dateTime.Day+"."+dateTime.Month+"."+dateTime.Year+"_"+dateTime.Hour+"."+dateTime.Minute+"."+dateTime.Second + ".xml";
            string directoryPath = Path.Combine(Application.persistentDataPath, tracePrefix + "s");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        
            string filePath = Path.Combine(directoryPath, fileName);
        
            writer = XmlWriter.Create(filePath,writerSettings);
        
            //begin document
            writer.WriteStartElement("trace");
            LogExtensionAttribute(new XesConceptName(traceName));
            if (myGuidingController.UseUserManagement)
            {
                LogString("userID",new XesString(myGuidingController.UserManagement.GetUserId()));
                
            }
        
        }


        /// <summary>
        /// Writes the given event to the Eventlog.
        /// </summary>
        /// <param name="eventToLog"></param>
        /// <param name="executedAtRightTime"></param>
        public void WriteEvent(XesEvent eventToLog, bool executedAtRightTime)
        {
            writer?.WriteStartElement("event");

            //go through all fields of the event object and serialize them based on their type
            FieldInfo[] fields = eventToLog.GetType().GetFields();
            foreach (var field in fields)
            {
                if (!(writer is null))
                {
                    Debug.Log(field.Name);
                    LogFromType(field.Name, field.GetValue(eventToLog));
                }
            }
                
            if (!(eventToLog is PositiveFeedback)&&!(eventToLog is NegativeFeedback))
            {
                if (!(writer is null))
                {
                    LogBool("executedAtRightTime", new XesBoolean(executedAtRightTime));
                }
            }
            
            writer?.WriteEndElement();
            writer?.Flush();
            if(writer is null)
            {
                Debug.LogWarning("XMLWriter not initialized");
            }
        }
        
        /// <summary>
        /// Writes the given event to the Eventlog.
        /// </summary>
        /// <param name="eventToLog"></param>
        public void WriteEvent(XesEvent eventToLog)
        {
            writer?.WriteStartElement("event");

            //go through all fields of the event object and serialize them based on their type
            FieldInfo[] fields = eventToLog.GetType().GetFields();
            foreach (var field in fields)
            {
                if (!(writer is null))
                {
                    Debug.Log(field.Name);
                    LogFromType(field.Name, field.GetValue(eventToLog));
                }
            }
            writer?.WriteEndElement();
            writer?.Flush();
            if(writer is null)
            {
                Debug.LogWarning("XMLWriter not initialized");
            }
        }

        

        [ContextMenu("Close")]
        public void CloseWriter()
        {
            writer?.Close();
        }
        
    
        private void LogInt(string attributeName, XesAttribute value)
        {
           
            writer.WriteStartElement("int");
            writer.WriteAttributeString("key",attributeName);
            writer.WriteAttributeString("value",value.ToString());
            
            writer.WriteEndElement();
            writer.Flush();
        }
    
        private void LogFloat(string attributeName, XesAttribute value)
        {
          
            writer.WriteStartElement("float");
            writer.WriteAttributeString("key",attributeName);
            writer.WriteAttributeString("value",value.ToString());
            writer.WriteEndElement();
            writer.Flush();
        }
    
        private void LogBool(string attributeName, XesAttribute value)
        {
            //if(debug!=null)
            //    debug.AddText(attributeName+": "+(value).ToString()+"\n");
            writer.WriteStartElement("boolean");
            writer.WriteAttributeString("key",attributeName);
            writer.WriteAttributeString("value",value.ToString());
            writer.WriteEndElement();
            writer.Flush();
        }
    
        private void LogString(string attributeName, XesAttribute value)
        {
            //if(debug!=null)
            //    debug.AddText(attributeName+": "+(value).ToString()+"\n");
            writer.WriteStartElement("string");
            writer.WriteAttributeString("key",attributeName);
            writer.WriteAttributeString("value",value.ToString());
            writer.WriteEndElement();
            writer.Flush();
        }
    
        private void LogId(string attributeName, XesAttribute value)
        {
            //if(debug!=null)
            //    debug.AddText(attributeName+": "+(value).ToString()+"\n");
            writer.WriteStartElement("id");
            writer.WriteAttributeString("key",attributeName);
            writer.WriteAttributeString("value",value.ToString());
            writer.WriteEndElement();
            writer.Flush();
        }
    
        private void LogDate(string attributeName, XesAttribute value)
        {
            // if(debug!=null)
            //     debug.AddText(attributeName+": "+(value).ToString()+"\n");
            writer.WriteStartElement("date");
            writer.WriteAttributeString("key",attributeName);
            writer.WriteAttributeString("value",value.ToString());
            writer.WriteEndElement();
            writer.Flush();
        }
    
        private void LogList(string attributeName, List<XesCollectionAttribute> list)
        {
            //Do List Specific logging
            writer.WriteStartElement("list");
            writer.WriteAttributeString("key",attributeName);
            LogCollectionAttributes(list);
            //end list specific logging
            writer.WriteEndElement();
            writer.Flush();
        
        
        }
    
        private void LogContainer(string attributeName, List<XesCollectionAttribute> container)
        {
            //Do container Specific logging
            writer.WriteStartElement("container");
            writer.WriteAttributeString("key",attributeName);
            LogCollectionAttributes(container);
            //end container specific logging
            writer.WriteEndElement();
            writer.Flush();
        }

        private void LogCollectionAttributes(List<XesCollectionAttribute> collection)
        {
            foreach (var collectionAttribute in collection)
            {
                LogFromType(collectionAttribute.Name, collectionAttribute.Value);
            }
        }

        /*
     * Logs the extension attribute, so that PM tool can use it as extension
     */
        private void LogExtensionAttribute(XesExtension extensionAttribute)
        {
            writer.WriteStartElement(extensionAttribute.type);
            writer.WriteAttributeString("key",extensionAttribute.prefix+":"+extensionAttribute.key);
            writer.WriteAttributeString("value", extensionAttribute.value.ToString());
            writer.WriteEndElement();
            writer.Flush();
        }

        /*
     * Loggs Custom Attributes.
     * Creates a string attribute containing the name of the custom attribute.
     * Logs the custom attributes' fields as child (nested) attributes     
     */
        private void LogCustomAttribute(string attributeName, XesCustomAttribute customAttribute)
        {
            writer.WriteStartElement("string");
            writer.WriteAttributeString("key","name");
            writer.WriteAttributeString("value",attributeName);
            writer.Flush();
            //go through all fields of the event object and serialize them
            FieldInfo[] fields = customAttribute.GetType().GetFields();
            foreach (var field in fields )
            {
                //Debug.Log(field.Name);
                LogFromType(field.Name,field.GetValue(customAttribute));
            }
            writer.WriteEndElement();
            writer.Flush();
        }
    
        /*
     * Determines the type of the attribute and calls the corresponding logging method
     */
        private void LogFromType(string attributeName, object value)
        {
            object _value = value;
            switch (value)
            {
                case XesInt xesInt:
                    LogInt(attributeName,xesInt);
                    break;
                case XesFloat xesFloat:
                    LogFloat(attributeName,xesFloat);
                    break;
                case XesBoolean boolean:
                    LogBool(attributeName,boolean);
                    break;
                case XesString xesString:
                    LogString(attributeName,xesString);
                    break;
                case XesID id:
                    LogId(attributeName,id);
                    break;
                case XesDate date:
                    LogDate(attributeName,date);
                    break;
                case XesList list:
                    LogList(attributeName,list.Attributes);
                    break;
                case XesContainer container:
                    LogContainer(attributeName,container.Attributes);
                    break;
                case XesExtension xesExtension:
                    LogExtensionAttribute(xesExtension);
                    break;
                case XesCustomAttribute customAttribute:
                    LogCustomAttribute(attributeName,customAttribute);
                    break;
                
            }
        }
    
        public enum XesExtensionType
        {
            Concept,Lifecycle,Organizational,Time,Semantic,Identity,Cost
        }
    
      
    }
}