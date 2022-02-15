using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    
    private RobotCarrige myCarriage;

    public RobotCarrige MyCarriage => myCarriage;

    private bool robotMoves;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        myCarriage = GetComponentInChildren<RobotCarrige>();
        agent = GetComponent<NavMeshAgent>();
        
    }
    
    void Update()
    {
        //Locks the robots carriage, if it moves
        if (agent.velocity.sqrMagnitude > 0)
        {
            //check if the action has been performed before, so we do not change the rigidbody on every frame
            if (!robotMoves)
            {
                robotMoves = true;
                myCarriage.LockCarriage();
            }
        }
        else
        {
            if (robotMoves)
            {
                robotMoves = false;
                myCarriage.UnlockCarriage();
            }
        }
    }

    
        
}
