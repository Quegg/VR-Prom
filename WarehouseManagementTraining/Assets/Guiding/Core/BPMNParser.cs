using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Guiding.Core.BpmnParser;
using UnityEngine;


public class BPMNParser
{
    private List<BpmnElement> elements;
    private List<BpmnSequenceFlow> sequenceFlows;
    private List<BpmnStartEvent> startEvents;
    private BpmnProcess process;
   
    /// <summary>
    /// parses the .bpmn file at the given path and returns the corresponding BpmnProcess
    /// </summary>
    /// <param name="filepath">path of the .bpmn file</param>
    /// <returns>BpmnProcess object of given process</returns>
    /// <exception cref="Exception"></exception>
    public BpmnProcess ParseBPMN(string filepath)
    {
        process= new BpmnProcess();
        startEvents = new List<BpmnStartEvent>();
        elements= new List<BpmnElement>();
        sequenceFlows=new List<BpmnSequenceFlow>();
     
        XDocument doc = XDocument.Load(filepath);
        var sequenceFlow = 
            from e in doc.Descendants()
            where e.Name.LocalName == "sequenceFlow"
            select e;
        
        foreach (var element in sequenceFlow)
        {
            
            BpmnSequenceFlow sqf = new BpmnSequenceFlow();
            string _id = "";
            string _name = "";
            string _sourceId = "";
            string _targetId = "";
            
            
                _id = element.Attribute("id")?.Value;
            
                _name = element.Attribute("name")?.Value;

                
                _sourceId = element.Attribute("sourceRef")?.Value;
            
                _targetId = element.Attribute("targetRef")?.Value;
            
            sqf.name = _name;
            sqf.id = _id;
            sqf.sourceId = _sourceId;
            sqf.targetId = _targetId;
            sequenceFlows.Add(sqf);
        }
        
        
        process.allTasks = new List<BpmnTask>();
        var tasks = 
            from e in doc.Descendants()
            where e.Name.LocalName == "task"
            select e;
        
        
        foreach (var element in tasks)
        {
            
            BpmnTask newTask = new BpmnTask();
            SetAttributesAndReferences(element, newTask);
            process.allTasks.Add(newTask);
            
        }
        
        
        var startEvent = 
            from e in doc.Descendants()
            where e.Name.LocalName == "startEvent"
            select e;
        
        foreach (var element in startEvent)
        {
            BpmnStartEvent se = new BpmnStartEvent();
            startEvents.Add((BpmnStartEvent)SetAttributesAndReferences(element,se));
        }

        var endEvent = 
            from e in doc.Descendants()
            where e.Name.LocalName == "endEvent"
            select e;
        
        
        foreach (var element in endEvent)
        {
            BpmnEndEvent ee = new BpmnEndEvent();
            SetAttributesAndReferences(element,ee);
        }
        
        process.allGateways= new List<BpmnExclusiveGateway>();
        
        var exclusiveGateway = 
            from e in doc.Descendants()
            where e.Name.LocalName == "exclusiveGateway"
            select e;
        
        foreach (var element in exclusiveGateway)
        {
            BpmnExclusiveGateway gateway =GetExclusiveGatewayById(element.Attribute("id").Value,process.allGateways);
            if (!(gateway is null))
            {
                throw new Exception("Das sollte nicht so sein");
            }

            BpmnExclusiveGateway eg = new BpmnExclusiveGateway();
                SetAttributesAndReferences(element, eg);
                process.allGateways.Add(eg);
            
            
        }
        
        //after generating the elements, we can finally set the references on the sequence flows

        foreach (var sqf in sequenceFlows)
        {
            if (!(sqf.sourceId is null))
            {
                sqf.source = GetElementById(sqf.sourceId);
            }
            if (!(sqf.targetId is null))
            {
                sqf.target = GetElementById(sqf.targetId);
            }
        }

        process.startEvents = startEvents;
        return process;
    }

    /// <summary>
    /// helper class to set the attributes and references for a single bpmnelement
    /// </summary>
    /// <param name="element"></param>
    /// <param name="bpmnElement"></param>
    /// <returns></returns>
    private BpmnElement SetAttributesAndReferences(XElement element, BpmnElement bpmnElement)
    {
        string _id = "";
        string _name = "";
       
            _id = element.Attribute("id")?.Value;
        
            _name = element.Attribute("name")?.Value;
        
        bpmnElement.nameRaw = _name;
        if(_name!=null)
            bpmnElement.nameCut = CutNumbersAtEnd(_name);
        bpmnElement.id = _id;
        bpmnElement.incoming= new List<BpmnSequenceFlow>();
        bpmnElement.outgoing= new List<BpmnSequenceFlow>();
            
        var incoming =
            from e in element.Descendants()
            where e.Name.LocalName == "incoming"
            select e;
        foreach (var sqfIn in incoming)
        {
            bpmnElement.incoming.Add(GetSequenceFlowById(sqfIn.Value));
        }
            
        var outgoing =
            from e in element.Descendants()
            where e.Name.LocalName == "outgoing"
            select e;
        foreach (var sqfOut in outgoing)
        {
            bpmnElement.outgoing.Add(GetSequenceFlowById(sqfOut.Value));
        }
        elements.Add(bpmnElement);

        return bpmnElement;
    }

    [ContextMenu("TestParsing")]
    public void TestParsing()
    {
        ParseBPMN(Application.dataPath+"/StreamingAssets/processParse.bpmn");
    }

    /// <summary>
    /// returns the BpmnSequenceFlow matching the given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private BpmnSequenceFlow GetSequenceFlowById(string id)
    {
        foreach (var sqf in sequenceFlows)
        {
            if (sqf.id.Equals(id))
                return sqf;
        }
        return null;
    }
    
    /// <summary>
    /// returns the BpmnElement matching the given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private BpmnElement GetElementById(string id)
    {
        foreach (var element in elements)
        {
            if (element.id.Equals(id))
                return element;
        }
        return null;
    }

    /// <summary>
    /// returns the BpmnExclusiveGateway matching the given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private BpmnExclusiveGateway GetExclusiveGatewayById(string id, List<BpmnExclusiveGateway> gateways)
    {
        foreach (var gw in gateways)
        {
            if (gw.id.Equals(id))
                return gw;
        }

        return null;
    }

    /// <summary>
    /// Helper function that removes all numbers at the end of an string
    /// </summary>
    /// <param name="stringToCut"></param>
    /// <returns></returns>
    private string CutNumbersAtEnd(string stringToCut)
    {
        Regex rx = new Regex(@"(\d)*$",RegexOptions.Compiled | RegexOptions.IgnoreCase);
        return stringToCut.Substring(0, stringToCut.Length - rx.Match(stringToCut).Length);
    }
}
