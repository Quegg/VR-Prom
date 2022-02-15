using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class ScanUndefinedBarcode : XesEvent {
        
        public bool isError=true;
        public XesBoolean isErrorEvent = new XesBoolean(true);
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        
        
        public  ScanUndefinedBarcode(DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("ScanUndefinedBarcode"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        }
        
    }
}
