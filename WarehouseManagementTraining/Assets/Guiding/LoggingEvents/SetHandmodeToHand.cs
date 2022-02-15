using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class SetHandmodeToHand : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        
        
        public  SetHandmodeToHand(DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("SetHandmodeToHand"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        
        }
        
    }
}
