using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    public GameObject renderPlane;
    public InteractionMediator mediator;
    public float speed = 10F;

    private Material renderPlaneMaterial;
    private float rotation = 0F;

    public void Start()
    {
        renderPlaneMaterial = renderPlane.GetComponent<MeshRenderer>().material;
    }

    public void Update()
    {
        float newRotation = rotation + speed * (-1 * mediator.FaceDirectionalWeight()) * Time.deltaTime;
        rotation = Mathf.Clamp(newRotation, -90, 90);

        renderPlaneMaterial.SetFloat("_Rotation", rotation);
    }

    public float GetRotation()
    {
        return rotation;
    }
}
