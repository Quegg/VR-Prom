using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class ScanForklift : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        
        
        public  ScanForklift(DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("ScanForklift"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        }
        
    }
}
