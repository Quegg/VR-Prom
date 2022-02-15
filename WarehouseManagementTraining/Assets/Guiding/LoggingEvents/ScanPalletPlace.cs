using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class ScanPalletPlace : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesString palletPlaceNumber;
        
        public  ScanPalletPlace(string palletPlaceNumber, DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("ScanPalletPlace"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.palletPlaceNumber = new XesString(palletPlaceNumber);
        }
        
    }
}
