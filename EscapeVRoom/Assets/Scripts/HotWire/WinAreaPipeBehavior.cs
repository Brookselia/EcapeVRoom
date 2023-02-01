using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinAreaPipeBehavior : MonoBehaviour
{
    /// <summary>
    /// wenn der User mit dem Handle die WinArea berührt, wird das Event WireWon gefeuert 
    /// </summary>

    public UnityEvent WireWon;

    public AudioClip clip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Handle"))
        {
            WireWon.Invoke();
        }
    }
}
