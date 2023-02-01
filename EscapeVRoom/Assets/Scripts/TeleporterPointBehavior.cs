using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterPointBehavior : MonoBehaviour
{
    [SerializeField]
    private TimerManager Timer;

    /// <summary>
    /// Löst ein Event zum Starten des Timers aus, sollte dieser noch nicht laufen.
    /// </summary>
    public void StartTimer()
    {
        if(!Timer.GetRunning())
        {
            Timer.StartTimer();
        }
    }
}
