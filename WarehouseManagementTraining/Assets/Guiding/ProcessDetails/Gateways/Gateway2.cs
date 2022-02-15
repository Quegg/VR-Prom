using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;

namespace Guiding.ProcessDetails.Gateways
    {
    public class Gateway2 : MonoBehaviour, IGatewayDetails {
        
        
        private string className="Gateway2";
        
        //Return the class' name
        public string GetName()
        {
            return className;
        }
        
        
        // Check the conditions for the exclusive Gateways followed by this task. Return the id of the next task
        public string CheckConditions(List<string> possibleNextElements)
        {
            return "SelectOrderInMenu";
        }
    }
}
