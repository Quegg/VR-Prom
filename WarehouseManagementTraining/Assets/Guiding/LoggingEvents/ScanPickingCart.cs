using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class ScanPickingCart : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesString orderNumber;
        
        
        public  ScanPickingCart(string orderNumber, DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("ScanPickingCart"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.orderNumber = new XesString(orderNumber);
        }
        
    }
}
