using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XesLifecycleTransition : XesLifecycleExtension
{
    public static XesLifecycleTransition schedule = new XesLifecycleTransition(new XesString("schedule"));
    public static XesLifecycleTransition assign=new XesLifecycleTransition(new XesString("assign"));
    public static XesLifecycleTransition withdraw=new XesLifecycleTransition(new XesString("withdraw"));
    public static XesLifecycleTransition reassign=new XesLifecycleTransition(new  XesString("reassign"));
    public static XesLifecycleTransition start=new XesLifecycleTransition (new XesString("start"));
    public static XesLifecycleTransition suspend=new XesLifecycleTransition (new XesString("suspend"));
    public static XesLifecycleTransition resume=new XesLifecycleTransition (new XesString("resume"));
    public static XesLifecycleTransition pi_abort=new XesLifecycleTransition (new XesString("pi_abort"));
    public static XesLifecycleTransition ate_abort=new XesLifecycleTransition (new XesString("ate_abort"));
    public static XesLifecycleTransition complete=new XesLifecycleTransition (new XesString("complete"));
    public static XesLifecycleTransition autoskip=new XesLifecycleTransition (new XesString("autoskip"));
    public static XesLifecycleTransition manualskip=new XesLifecycleTransition (new XesString("manualskip"));
    public static XesLifecycleTransition unknown=new XesLifecycleTransition(new XesString("unknown"));
    
    public XesLifecycleTransition(XesString value) : base(value)
    {
        key = "transition";
    }
}
