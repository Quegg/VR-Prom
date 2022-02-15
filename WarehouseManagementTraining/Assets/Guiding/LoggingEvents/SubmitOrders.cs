using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class SubmitOrders : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        
        
        public  SubmitOrders(DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("SubmitOrders"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        
        }
        
    }
}
