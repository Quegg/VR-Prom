using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class PlaceItemInRobot : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesString itemName;
        
        
        public  PlaceItemInRobot(string itemName, DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("PlaceItemInRobot"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.itemName = new XesString(itemName);
        }
        
    }
}
