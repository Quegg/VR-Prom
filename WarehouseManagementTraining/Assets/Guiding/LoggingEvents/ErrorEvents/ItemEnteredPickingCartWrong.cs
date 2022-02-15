using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class ItemEnteredPickingCartWrong : XesEvent {
        
        public bool isError=true;
        public XesBoolean isErrorEvent = new XesBoolean(true);
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesString itemName;
        public XesString locationCategory;
        
        public  ItemEnteredPickingCartWrong(string itemName,string locationCategory,DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("ItemEnteredPickingCartWrong"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.itemName = new XesString(itemName);
        this.locationCategory = new XesString(locationCategory);
        }
        
    }
}
