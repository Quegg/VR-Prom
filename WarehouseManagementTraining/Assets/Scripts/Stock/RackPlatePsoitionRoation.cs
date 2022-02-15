using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using TMPro;
using UnityEngine;

/// <summary>
/// Every rack has 3 Rack plates. This class holds information about a single plate
/// </summary>
public class RackPlatePsoitionRoation
{

    private Vector3 position;
    private Quaternion rotation;
    private TextMeshPro nameTMP;

    public RackPlatePsoitionRoation(Vector3 pos, Quaternion rot, TextMeshPro label)
    {
        position = pos;
        rotation = rot;
        nameTMP = label;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public Quaternion GetRoation()
    {
        return rotation;
    }

    public void SetName(string nameText)
    {
        nameTMP.SetText(nameText);
    }

    
}
