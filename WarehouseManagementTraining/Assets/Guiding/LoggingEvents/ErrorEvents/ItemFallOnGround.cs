using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class ItemFallOnGround : XesEvent {
        
        public bool isError=true;
        public XesBoolean isErrorEvent = new XesBoolean(true);
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesString itemName;
        
        public  ItemFallOnGround(string itemName, DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("ItemFallOnGround"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.itemName = new XesString(itemName);
        }
        
    }
}
