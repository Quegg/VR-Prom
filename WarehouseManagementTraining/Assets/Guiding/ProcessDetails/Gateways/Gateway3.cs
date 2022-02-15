using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Guiding.Core;
using Orders;

namespace Guiding.ProcessDetails.Gateways
    {
    public class Gateway3 : MonoBehaviour, IGatewayDetails {
        
        public Robot robot;
        public OrderManager orderManager;
        
        private string className="Gateway3";
        
        //Return the class' name
        public string GetName()
        {
            return className;
        }
        
        
        // Check the conditions for the exclusive Gateways followed by this task. Return the id of the next task
        public string CheckConditions(List<string> possibleNextElements)
        {
            Dictionary<string, int> itemCounts= new Dictionary<string, int>();
            List<GameObject> objectsInRobot = robot.MyCarriage.AllObjectsInCarriage;
            foreach (var obj in objectsInRobot)
            {
                SmallItem smallItemScript = obj.GetComponent<SmallItem>();
                if (itemCounts.ContainsKey(smallItemScript.GetId()))
                {
                    itemCounts[smallItemScript.GetId()]++;
                
                }
                else
                {
                    itemCounts.Add(smallItemScript.GetId(),1);
                }
            }

            //is true if at least the needed amount of items is in the robot
            bool itemsCollected = true;
            Dictionary<Item,int> neededItems = orderManager.ItemsNeededInCurrentOrder();
            foreach (var item in neededItems)
            {
                if(!itemCounts.ContainsKey(((SmallItem)item.Key).id))
                {
                    itemsCollected = false;
                    break;
                } 
                else {
                    string key =((SmallItem)item.Key).id;
                    if(!(itemCounts[key]>=item.Value) )
                    {
                        itemsCollected = false;
                        break;
                    }
                    
                }
            }

            if (itemsCollected)
            {
                return "Gateway4";
            }
            else
            {
                return "PickUpItem0";
            }


        
        }
    }
}
