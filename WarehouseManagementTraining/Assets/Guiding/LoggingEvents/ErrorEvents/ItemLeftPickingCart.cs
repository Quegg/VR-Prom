using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class ItemLeftPickingCart : XesEvent {
        
        public bool isError=true;
        public XesBoolean isErrorEvent = new XesBoolean(true);
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesString itemName;
        
        public  ItemLeftPickingCart(string itemName, DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("ItemLeftPickingCart"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.itemName = new XesString(itemName);
        }
        
    }
}
