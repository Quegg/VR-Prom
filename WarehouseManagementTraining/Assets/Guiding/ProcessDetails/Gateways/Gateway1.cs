using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;

namespace Guiding.ProcessDetails.Gateways
    {
    public class Gateway1 : MonoBehaviour, IGatewayDetails {
        
        public GameObject robotSwitch;
        private StartRobotButton robotButton;
        
        private string className="Gateway1";
        
        private void Start()
        {
            robotButton = robotSwitch.GetComponentInChildren<StartRobotButton>();
        }
        
        //Return the class' name
        public string GetName()
        {
            return className;
        }
        
        
        // Check the conditions for the exclusive Gateways followed by this task. Return the id of the next task
        public string CheckConditions(List<string> possibleNextElements)
        {
            if(robotButton.IsRobotActive())
                return "Gateway2";
            else
            {
                return "ActivateRobot";
            }
        }
    }
}
