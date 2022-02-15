using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class DriveForkliftToPallets : XesEvent {
        
        public bool isError=false;
        public bool isFeedback=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        
        
        public  DriveForkliftToPallets(DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("DriveForkliftToPallets"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        
        }
        
    }
}
