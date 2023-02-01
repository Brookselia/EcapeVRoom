using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WallManager : AbstractTask
{
    [SerializeField]
    private int NormalScore = 30;
    private int FinalScore;
    private int Score = 0;

    [SerializeField]
    private GameObject[] Lights;

    [SerializeField]
    private GameObject StartButton;



    /// <summary>
    /// Setting Final Score
    /// Enable Startbuttonbehavior
    /// Enabled das WallLightsbehavior für jedes Licht 
    /// Gibt den beiden Komponenten Listener für Events
    /// </summary>
    void Start()
    {
        FinalScore = NormalScore + Lights.Length;

        StartButton.GetComponent<StartButtonBehavior>().enabled = true;
        foreach (var light in Lights)
        {
            light.GetComponent<WallLightsBehavior>().enabled = true;
        }
        
        StartButton.GetComponent<StartButtonBehavior>().StartButtonHit.AddListener(HandleButtonHit);



        foreach (var light in Lights)
        {
            light.GetComponent<WallLightsBehavior>().LightTouched.AddListener(IncreaseScore);
        }
    }

    /// <summary>
    /// Es wird ein Random Licht ausgewählt, das aktiviert wird
    /// </summary>
    private void ActivateLight()
    {
        int lightIndex = Random.Range(0, Lights.Length);
        Lights[lightIndex].GetComponent<WallLightsBehavior>().Activate();
    }

    /// <summary>
    /// Alle Lichter werden gleichzeitig aktiviert
    /// </summary>
    private void ActivateAllLights()
    {
        foreach (var light in Lights)
        {
            light.GetComponent<WallLightsBehavior>().Activate();
        }
    }

    /// <summary>
    /// Das Spiel wird gestartet und das erste Licht wird aktiviert
    /// </summary>
    private void StartGame()
    {
        StartButton.GetComponent<StartButtonBehavior>().enabled = false;
        ActivateLight();
    }

    /// <summary>
    /// Der Score wird erhöht, falls er noch kleiner als der normalScore ist: nur ein Licht wird danach wieder aktiviert
    /// Falls er gleich dem normalScore ist: Alle Lichter sollen aktiviert werden
    /// Falls er größer/gleich dem finalScore ist, ist das Spiel gewonnen
    /// </summary>
    private void IncreaseScore()
    {
        Score++;
        if (Score < NormalScore)
        {
            ActivateLight();
        }
        if(Score == NormalScore)
        {
            ActivateAllLights();
        }
        if(Score >= FinalScore)
        {
            GameWon();
        }
    }

    private void HandleButtonHit()
    {
        if (!IsFinished)
        {
            StartGame();
        }
    }

    /// <summary>
    /// Spiel ist gewonnen, Event wird gefeuert an die EventKlasse
    /// </summary>
    private void GameWon()
    {
        IsFinished = true;
        new OnTaskCompleted("Reaction Wall");
    }

    protected override void ResetTask(OnResetAll reset)
    {
        IsFinished = false;
        Score = 0;
    }
}
