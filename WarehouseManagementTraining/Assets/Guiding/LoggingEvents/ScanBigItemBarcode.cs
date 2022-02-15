using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class ScanBigItemBarcode : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesString objectName;
        
        
        public  ScanBigItemBarcode(string objectName, DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("ScanBigItemBarcode"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.objectName = new XesString(objectName);
        }
        
    }
}
