using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class ScanRobot : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        
        
        public  ScanRobot(DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("ScanRobot"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        
        }
        
    }
}
