using System;
using System.Collections;
using System.Collections.Generic;
using Guiding.LoggingEvents;
using PMLogging;
using UnityEngine;

public class RobotCarrige : MonoBehaviour
{
    private List<GameObject> allObjectsInCarriage;

    public List<GameObject> AllObjectsInCarriage => allObjectsInCarriage;

    private GuidingController guidingController;
    // Start is called before the first frame update
    void Start()
    {
        guidingController=FindObjectOfType<GuidingController>();
        allObjectsInCarriage = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            
            allObjectsInCarriage.Add(other.gameObject);
            guidingController.UserEvent(new PlaceItemInRobot(other.GetComponent<SmallItem>().GetName(),DateTime.Now, XesLifecycleTransition.complete));
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            GameObject o;
            (o = other.gameObject).transform.SetParent(null);
            allObjectsInCarriage.Remove(o);
            //logger.WriteEvent(new RemoveItemFromRobot(other.GetComponent<SmallItem>().GetName(),DateTime.Now, XesLifecycleTransition.complete));
        }
    }


    public void LockCarriage()
    {
        
        foreach (var go in allObjectsInCarriage)
        {
            go.transform.SetParent(this.gameObject.transform);
        }
    }

    public void UnlockCarriage()
    {
        foreach (var go in allObjectsInCarriage)
        {
            go.transform.SetParent(null);
        }
    }
}
