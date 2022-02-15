using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RackPlateStartPoint : MonoBehaviour
{
   
    public RackPlatePsoitionRoation GetRackPlateStartPoint()
    {
        var transform1 = transform;
        return new RackPlatePsoitionRoation(transform1.position, transform1.parent.rotation, GetComponentInChildren<TextMeshPro>());
    }

}
