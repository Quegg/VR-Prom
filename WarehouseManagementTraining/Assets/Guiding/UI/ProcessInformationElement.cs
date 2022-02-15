using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessInformationElement
{
    public ElementType myType;
    public string nameRaw;
    public string nameCut;
    public List<ProcessInformationElement> decedents;
    public string inComingSequenceFlowInfo;
    
    
    public enum ElementType{StartEvent, EndEvent, ExclusiveGateway, Task};
    
}
