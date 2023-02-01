using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFlagBehaviour : MonoBehaviour
{
    [SerializeField]
    private string CorrectAnswer = "N/A";
    [SerializeField]
    private TextMesh TextField;
    [SerializeField]
    [Tooltip("Amount of time until the sign will reset, if the answer was incorrect")]
    [Range(.5f, 5f)]
    private float flagResetWaitingTimeInSeconds = 1.5f;

    [Header("Materialien")]
    [SerializeField]
    private Material SignMaterial;
    [SerializeField]
    private Material PreviewMaterial;

    //Markiert Flagge als richtig, verhindert, dass richtige Flagge geändert werden kann
    private bool IsCorrect = false;
    //Markiert Flagge als in Bearbeitung, soll verhindern, dass, während eine Verarbeitung stattfindet, das Schild erneut geändert werden kann
    private bool IsProcessing = false;

    private Renderer renderer;

    private void Awake()
    {
        OnResetAll.RegisterListener(ResetFlag);
    }

    private void OnDestroy()
    {
        OnResetAll.UnregisterListener(ResetFlag);
    }

    public bool GetCorrecness()
    {
        return IsCorrect;
    }

    public bool GetProcessing()
    {
        return IsProcessing;
    }

    private void Start()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Wenn das Schild noch nicht richtig beantwortet wurde und aktuell nicht bearbeitet wird, wird der eigentliche 
    /// Zweck der Methode abgearbeitet.
    /// Zunächst wird das Schild als in Bearbeitung (IsProcessing) markiert, das Schild bekommt ein nicht transparentes 
    /// Material und die Antwort, die für das Schild gegeben wurde, wird dort angezeigt.
    /// Dann wird überprüft, ob die Antwort richtig ist. Ist dies der Fall, wird das Schild als richtig beantwortet markiert
    /// und der Flag, dass das Schild bearbeitet wird, wird zurück gesetzt. Ist die Antwort falsch wird eine Coroutine zum
    /// Entfernen der Antwort gestartet.
    /// </summary>
    /// <param name="_answer">Die Antwort, die für das Schild angegeben wird.</param>
    /// <returns>Gibt zurück, ob das Schild richtig oder falsch beantwortet wurde.</returns>
    public bool SetAnswer(string _answer)
    {
        if (!IsCorrect && !IsProcessing)
        {
            IsProcessing = true;

            renderer.sharedMaterial = SignMaterial;
            TextField.text = _answer;

            if (_answer.Equals(CorrectAnswer))
            {
                IsCorrect = true;
                IsProcessing = false;
            }
            else
            {
                StartCoroutine(ResetSign());
            }
        }
        return IsCorrect;
    }

    /// <summary>
    /// Coroutine zum Entfernen einer falschen Antwort mit einem im Editor einstellbaren Zeit der Anzeige der falschen Antwort.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetSign()
    {
        yield return new WaitForSeconds(flagResetWaitingTimeInSeconds);
        DoReset();
    }

    /// <summary>
    /// Das Ausführen des Schildresets.
    /// Das Material wird wieder auf das teiltransparente Vorschaumaterial gesetzt und der Text wird entfernt. Da das Schild nun 
    /// wieder im Ausgangszustand ist, wird der Bearbeitungsflag wieder entfernt.
    /// </summary>
    private void DoReset()
    {
        renderer.sharedMaterial = PreviewMaterial;
        TextField.text = " ";
        IsProcessing = false;
    }

    /// <summary>
    /// Zurücksetzen der Flagge nach einem globalen Reset. Dabei wird zunächst der gleiche Reset wie bei einer falschen Antwort genutzt.
    /// Für den Fall, dass die Flagge bereits richtig beantwortet wurde, wird dies auch zurück gesetzt.
    /// </summary>
    /// <param name="reset"></param>
    private void ResetFlag(OnResetAll reset)
    {
        DoReset();
        IsCorrect = false;
    }
}
