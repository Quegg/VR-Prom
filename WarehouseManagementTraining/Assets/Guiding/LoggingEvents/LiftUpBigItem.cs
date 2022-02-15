using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class LiftUpBigItem : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesVector3 position;
        
        
        public  LiftUpBigItem(Vector3 position, DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("LiftUpBigItem"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.position = new XesVector3(position);
        }
        
    }
}
