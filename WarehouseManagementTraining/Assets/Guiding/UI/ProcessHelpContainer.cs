using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Guiding.Core.BpmnParser;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class ProcessHelpContainer : MonoBehaviour
{
    
    public GameObject startEventPrefab;
    public GameObject endEventPrefab;
    public GameObject taskPrefab;
    public GameObject gatewayPrefab;
    public GameObject arrowTipPrefab;
    public GameObject containerPrefab;
    public GameObject pointPrefab;
    public GameObject linePrefab;
    
    public Color lastElementTint;
    

    public float height;
    public float hSpace;

    [HideInInspector]public List<Transform> nextContainers;
    public Transform startPoint;

    [HideInInspector] public GuidingController guidingController;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position+Vector3.up*height/2-Vector3.right/3,transform.position+Vector3.up*height/2-Vector3.left/3);
        Gizmos.DrawLine(transform.position-Vector3.up*height/2-Vector3.right/3,transform.position-Vector3.up*height/2-Vector3.left/3);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position+Vector3.up*height/2+Vector3.right/3+Vector3.left*hSpace,transform.position-Vector3.up*height/2+Vector3.right/3+Vector3.left*hSpace);
        Gizmos.DrawLine(transform.position+Vector3.up*height/2+Vector3.right/3,transform.position-Vector3.up*height/2+Vector3.right/3);
        
    }

    
    public void ShowProcessInformation(BpmnElement state)
    {
        if (guidingController is null)
        {
            guidingController = FindObjectOfType<GuidingController>();
        }
        foreach (Transform child in startPoint.transform)
        {
            Destroy(child.gameObject);
        }
        ProcessInformationElement info = GenerateElementsFromTaskStart(state);
        DrawElement(info,true);
    }

    //Draw the images and return the coordinates of the inPoints to draw the lines
    private Transform DrawElement(ProcessInformationElement element,bool isFirst)
    {
            //spawn the correct sprite based on the type
            GameObject newObject=null;
            GameObject arrowTipObject=null;
            if (element.myType == ProcessInformationElement.ElementType.Task)
            {
                newObject = Instantiate(taskPrefab, startPoint.transform);
                newObject.GetComponentInChildren<TextMeshProUGUI>().text = element.nameCut;
                if (element.nameCut.Equals(guidingController.NameCutOfCurrentTask))
                {
                    newObject.transform.Find("Frame").gameObject.SetActive(true);
                }
            }
            else if (element.myType == ProcessInformationElement.ElementType.ExclusiveGateway)
            {
                newObject = Instantiate(gatewayPrefab, startPoint.transform);
            }
            else if (element.myType == ProcessInformationElement.ElementType.StartEvent)
            {
                newObject = Instantiate(startEventPrefab, startPoint.transform);
            }
            else if (element.myType == ProcessInformationElement.ElementType.EndEvent)
            {
                newObject = Instantiate(endEventPrefab, startPoint.transform);
            }

            if(!isFirst)
            {
                if (!(newObject is null))
                {
                    Transform arrowTipTransform =Instantiate(arrowTipPrefab, newObject.GetComponent<UIConnectors>().inPoint).transform;
                    arrowTipTransform.localPosition+=Vector3.right*0.003f;
                }
                else
                {
                    Debug.LogError("ProcessInformationElement type not found: "+element.nameRaw);
                }
                
            }
            else
            {
                Image image = newObject.GetComponent<Image>();
                    if(image is null)
                        image=newObject.GetComponentInChildren<Image>();
                image.color = lastElementTint;
            }

            //generate containers for the next "generation"
            InitializeNextContainerPoints(element.decedents.Count);
            for (int i = 0; i < nextContainers.Count; i++)
            {
                GameObject container = Instantiate(containerPrefab, nextContainers[i].position,// + hSpace * Vector3.left,
                    nextContainers[i].rotation, nextContainers[i]);
                ProcessHelpContainer containerScript=container.AddComponent<ProcessHelpContainer>();
                //pass all the paramenters to the containers of the next generation
                containerScript.guidingController = guidingController;
                containerScript.taskPrefab = taskPrefab;
                containerScript.gatewayPrefab = gatewayPrefab;
                containerScript.arrowTipPrefab = arrowTipPrefab;
                containerScript.containerPrefab = containerPrefab;
                containerScript.pointPrefab = pointPrefab;
                containerScript.linePrefab = linePrefab;
                containerScript.startEventPrefab = startEventPrefab;
                containerScript.endEventPrefab = endEventPrefab;

                containerScript.height = height/nextContainers.Count;
                containerScript.hSpace = hSpace;
                containerScript.startPoint=Instantiate(pointPrefab,  container.transform.position + hSpace / 2 * Vector3.left, //-height/2*Vector3.up,
                    container.transform.localRotation, container.transform).transform;
               
                //connect the sprites with a line
                Transform toPoint=container.GetComponent<ProcessHelpContainer>().DrawElement(element.decedents[i],false);
                GameObject lineObject = Instantiate(linePrefab, container.transform);
                lineObject.GetComponent<LineRendererPositionHelper>().from=newObject.GetComponentInChildren<UIConnectors>().outPoint;
                lineObject.GetComponent<LineRendererPositionHelper>().to=toPoint;
                if (!string.IsNullOrEmpty(element.decedents[i].inComingSequenceFlowInfo))
                {
                    lineObject.GetComponent<LineRendererPositionHelper>().SetLineInformation(element.decedents[i].inComingSequenceFlowInfo);
                }
                
            }
            return newObject.GetComponent<UIConnectors>().inPoint;
    }

    //generates ProcessInformationElement until next task occurs 
    private ProcessInformationElement GenerateElementsFromTaskStart(BpmnElement element)
    {
         ProcessInformationElement help = new ProcessInformationElement();
         help.decedents= new List<ProcessInformationElement>();
         help.nameRaw = element.nameRaw;
         help.nameCut = element.nameCut;
         help.inComingSequenceFlowInfo = "";
         
         if (element is BpmnTask)
         {
             help.myType = ProcessInformationElement.ElementType.Task;
         }
         else if (element is BpmnExclusiveGateway)
         {
             help.myType = ProcessInformationElement.ElementType.ExclusiveGateway;
         }
         else if(element is BpmnStartEvent)
         {
             help.myType = ProcessInformationElement.ElementType.StartEvent;
         }
         
         foreach (var sf in element.outgoing)
         {
             help.decedents.Add(GenerateElementsFromTask(sf.target,sf.name));
         }

         

         return help;
    }
    
    private ProcessInformationElement GenerateElementsFromTask(BpmnElement element,string incomingSequenceFlowInfo)
    {
        ProcessInformationElement help = new ProcessInformationElement();
        help.decedents= new List<ProcessInformationElement>();
        help.nameRaw = element.nameRaw;
        help.nameCut = element.nameCut;
        help.inComingSequenceFlowInfo = incomingSequenceFlowInfo;
        if (element is BpmnTask)
        {
            help.myType = ProcessInformationElement.ElementType.Task;
            return help;
        }
        else if (element is BpmnExclusiveGateway)
        {
            help.myType = ProcessInformationElement.ElementType.ExclusiveGateway;
        }
        else if(element is BpmnEndEvent)
        {
            help.myType = ProcessInformationElement.ElementType.EndEvent;
            return help;
        }
         
        foreach (var sf in element.outgoing)
        {
            help.decedents.Add(GenerateElementsFromTask(sf.target,sf.name));
        }

        return help;
    }

    public void InitializeNextContainerPoints(int numberOfPoints)
    {
        nextContainers = new List<Transform>();
        int offset;
        if (numberOfPoints > 1)
            offset = 1;
        else
            offset = 0;
        
        //calculate the point in the top left corner from where the new start points should start
        Vector3 newPositionBase = startPoint.transform.position + hSpace / 2 * Vector3.left+ height/2* Vector3.up;//*numberOfPoints*offset*Vector3.up;
        for (int i = 0; i < numberOfPoints; i++)
        {
            //Spawn containers at the middle of the new heigth (heigth/numberofPoints). /2 calculates the middle
            Transform newContainerPoint = Instantiate(pointPrefab,
                //newPositionBase + (height / numberOfPoints * i * Vector3.down/2), startPoint.rotation,
                newPositionBase + (height / numberOfPoints * i * Vector3.down+(height/numberOfPoints)/2*Vector3.down), startPoint.rotation,
                startPoint).transform;
            nextContainers.Add(newContainerPoint);
        }
        
    }
    
    
}
