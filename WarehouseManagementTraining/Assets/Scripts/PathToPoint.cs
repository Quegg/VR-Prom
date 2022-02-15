using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Valve.VR.InteractionSystem;

public class PathToPoint : MonoBehaviour
{
    //public Transform start;

    public Transform end;


    private List<ArrowPointOnPath> arrows;

    private Vector3 positionStart;

    private Vector3 positionEnd;

    private Transform playerTransform;

    public float distanceBetweenArrows;
    private float pathLength;
    public GameObject arrowPrefab;
    private bool changed;

    public bool showCorners;
    // Start is called before the first frame update
    private NavMeshPath path;
    private float elapsed = 0.0f;

    private List<ArrowPointOnPath> arrowsOnPath;
    private List<GameObject> pathObjects;
    private List<ArrowPathPoint> pointsOnPath;
    private List<Animator> animators;

    private bool active;

    public bool Active => active;

    private bool first;

    public bool useOtherObjectAsStart;
    public Transform startTransform;


    void Start()
    {
        arrows=new List<ArrowPointOnPath>();
        pointsOnPath=new List<ArrowPathPoint>();
        path = new NavMeshPath();
        elapsed = 0.0f;
        
        positionEnd = end.position;
        
        changed = true;
        pathObjects=new List<GameObject>();
    }

    void Update()
    {
        if (active)
        {
            // Recalculate the arrows every half second
            elapsed += Time.deltaTime;
            if (elapsed > 0.5f)
            {
                elapsed -= 0.5f;

                //dont need to recalculate, if nothing changed
                if (positionEnd != end.position)
                {
                    positionEnd = end.position;
                    changed = true;
                }

                // Check, if the player is the starting point or any other object
                if(!useOtherObjectAsStart)
                {
                    if (positionStart != playerTransform.position)
                    {
                        positionStart = playerTransform.position;
                        changed = true;
                    }
                }
                else
                {
                    if (positionStart != startTransform.position)
                    {
                        positionStart = startTransform.position;
                        changed = true;
                    }
                }

                
                if (changed||first)
                {
                    first = false;
                    //calculate the path on the NavMesh
                    if (!useOtherObjectAsStart)
                    {
                        NavMesh.CalculatePath(playerTransform.position, end.position, NavMesh.AllAreas, path);
                    }
                    else
                    {
                        NavMesh.CalculatePath(startTransform.position, end.position, NavMesh.AllAreas, path);
                    }
                    
                    //if there is a new path
                    if (path.corners.Length != 0) 
                    {
                        //destroy the old arrows
                        foreach (var arrow in pathObjects)
                        {
                            Destroy(arrow);
                        }

                        pathObjects.Clear();

                        pointsOnPath = new List<ArrowPathPoint>();

                        pathLength = 0;
                        //calculate hthe length of the wohle path
                        for (int i = 0; i < path.corners.Length; i++)
                        {
                            float distance;
                            if (i == 0)
                            {
                                distance = 0;
                            }
                            else
                            {
                                distance = Vector3.Distance(path.corners[i - 1], path.corners[i]);
                            }


                            pathLength += distance;
                            pointsOnPath.Add(new ArrowPathPoint
                            {
                                Position = path.corners[i], DistanceToLastPoint = distance, DistanceToStart = pathLength
                            });


                            //DEBUG! Show Spheres at the corners of the path
                            if (showCorners)
                            {
                                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                                sphere.transform.position = path.corners[i];
                                sphere.transform.localScale = Vector3.one * 0.5f;
                                pathObjects.Add(sphere);
                            }
                        }

                        //spawn new arrows according to the defined distance. Consider the path going around corners
                        ArrowPathPoint[] rawPathPoints = pointsOnPath.ToArray();

                        int index = 0;
                        Vector3 lastPoint = rawPathPoints[0].Position;
                        bool isFirst = true;
                        arrows = new List<ArrowPointOnPath>();
                        float coveredDistance = 0;
                        while (coveredDistance <= pathLength - distanceBetweenArrows)
                        {
                            ArrowPointOnPath point = new ArrowPointOnPath();

                            float distanceToNextPoint = distanceBetweenArrows;

                            //if the next arrow is on a straight line to the last, use this last arrow as calculation point for the direction.
                            //when we cross an edge, use the position of the edge to determine the direction
                            //set index, so that the next arrow is in the right interval
                            while (coveredDistance + distanceBetweenArrows > rawPathPoints[index + 1].DistanceToStart)
                            {
                                distanceToNextPoint -= Vector3.Distance(rawPathPoints[index + 1].Position, lastPoint);
                                index++;
                                lastPoint = rawPathPoints[index].Position;
                            }


                            Vector3 direction = (rawPathPoints[index + 1].Position - lastPoint).normalized;
                            Vector3 position = lastPoint + direction * distanceToNextPoint;

                            point.Position = position;
                            lastPoint = position;



                            coveredDistance += distanceBetweenArrows;
                            GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity, transform);
                            pathObjects.Add(arrow);
                            arrow.transform.LookAt(rawPathPoints[index + 1].Position);
                        }
                    }

                    changed = false;
                }
            }
        }

    }
    
    

    [ContextMenu("StartAnimation")]
    public void ShowPath()
    {
        if(!useOtherObjectAsStart)
        {
            if (playerTransform is null)
            {
                playerTransform=FindObjectOfType<Player>().transform;
            }
            positionStart = playerTransform.position;
        }
        else
        {
            positionStart = startTransform.position;
        }
        active = true;
        first = true;
    }
    
    
    
    [ContextMenu("EndAnimation")]
    public void HidePath()
    {
        foreach (var o in pathObjects)
        {
            Destroy(o);
        }
        pathObjects.Clear();
        active = false;
    }
    
}