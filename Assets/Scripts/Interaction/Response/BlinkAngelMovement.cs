using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlinkAngelMovement : MonoBehaviour
{
    public InteractionMediator mediator;
    public GameObject trackedObject;
    public NavMeshAgent agent;

    public float speedTeleport = 5F;
    public float minOpenTimeForReset = 0.1F;
    public float jumpScareCooldownInSeconds = 60F;

    private bool firstActivation = false;
    private bool lastCapturedEyeState = false;
    private float openTime = 0F;
    private float jumpScareTime = 0F;

    public GameOverController GameOverController;

    void Start()
    {
        agent.isStopped = true;
        jumpScareTime = jumpScareCooldownInSeconds;
    }

    bool CanJumpScare()
    {
        return jumpScareTime >= jumpScareCooldownInSeconds;
    }

    void IncrementJumpScare()
    {
        if (CanJumpScare())
        {
            return;
        }

        jumpScareTime += Time.deltaTime;
    }

    void HandleFirstActivation()
    {
        Vector3 currentPosition = transform.position;
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(trackedObject.transform.position, currentPosition, NavMesh.AllAreas, path))
        {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                Debug.Log("Activating " + gameObject.name);
                firstActivation = true;
            }
        }
    }

    void Update()
    {
        IncrementJumpScare();

        if (!firstActivation)
        {
            HandleFirstActivation();
        } else
        {
            MoveAngel();
        }
    }
    private float CalculatePathDistance(Vector3[] points)
    {
        float totalDistance = 0f;
        for (int i = 0; i < points.Length - 1; i++)
        {
            totalDistance += Vector3.Distance(points[i], points[i + 1]);
        }
        return totalDistance;
    }

    private Vector3 FindPointAtFraction(Vector3[] points, float totalDistance, float fraction)
    {
        if (points.Length < 2)
        {
            Debug.LogWarning("Not enough points to define a path.");
            return Vector3.zero;
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

    bool AttemptJumpScare(float remainingDistance)
    {
        Vector3 currentPosition = transform.position;

        Vector3 direction = trackedObject.transform.position - currentPosition;
        RaycastHit hit;

        if (Physics.Raycast(currentPosition, direction, out hit))
        {
            bool parentIsTarget = hit.collider.gameObject.transform.parent.gameObject == trackedObject;
            bool targetMatch = parentIsTarget || hit.collider.gameObject == trackedObject;

            if (targetMatch)
            {
                Vector3 targetPosition = trackedObject.transform.position;
                Vector3 targetForward = trackedObject.transform.forward.normalized;
                Vector3 desiredPosition = targetPosition + remainingDistance * targetForward;

                NavMeshHit navHit;
                if (NavMesh.SamplePosition(desiredPosition, out navHit, 3F, NavMesh.AllAreas))
                {
                    NavMeshPath path = new NavMeshPath();
                    if (NavMesh.CalculatePath(navHit.position, currentPosition, NavMesh.AllAreas, path)) {
                        if (path.status == NavMeshPathStatus.PathComplete)
                        {
                            Debug.Log("Jumpscarin");
                            agent.transform.position = navHit.position;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    void MoveAngel()
    {
        agent.SetDestination(trackedObject.transform.position);

        if (mediator.EyesClosed() && !lastCapturedEyeState)
        {
            lastCapturedEyeState = true;

            float remainingDistance = CalculatePathDistance(agent.path.corners);

            if (CanJumpScare())
            {
                if (AttemptJumpScare(remainingDistance))
                {
                    jumpScareTime = 0F;
                    return;
                }
            }

            float percentageToMove = 1.0F;

            if (remainingDistance > speedTeleport)
            {
                // TODO: In the else case, we could trigger the jumpscare I guess?
                percentageToMove = speedTeleport / remainingDistance;
            }else{
                GameOverController.Activate();
            }

            Vector3 point = FindPointAtFraction(agent.path.corners, remainingDistance, percentageToMove);

            if (point == Vector3.zero)
            {
                agent.isStopped = true;
                return;
            }

            agent.transform.position = point;
            agent.isStopped = false;
        }
        else if (!mediator.EyesClosed())
        {
            agent.isStopped = true;
            if (lastCapturedEyeState)
            {
                openTime += Time.deltaTime;
                if (openTime > minOpenTimeForReset)
                {
                    lastCapturedEyeState = false;
                    openTime = 0F;
                }
            }
        }
    }
}
