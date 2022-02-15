using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Guiding.Core.BpmnParser;
using UnityEngine;


public class StateMachieneController
{
    private BpmnProcess process;
    private BpmnElement lastState;
    public BpmnElement LastState => lastState;
    
    
    public void Initialize(string filepath, bool goToFirstTask)
    {
        BPMNParser parser = new BPMNParser(); 
        process = parser.ParseBPMN(filepath);
        lastState = process.startEvents[0];
        if (goToFirstTask)
            lastState = lastState.outgoing[0].target;
    }

    public BpmnExclusiveGateway GetGatewayByName(string gatewayName)
    {
        foreach (var gateway in process.allGateways)
        {
            if (gateway.nameRaw.Equals(gatewayName))
                return gateway;
        }
        return null;
    }
    
    public BpmnTask GetTaskByName(string taskName)
    {
        foreach (var task in process.allTasks)
        {
            if (task.nameRaw.Equals(taskName))
                return task;
        }
        return null;
    }

    public BpmnElement GetElementByName(string elementName)
    {
        BpmnElement element = GetGatewayByName(elementName);
        if (!(element is null))
        {
            return element;
        }

        return GetTaskByName(elementName);
    }



    
    /// <summary>
    /// Updates the current state to the next state with matching name
    /// </summary>
    /// <param name="stateId"></param>
    public void NextState(string stateId)
    {
        BpmnElement newState = GetNextStateById(lastState, stateId);
        lastState = newState ?? throw new Exception("Cannot find next State: "+stateId);
    }


    
    /// <summary>
    /// Searches all possible nextStates and returns it if the name matches
    /// </summary>
    /// <param name="currentElement"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public BpmnElement GetNextStateById(BpmnElement currentElement, string id)
    {
        foreach (var flow in currentElement.outgoing)
        {
            Debug.Log(flow.target.id);
            if (flow.target is BpmnTask)
            {
                if (flow.target.id.Equals(id))
                {
                    return flow.target;
                }
            }
            else if (flow.target is BpmnExclusiveGateway)
            {
                BpmnElement nextTask =GetNextStateById(flow.target, id);

                if (!(nextTask is null))
                    return nextTask;
            }
        }

        return null;
    }

    
    /// <summary>
    /// Returns the next Tasks
    /// </summary>
    /// <param name="startState"></param>
    /// <returns></returns>
    public List<BpmnElement> GetNextStates(BpmnElement startState)
    {
        List<BpmnElement> nextStates = new List<BpmnElement>();

        foreach (var flow in startState.outgoing)
        {
            if (flow.target is BpmnTask)
            {
                nextStates.Add(flow.target);
            }
            
           
            else if (flow.target is BpmnExclusiveGateway)
            {
                nextStates.AddRange(GetNextStates(flow.target));
            }
        }

        return nextStates;
    }
    
}


