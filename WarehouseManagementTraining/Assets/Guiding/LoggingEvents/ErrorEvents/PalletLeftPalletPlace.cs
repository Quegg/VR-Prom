using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class PalletLeftPalletPlace : XesEvent {
        
        public bool isError=true;
        public XesBoolean isErrorEvent = new XesBoolean(true);
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesInt orderNumber;
        public XesString itemName;
        
        public  PalletLeftPalletPlace(int orderNumber, string itemName, DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("PalletLeftPalletPlace"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.orderNumber = new XesInt(orderNumber);
        this.itemName = new XesString(itemName);
        }
        
    }
}
