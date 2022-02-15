using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BpmnElementNameWrapper : MonoBehaviour
{
    public string elementName;
    public BpmnElementNameWrapper nextElement;

    public BpmnElementNameWrapper(string elementName)
    {
        this.elementName = elementName;
    }
}
