using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointOnPath
{
    private Vector3 position;
    private Vector3 previousPointOnPathPosition;
    private Vector3 nextPointOnPathPosition;
    private GameObject arrowObject;

    public GameObject ArrowObject
    {
        get => arrowObject;
        set => arrowObject = value;
    }

    public Vector3 Position
    {
        get => position;
        set => position = value;
    }

    public Vector3 PreviousPointOnPathPosition
    {
        get => previousPointOnPathPosition;
        set => previousPointOnPathPosition = value;
    }

    public Vector3 NextPointOnPathPosition
    {
        get => nextPointOnPathPosition;
        set => nextPointOnPathPosition = value;
    }
}
