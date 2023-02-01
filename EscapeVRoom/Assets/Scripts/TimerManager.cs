using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField]
    [Range(1,10)]
    private int MaxCountTask = 4;
    private int FinishedTasks = 0;
    private bool IsRunning = false;
    private bool CanBeReseted = false;

    [SerializeField]
    [Tooltip("Timer in seconds")]
    [Range(300f,900f)]
    private float ActualTime = 600f;
    private float BestTime = 0f;
    private float RemainingTime = 0f;

    [SerializeField]
    private TextMesh ActualTimeText;
    [SerializeField]
    private TextMesh BestTimeText;

    private void Awake()
    {
        OnTaskCompleted.RegisterListener(CountFinishedTasks);
        OnResetAll.RegisterListener(ResetTimer);
        OnPenatly.RegisterListener(ReduceRemainingTime);
    }

    private void OnDestroy()
    {
        OnTaskCompleted.UnregisterListener(CountFinishedTasks);
        OnResetAll.UnregisterListener(ResetTimer);
        OnPenatly.UnregisterListener(ReduceRemainingTime);
    }

    public bool GetResetable()
    {
        return CanBeReseted;
    }

    public bool GetRunning()
    {
        return IsRunning;
    }

    private void Start()
    {
        RemainingTime = ActualTime;
        SetTextForTimer(true, ActualTime);
    }

    private void Update()
    {
        if(IsRunning && !CanBeReseted)
        {
            if(RemainingTime > 0)
            {
                RemainingTime -= Time.deltaTime;
                SetTextForTimer(true, RemainingTime);
            }
            else
            {
                StopTimer();
            }
        }
    }

    /// <summary>
    /// Der Timer wird gestartet und als laufend markiert.
    /// </summary>
    public void StartTimer()
    {
        IsRunning = true;
    }

    /// <summary>
    /// Die Anzahl der erledigten Aufgaben wird gezählt. Wenn alle Aufgaben erledigt sind, wird der Timer gestoppt.
    /// </summary>
    /// <param name="_completedTask"></param>
    private void CountFinishedTasks(OnTaskCompleted _completedTask)
    {
        FinishedTasks++;
        if(FinishedTasks == MaxCountTask)
        {
            StopTimer();
        }
    }

    /// <summary>
    /// Beenden des Timers. Hat der Timer eine höhere Zahl als der beste Timer, wird der beste Timer neu gesetzt.
    /// </summary>
    private void StopTimer()
    {
        CanBeReseted = true;
        IsRunning = false;
        SetTextForTimer(true, RemainingTime);
        if (RemainingTime > BestTime)
        {
            BestTime = RemainingTime;
            SetTextForTimer(false, BestTime);
        }
        RemainingTime = 0;
    }

    /// <summary>
    /// Zurücksetzen des Timers durch das Zurücksetzen des Raums
    /// </summary>
    /// <param name="reset"></param>
    private void ResetTimer(OnResetAll _reset)
    {
        CanBeReseted = false;
        //should allready be set to false, but just in case here again
        IsRunning = false;
        RemainingTime = ActualTime;
        SetTextForTimer(true, ActualTime);
    }

    /// <summary>
    /// Rechnet die Zeit als float in Minuten und Sekunden um und ändert den Text beim entsprechenden Timer in 
    /// einen der Zeit entsprechenden String.
    /// </summary>
    /// <param name="_useActualTimer">Ob der aktuelle Timer aktualisiert werden soll oder die Bestzeit.</param>
    /// <param name="_time">Die Zeit als float, die dafür genutzt werden soll.</param>
    private void SetTextForTimer(bool _useActualTimer, float _time)
    {
        float minutes = Mathf.FloorToInt(_time / 60);
        float seconds = Mathf.FloorToInt(_time % 60);

        string cleanedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (_useActualTimer)
        {
            ActualTimeText.text = cleanedTime;
        }
        else
        {
            BestTimeText.text = cleanedTime;
        }
    }


    public void ReduceRemainingTime(OnPenatly _penalty)
    {
        RemainingTime -= _penalty.PenaltyTimeInSeconds;
    }
}
