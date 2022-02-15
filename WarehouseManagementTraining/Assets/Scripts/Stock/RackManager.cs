using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the properties of a rack
/// </summary>
public class RackManager : MonoBehaviour
{
   public List<RackPlatePsoitionRoation> GetRackPlateStartPoints()
    {
        List<RackPlatePsoitionRoation> points = new List<RackPlatePsoitionRoation>();
        RackPlateStartPoint[] pointScripts = GetComponentsInChildren<RackPlateStartPoint>();
        foreach (var pointScript in pointScripts)
        {
            points.Add(pointScript.GetRackPlateStartPoint());
        }

        return points;
    }

    
}
