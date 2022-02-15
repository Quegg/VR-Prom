using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class ForkliftHitsObject : XesEvent {
        
        public bool isError=true;
        public XesBoolean isErrorEvent = new XesBoolean(true);
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        
        
        public  ForkliftHitsObject(DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("ForkliftHitsObject"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        
        }
        
    }
}
