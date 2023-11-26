using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAngelMovement : MonoBehaviour
{
    public InteractionMediator mediator;
    public GameObject trackedObject;
    public float speed;

    void Start()
    {
        mediator.OnEyesStatusChanged += OnEyesChanged;
    }

    void OnDestroy()
    {
        mediator.OnEyesStatusChanged -= OnEyesChanged;
    }

    void OnEyesChanged(bool closed)
    {
        if (closed)
        {
            transform.position = Vector3.MoveTowards(transform.position, trackedObject.transform.position, speed);
        }
    }
}
