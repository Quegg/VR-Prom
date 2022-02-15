using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class DetailStubGenerator
{
   

    [ContextMenu("Generation Test")]
    public void TestGeneration()
    {
        
    }

    /// <summary>
    /// Generate a class stub for the given class name in the given directory
    /// </summary>
    /// <param name="directoryPath">directory to create the stubs</param>
    /// <param name="className">name of the class</param>
    public void GenerateTask(string directoryPath, string className)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (File.Exists(directoryPath + "/" + className + ".cs"))
        {
            Debug.Log("File already exists: "+className+".cs");
            return;
        }
        
        using (StreamWriter outfile = 
            new StreamWriter(directoryPath+"/"+className+".cs"))
        {
            outfile.WriteLine("using System;");
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using System.Collections;");
            outfile.WriteLine("using System.Collections.Generic;");
            outfile.WriteLine("using Guiding.Core;");
            outfile.WriteLine("");
            outfile.WriteLine("namespace Guiding.ProcessDetails.Tasks");
            outfile.WriteLine("{");
            outfile.WriteLine("    public class "+className+" : MonoBehaviour, ITaskDetails {");
            outfile.WriteLine("        private GuidingController myGuidingController;");
            outfile.WriteLine("        private string classNameRaw=\"" + className + "\";");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //Return the class' name");
            outfile.WriteLine("        public string GetNameRaw()");
            outfile.WriteLine("        {");
            outfile.WriteLine("            return classNameRaw;");
            outfile.WriteLine("        }");
            outfile.WriteLine("        ");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //Show custom help for this task");
            outfile.WriteLine("        public void ShowHelp()");
            outfile.WriteLine("        {");
            outfile.WriteLine("            throw new NoCustomHelpException();");
            outfile.WriteLine("        }");
            outfile.WriteLine("        ");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //Is help offered in this moment?");
            outfile.WriteLine("        public bool HelpAvailable()");
            outfile.WriteLine("        {");
            outfile.WriteLine("            throw new NotImplementedException();");
            outfile.WriteLine("        }");
            outfile.WriteLine("        ");         
            outfile.WriteLine("        ");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //return, if the current task is done to check if the user can go to the next");
            outfile.WriteLine("        public bool IsDone()");
            outfile.WriteLine("        {");
            outfile.WriteLine("            throw new NotImplementedException();");
            outfile.WriteLine("        }");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //Is called, when the user starts to execute this task");
            outfile.WriteLine("        public void TaskStarted()");
            outfile.WriteLine("        {");
            outfile.WriteLine("            throw new NotImplementedException();");
            outfile.WriteLine("        }");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //Is called, when the user starts to execute this task");
            outfile.WriteLine("        public void TaskEnded()");
            outfile.WriteLine("        {");
            outfile.WriteLine("            throw new NotImplementedException();");
            outfile.WriteLine("        }");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //Gets called by the GuidingController when loading the class);");
            outfile.WriteLine("        public void Initialize(GuidingController guidingController)");
            outfile.WriteLine("        {");
            outfile.WriteLine("            myGuidingController = guidingController;");
            outfile.WriteLine("            ");
            outfile.WriteLine("        }");
            outfile.WriteLine("    }");
            outfile.WriteLine("}");
        }

    }

    
    
    /// <summary>
    /// Generate a class stub for the given class name in the given directory
    /// </summary>
    /// <param name="directoryPath">directory to create the stubs</param>
    /// <param name="className">name of the class</param>
    public void GenerateErrorHelp(string directoryPath, string className)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (File.Exists(directoryPath + "/" + className + ".cs"))
        {
            Debug.Log("File already exists: "+className+".cs");
            return;
        }
        
        using (StreamWriter outfile = 
            new StreamWriter(directoryPath+"/"+className+".cs"))
        {
            outfile.WriteLine("using System;");
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using System.Collections;");
            outfile.WriteLine("using System.Collections.Generic;");
            outfile.WriteLine("using Guiding.Core;");
            outfile.WriteLine("");
            outfile.WriteLine("namespace Guiding.ProcessDetails.ErrorHelp");
            outfile.WriteLine("{");
            outfile.WriteLine("    public class "+className+" : MonoBehaviour, IErrorHelp {");
            outfile.WriteLine("        ");
            outfile.WriteLine("        ");
            outfile.WriteLine("        private string className=\""+className+"\";");
            outfile.WriteLine("        private GuidingController myGuidingController;");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //Return the class' name");
            outfile.WriteLine("        public string GetName()");
            outfile.WriteLine("        {");
            outfile.WriteLine("            return className;");
            outfile.WriteLine("        }");
            outfile.WriteLine("        ");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //Show Help, if this error occurs. Else Help for current task is shown");
            outfile.WriteLine("        public void ShowHelp()");
            outfile.WriteLine("        {");
            outfile.WriteLine("            throw new NotImplementedException();");
            outfile.WriteLine("        }");
            outfile.WriteLine("        ");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //Is help offered in this moment?");
            outfile.WriteLine("        public bool HelpAvailable()");
            outfile.WriteLine("        {");
            outfile.WriteLine("            throw new NotImplementedException();");
            outfile.WriteLine("        }");
            outfile.WriteLine("        ");         
            outfile.WriteLine("        ");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //Gets called by the GuidingController when loading the class);");
            outfile.WriteLine("        public void Initialize(GuidingController guidingController)");
            outfile.WriteLine("        {");
            outfile.WriteLine("            myGuidingController = guidingController;");
            outfile.WriteLine("            ");
            outfile.WriteLine("        }");
            outfile.WriteLine("    }");
            outfile.WriteLine("}");
        }

    }
    
    /// <summary>
    /// Generates a Xes logging event for the given class name in the given directory
    /// </summary>
    /// <param name="directoryPath">directory to create the stubs</param>
    /// <param name="className">name of the class</param>
    public void GenerateLoggingEvent(string directoryPath, string className, bool isError)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (File.Exists(directoryPath + "/" + className + ".cs"))
        {
            Debug.Log("File already exists: "+className+".cs");
            return;
        }
        
        using (StreamWriter outfile = 
            new StreamWriter(directoryPath+"/"+className+".cs"))
        {
            outfile.WriteLine("using System;");
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using XesAttributes;");
            outfile.WriteLine("");
            outfile.WriteLine("namespace Guiding.LoggingEvents");
            outfile.WriteLine("{");
            outfile.WriteLine("    public class "+className+" : XesEvent {");
            outfile.WriteLine("        ");
            outfile.WriteLine("        public bool isError="+isError.ToString().ToLower()+";");
            outfile.WriteLine("        public bool isFeedback=false;");
            outfile.WriteLine("        public XesTimestamp time;");
            outfile.WriteLine("        public XesConceptName eventName;");
            outfile.WriteLine("        public XesLifecycleTransition lifecycle;");
            outfile.WriteLine("        ");
            outfile.WriteLine("        ");
            outfile.WriteLine("        public  "+className+"(DateTime time,XesLifecycleTransition lifecycleState)");
            outfile.WriteLine("        {");
            outfile.WriteLine("        eventName= new XesConceptName(new XesString(\""+className+"\"));");
            outfile.WriteLine("        this.time =new XesTimestamp(new XesDate(time));");
            outfile.WriteLine("        lifecycle = lifecycleState;");
            outfile.WriteLine("        ");
            outfile.WriteLine("        }");
            outfile.WriteLine("        ");
            outfile.WriteLine("    }");
            outfile.WriteLine("}");
        }
    }
    /// <summary>
    /// Generate a class stub for the given class name in the given directory
    /// </summary>
    /// <param name="directoryPath">directory to create the stubs</param>
    /// <param name="className">name of the class</param>
    public void GenerateExclusiveGateway(string directoryPath, string className)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (File.Exists(directoryPath + "/" + className + ".cs"))
        {
            Debug.Log("File already exists: "+className+".cs");
            return;
        }
        using (StreamWriter outfile =
            new StreamWriter(directoryPath + "/" + className + ".cs"))
        {
            outfile.WriteLine("using System;");
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using System.Collections;");
            outfile.WriteLine("using System.Collections.Generic;");
            outfile.WriteLine("using Guiding.Core;");
            outfile.WriteLine("");
            outfile.WriteLine("namespace Guiding.ProcessDetails.Gateways");
            outfile.WriteLine("    {");
            outfile.WriteLine("    public class "+className+" : MonoBehaviour, IGatewayDetails {");
            outfile.WriteLine("        ");
            outfile.WriteLine("        ");
            outfile.WriteLine("        private string className=\""+className+"\";");
            outfile.WriteLine("        ");
            outfile.WriteLine("        //Return the class' name");
            outfile.WriteLine("        public string GetName()");
            outfile.WriteLine("        {");
            outfile.WriteLine("            return className;");
            outfile.WriteLine("        }");
            outfile.WriteLine("        ");
            outfile.WriteLine("        ");
            outfile.WriteLine("        // Check the conditions for the exclusive Gateways followed by this task. Return the id of the next task");
            outfile.WriteLine("        public string CheckConditions(List<string> possibleNextElements)");
            outfile.WriteLine("        {");
            outfile.WriteLine("            throw new NotImplementedException();");
            outfile.WriteLine("        }");
            outfile.WriteLine("    }");
            outfile.WriteLine("}");
        }
    }
}
