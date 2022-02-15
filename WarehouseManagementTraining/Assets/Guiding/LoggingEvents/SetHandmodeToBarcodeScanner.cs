using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class SetHandmodeToBarcodeScanner : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        
        
        public  SetHandmodeToBarcodeScanner(DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("SetHandmodeToBarcodeScanner"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        
        }
        
    }
}
