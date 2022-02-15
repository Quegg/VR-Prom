using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class MarkOrderAsComplete : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesInt orderNumber;
        
        
        public  MarkOrderAsComplete(int orderNumber,DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("MarkOrderAsComplete"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.orderNumber = new XesInt(orderNumber);
        
        }
        
    }
}
