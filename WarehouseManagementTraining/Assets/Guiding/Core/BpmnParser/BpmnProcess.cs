using System.Collections;
using System.Collections.Generic;
using Guiding.Core.BpmnParser;
using UnityEngine;

public class BpmnProcess:BpmnElement
{
    public List<BpmnStartEvent> startEvents;
    public List<BpmnTask> allTasks;
    public List<BpmnExclusiveGateway> allGateways;
}
