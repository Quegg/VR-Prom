using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;
using Orders;

namespace Guiding.ProcessDetails.Gateways
    {
    public class Gateway5 : MonoBehaviour, IGatewayDetails {
        
        public OrderManager orderManager;
        private string className="Gateway5";
        
        //Return the class' name
        public string GetName()
        {
            return className;
        }
        
        
        // Check the conditions for the exclusive Gateways followed by this task. Return the id of the next task
        public string CheckConditions(List<string> possibleNextElements)
        {
            if (orderManager.CheckBigItemsOfCurrentOrder())
            {
                return "MarkOrderAsComplete";
            }
            else
            {
                return "DriveForkliftToPallets";
            }
        }
    }
}
