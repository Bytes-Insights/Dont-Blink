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
    public bool moveOnNotSeen;
    public bool crawl;
    public float angelViewDistance;
    public float wanderRadius;

    private bool lastCapturedEyeState = false;
    private float openTime = 0F;
    private bool chasingMode = true;
    private Vector3 targetPosition;
    private float distanceThreshold = 3.0f;
    private GameObject[] rooms;
    private Plane[] cameraFrustum;

    void Start()
    {
        //Original target position = self
        targetPosition = transform.position;
        rooms = GameObject.FindGameObjectsWithTag("Room");
        if(crawl)
            chasingMode = false;
    }

    void Update()
    {
        MoveAngel();
    }

    //Returns true if angel can clearly see the player in the given distance and without obstacles in between
    bool seePlayer()
    {
        RaycastHit objectHit;
        Ray ray = new Ray(transform.position, trackedObject.transform.position - transform.position);
        //casting a ray against the player
        if (Physics.Raycast(ray, out objectHit, angelViewDistance))
        {
            if(objectHit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    //Returns true if player can see the angel in the camera
    bool playerSeesAngel()
    {
        //See if angel is inside cameraViewport
        Bounds bounds = GetComponent<BoxCollider>().bounds;
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
        {
            RaycastHit objectHit;
            Ray ray = new Ray(trackedObject.transform.position, transform.position - trackedObject.transform.position);
            //If so, cast a ray against the angel to see if there's an obstacle
            if (Physics.Raycast(ray, out objectHit))
            {
                if(objectHit.collider.gameObject.CompareTag("Angel"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    //Returns next room position to be checked by angel
    Vector3 getNextTargetRoom()
    {
        return rooms[Random.Range(0, rooms.Length)].transform.position;
    }

    void Wander()
    {   
        //If the angel has reached the room, get a new random room and go check it
        if(Vector3.Distance(targetPosition, transform.position) < distanceThreshold)
        {
            //Selects a random target point in the maze and updates it if the angel reached it
            targetPosition = getNextTargetRoom();
            agent.SetDestination(targetPosition);
        }

        // TODO: This can be more complex. Angel should be able to detect sounds. Maybe the angel should wander to a random position close to the player
    }

    void MoveOnBlink()
    {
        if (mediator.EyesClosed() && !lastCapturedEyeState)
        {
            lastCapturedEyeState = true;
            Vector3 point = FindPointAtFraction(agent.path.corners, 0.25F);
            agent.transform.position = point;

        } else if (!mediator.EyesClosed())
        {
            if(lastCapturedEyeState){
                openTime += Time.deltaTime;
                    if (openTime > minOpenTimeForReset)
                    {
                        lastCapturedEyeState = false;
                        openTime = 0F;
                    }
            }
        }
    }

    private Vector3 FindPointAtFraction(Vector3[] points, float fraction)
    {
        if (points.Length < 2)
        {
            Debug.LogError("Not enough points to define a path.");
            return Vector3.zero;
        }

        float totalDistance = 0f;
        for (int i = 0; i < points.Length - 1; i++)
        {
            totalDistance += Vector3.Distance(points[i], points[i + 1]);
        }

        float targetDistance = totalDistance * fraction;
        float distanceCovered = 0f;

        for (int i = 0; i < points.Length - 1; i++)
        {
            float distanceBetween = Vector3.Distance(points[i], points[i + 1]);
            if (distanceCovered + distanceBetween >= targetDistance)
            {
                float remainingDistance = targetDistance - distanceCovered;
                float fractionOfSegment = remainingDistance / distanceBetween;
                return Vector3.Lerp(points[i], points[i + 1], fractionOfSegment);
            }
            distanceCovered += distanceBetween;
        }

        return points[points.Length - 1];
    }

    void MoveAngel()
    {
        //Angel enters chasing mode as soon as player is seen
        if(seePlayer())
            chasingMode = true;

        //Wanders in the maze or travels towards the player depending on chasingMode. 
        if(chasingMode)
            agent.SetDestination(trackedObject.transform.position);
        else 
            Wander();
        
        if(moveOnNotSeen && !playerSeesAngel())
        {
            agent.isStopped = false;
        }else
        {
            MoveOnBlink();
        }
    }
}
