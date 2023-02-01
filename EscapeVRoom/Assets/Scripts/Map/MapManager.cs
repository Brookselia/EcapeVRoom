using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : AbstractTask
{
    [SerializeField]
    [Tooltip("Count of all signs, that are on the map")]
    [Range(0, 30)]
    private int CountOfAllFlags = 16;
    private int AcutalCnt = 0;

    /// <summary>
    /// Globales Zur�cksetzen alles Aufgaben. 
    /// Die Anzahl der korrekt platzierten Flaggen wird zur�ckgesetzt.
    /// Die Aufgabe wird als nicht erledigt markiert.
    /// </summary>
    /// <param name="reset"></param>
    protected override void ResetTask(OnResetAll reset)
    {
        AcutalCnt = 0;
        IsFinished = false;
    }

    private void Awake()
    {
        OnFlagPlacement.RegisterListener(ProcessFlagPlacement);
        OnResetAll.RegisterListener(ResetTask);
    }

    private void OnDestroy()
    {
        OnFlagPlacement.UnregisterListener(ProcessFlagPlacement);
        OnResetAll.UnregisterListener(ResetTask);
    }

    /// <summary>
    /// Es wird keine Platzierung beachtet, wenn die Aufgabe als erledigt markiert ist.
    /// Die Antwort des platzierbaren Schildes wird bei dem Schild auf der Karte verarbeitet.
    /// Bei einer richtigen Antwort, wird die Anzahl der richtigen L�sungen hochgez�hlt.
    /// Sobald die Anzahl der richtigen L�sung mit der Anzahl aller Schilder �bereinstimmt,
    /// wird das entsprechende Event ausgel�st und die Aufgabe als erledigt markiert.
    /// </summary>
    /// <param name="_flag">Das Event, welches durch das Platzieren eines Schildes (urspr�nglich Flagge) ausgel�st wird.</param>
    private void ProcessFlagPlacement(OnFlagPlacement _flag)
    {
        if (IsFinished)
        {
            return;
        }

        if (_flag.MapFlag.GetComponent<MapFlagBehaviour>().SetAnswer(_flag.PlaceName))
        {
            AcutalCnt++;
            if (AcutalCnt == CountOfAllFlags)
            {
                new OnTaskCompleted("Topologie in Europa");
                IsFinished = true;
            }
        }
    }
}
