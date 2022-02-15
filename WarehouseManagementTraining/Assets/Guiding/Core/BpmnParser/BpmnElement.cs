using System.Collections;
using System.Collections.Generic;
using Guiding.Core.BpmnParser;
using UnityEngine;

public abstract class BpmnElement
{
    public string id;
    public string nameRaw;
    public string nameCut;
    public List<BpmnSequenceFlow> incoming;
    public List<BpmnSequenceFlow> outgoing;
}
