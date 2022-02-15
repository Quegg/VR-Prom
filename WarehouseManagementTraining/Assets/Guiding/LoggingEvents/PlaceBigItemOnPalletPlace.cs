using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class PlaceBigItemOnPalletPlace : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesInt orderNumber;
        public XesString itemName;
        public XesBoolean isCorrectPalletPLace;
        
        public  PlaceBigItemOnPalletPlace(int orderNumber, string itemName, bool isCorrectPalletPlace, DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("PlaceBigItemOnPalletPlace"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.orderNumber = new XesInt(orderNumber);
        this.itemName = new XesString(itemName);
        
        }
        
    }
}
