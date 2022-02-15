using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class ActivateRobot : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;


        public ActivateRobot(DateTime time, XesLifecycleTransition lifecycleState)
        {
            eventName = new XesConceptName(new XesString("ActivateRobot"));
            this.time = new XesTimestamp(new XesDate(time));
            lifecycle = lifecycleState;
        }
    }
}
