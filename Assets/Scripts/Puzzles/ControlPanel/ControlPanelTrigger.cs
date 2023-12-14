using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelTrigger : MonoBehaviour
{
    public GameObject player;
    public VoiceControl vc;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            vc.Activate();
            Destroy(this.gameObject);
        }    
    }
}
