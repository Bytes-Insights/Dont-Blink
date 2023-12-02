using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAngelMovement : MonoBehaviour
{
    public InteractionMediator mediator;
    public GameObject trackedObject;
    public float speed;
    public float minOpenTimeForReset = 0.1F;
    public NavMeshAgent agent;

    private bool lastCapturedEyeState = false;
    private float openTime = 0F;

    void MoveAngel()
    {
        /*
        Vector3 currPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 trackedPos = new Vector3(trackedObject.transform.position.x, 0, trackedObject.transform.position.z);
        Vector3 result = Vector3.MoveTowards(currPos, trackedPos, speed);
        transform.position = new Vector3(result.x, transform.position.y, result.z);
        */

    }

    void Start()
    {
        agent.SetDestination(trackedObject.transform.position);
    }

    void Update()
    {
        //agent.SetDestination(trackedObject.transform.position);
        if (mediator.EyesClosed() && !lastCapturedEyeState)
        {
            lastCapturedEyeState = true;
            //agent.SetDestination(trackedObject.transform.position);
            //agent.isStopped = false;
        } else if (!mediator.EyesClosed() && lastCapturedEyeState)
        {
            openTime += Time.deltaTime;
            if (openTime > minOpenTimeForReset)
            {
                lastCapturedEyeState = false;
                openTime = 0F;
                //agent.isStopped = true;
            }
        }
    }
}
