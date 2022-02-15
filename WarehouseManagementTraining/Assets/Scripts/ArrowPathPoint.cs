using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPathPoint
{
    private Vector3 position;

    private float distanceToStart;
    private float distanceToLastPoint;

    public float DistanceToLastPoint
    {
        get => distanceToLastPoint;
        set => distanceToLastPoint = value;
    }

    public Vector3 Position
    {
        get => position;
        set => position = value;
    }

    public float DistanceToStart
    {
        get => distanceToStart;
        set => distanceToStart = value;
    }
}
