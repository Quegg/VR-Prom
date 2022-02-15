using System;
using UnityEngine;
using XesAttributes;

namespace Guiding.LoggingEvents
{
    public class NegativeFeedback : XesEvent {
        
        public bool isError=false;
        public bool isFeedback=true;
        public XesTimestamp time;
        public XesConceptName eventName;
        public XesLifecycleTransition lifecycle;
        public XesString sectionName;
        
        
        
        public  NegativeFeedback(string sectionName, DateTime time,XesLifecycleTransition lifecycleState)
        {
        eventName= new XesConceptName(new XesString("NegativeFeedback"));
        this.time =new XesTimestamp(new XesDate(time));
        lifecycle = lifecycleState;
        this.sectionName = new XesString(sectionName);
        }
        
    }
}
