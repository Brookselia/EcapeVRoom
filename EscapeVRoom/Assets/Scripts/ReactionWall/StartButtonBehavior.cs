using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartButtonBehavior : MonoBehaviour
{

    /// <summary>
    /// wenn der User mit einer Hand den StartButton berührt, feuert dieser das Event StartButtonHit.
    /// </summary>

    public UnityEvent StartButtonHit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            StartButtonHit.Invoke();
        }
    }
}
