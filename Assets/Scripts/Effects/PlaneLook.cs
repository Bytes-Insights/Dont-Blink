using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneLook : MonoBehaviour
{
	public Transform player;

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - player.position);
    }
}
