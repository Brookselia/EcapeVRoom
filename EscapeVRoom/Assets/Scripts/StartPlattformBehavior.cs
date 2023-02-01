using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlattformBehavior : MonoBehaviour
{
    [SerializeField]
    private TimerManager Timer;

    /// <summary>
    /// Setzt alles abgesehen von der Bestzeit zurück, wenn der Raum abgeschlossen wurde.
    /// </summary>
    public void ResetRoom()
    {
        if (Timer.GetResetable())
        {
            new OnResetAll();
        }
    }
}
