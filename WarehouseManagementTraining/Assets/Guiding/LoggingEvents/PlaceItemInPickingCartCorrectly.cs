using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class PlaceItemInPickingCartCorrectly : XesEvent {
        
        public bool isError=false;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesString itemName;
        public XesString locationCategory;
        
        
        public  PlaceItemInPickingCartCorrectly(string itemName, string locationCategory,DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("PlaceItemInPickingCartCorrectly"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.itemName = new XesString(itemName);
        this.locationCategory = new XesString(locationCategory);
        }
        
    }
}
