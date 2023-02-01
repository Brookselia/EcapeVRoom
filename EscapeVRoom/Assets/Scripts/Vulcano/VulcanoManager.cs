using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulcanoManager : AbstractTask
{
    [Header("Objects and Materials")]
    [SerializeField]
    [Tooltip("Order in which the vulcano has to be build")]
    private GameObject[] vulcanoModel;
    [SerializeField]
    [Tooltip("Needs the same order as vulcanoModel")]
    private GameObject[] materialObjects;
    [SerializeField]
    [Tooltip("Materials which differ from materialObjects to vulcanoModel in order of vulcanoModel")]
    private Material[] materials;
    [SerializeField]
    private Material previewMaterial;

    [Header("Tweaking")]
    [SerializeField]
    [Tooltip("Amount of time until someone can place a vulcano material again. In secounds")]
    [Range(.5f, 5f)]
    private float materialWaitingTimeInSeconds = 2f;

    private int currentModelIndex = 0;

    private Renderer renderer;
    private float nextPossibleMatching;

    /// <summary>
    /// Listener hinzufügen
    /// </summary>
    private void Awake()
    {
        OnVulcanoMaterialCollision.RegisterListener(ProcessMaterialUse);
        OnResetAll.RegisterListener(ResetTask);
    }

    /// <summary>
    /// Der Renderer wird initial auf den ersten Vulkan-Part gesetzt.
    /// Der erste Zeitpunkt für das Betrachten eines Vulkan-Material-Events wird gesetzt.
    /// </summary>
    void Start()
    {
        renderer = vulcanoModel[0].GetComponent<MeshRenderer>();
        nextPossibleMatching = materialWaitingTimeInSeconds;
    }

    /// <summary>
    /// Das Event wird nur verarbeitet, wenn die Aufgabe noch nicht erledigt ist und der Cooldown für die Verarbeitung abgelaufen ist. 
    /// Der Cooldown ist im Editor einstellbar und soll verhindern, dass das Material zu häufig geändert wird und bei korrekter Antwort 
    /// der neue Teil direkt das alte Material zugeordnet bekommt.
    /// 
    /// Im Anschluss wird überprüft, ob die Antwort korrekt ist. Sollte dies der Fall sein, wird das korrekte Material gesetzt, der Part 
    /// als richtig markiert, damit es das Event, welches das Verarbeiten eines Materials auslöst, nicht mehr erzeugen kann, und der 
    /// nächste Teil des Vulkans wird aktiviert. Ist das angewandte Material falsch, wird das falsche Material gesetzt, aber nichts 
    /// weiteres passiert.
    /// 
    /// Als letztes wird der Zeitpunkt, wann der Cooldown beendet ist, festgelegt.
    /// </summary>
    /// <param name="matCollision"></param>
    private void ProcessMaterialUse(OnVulcanoMaterialCollision matCollision)
    {
        if (IsFinished || (Time.time < nextPossibleMatching))
        {
            return;
        }

        if(materialObjects[currentModelIndex].name.Equals(matCollision.ObjectName))
        {
            //material setzen, object als richtig markieren, zu nächsten Vulkanteil wechseln
            SetCorrectMaterial(matCollision.ObjectName);
            vulcanoModel[currentModelIndex].GetComponent<VulcanoPartBehavior>().SetCorrect();
            ToNextVulcanoPart();
        } 
        else
        {
            SetIncorrectMaterial(matCollision.ObjectName);
        }

        //Timer setzen, bevor eine erneute Collision beachtet wird
        nextPossibleMatching = Time.time + materialWaitingTimeInSeconds;
    }

    /// <summary>
    /// Abgesehen von den Teilen "Explosion" und "Gängen" wird das richtige Material aus der Liste einfach angewandt.
    /// Bei den eben genannten Teilen muss das Material der Kindobjekte gesetzt werden. Die Explosion hat dabei 2 
    /// Materialien, die ganz am Ende der Materialliste sind und von dort genutzt werden.
    /// </summary>
    /// <param name="materialObjectName"></param>
    private void SetCorrectMaterial(string materialObjectName)
    {
        if (materialObjectName.Equals("ExplosionMaterial"))
        {
            foreach (Transform child in vulcanoModel[currentModelIndex].GetComponentInChildren<Transform>())
            {
                renderer = child.gameObject.GetComponent<MeshRenderer>();
                if (child.gameObject.name.Contains("Wolke"))
                {
                    renderer.sharedMaterial = materials[8];
                }
                else
                {
                    renderer.sharedMaterial = materials[9];
                }
            }
        }
        else if (materialObjectName.Equals("Gaenge"))
        {
            foreach (Transform child in vulcanoModel[currentModelIndex].GetComponentInChildren<Transform>())
            {
                renderer = child.gameObject.GetComponent<MeshRenderer>();
                renderer.sharedMaterial = materials[currentModelIndex];
            }
        }
        else
        {
            renderer.sharedMaterial = materials[currentModelIndex];
        }
    }

    /// <summary>
    /// Entsprechend der Bezeichnung des falschen Materials wird ein Material für den aktuellen Bestandteil genutzt.
    /// </summary>
    /// <param name="materialObjectName"></param>
    private void SetIncorrectMaterial(string materialObjectName)
    {
        Material matToUse = previewMaterial;
        switch (materialObjectName)
        {
            case "ObererErdmantelMaterial":
                matToUse = materials[0];
                break;
            case "MagmaKammerMaterial":
                matToUse = materials[1];
                break;
            case "ErdkrusteMaterial":
                matToUse = materials[2];
                break;
            case "LavaschichtenMaterial":
                matToUse = materials[3];
                break;
            case "Gaenge":
                matToUse = materials[4];
                break;
            case "ExplosionMaterial":
                matToUse = materials[8];
                break;
            case "FontaeneMaterial":
                matToUse = materials[6];
                break;
            case "LavaflussMaterial":
                matToUse = materials[7];
                break;
            default:
                Debug.LogError("Unknown Material Source: " + materialObjectName);
                break;
        }

        if (vulcanoModel[currentModelIndex].name.Equals("Explosion") || vulcanoModel[currentModelIndex].name.Equals("Lavagaenge"))
        {
            foreach (Transform child in vulcanoModel[currentModelIndex].GetComponentInChildren<Transform>())
            {
                renderer = child.gameObject.GetComponent<MeshRenderer>();
                renderer.sharedMaterial = matToUse;
            }
        } 
        else
        {
            renderer.sharedMaterial = matToUse;
        }
    }

    /// <summary>
    /// Index des aktuell zu betrachtenden Objekts wird erhöht. Sollte damit die Aufgabe abgeschlossen sein, 
    /// wird dies markiert und das entsprechende Event wird gefeuert. Ansonsten wird der nächste Teil des 
    /// Vulkans in der Hierachry aktiviert und der Renderer auf das nächsete Element gesetzt.
    /// 
    /// In manchen Fällen hat der nächste Part keinen Renderer. Dieser ist auf den Kindern. Dies wird beim
    /// Verarbeiten der Materialien bedacht.
    /// </summary>
    private void ToNextVulcanoPart()
    {
        currentModelIndex++;

        if (currentModelIndex == vulcanoModel.Length)
        {
            new OnTaskCompleted("Aufbau eines Vulkan");
            IsFinished = true;
        }
        else
        {

            vulcanoModel[currentModelIndex].SetActive(true);
            try
            {
                renderer = vulcanoModel[currentModelIndex].GetComponent<MeshRenderer>();
            }
            catch (MissingComponentException _exception)
            {
                renderer = null;
            }
        }
    }

    /// <summary>
    /// Listener entfernen
    /// </summary>
    private void OnDestroy()
    {
        OnVulcanoMaterialCollision.UnregisterListener(ProcessMaterialUse);
        OnResetAll.UnregisterListener(ResetTask);
    }

    /// <summary>
    /// Alle Modellbestandteile des Vulkans, die einen Renderer haben, müssen ihr Material wieder zu dem Vorschaumaterial zurück gesetzt bekommen.
    /// Im Anschluss werden alle Bestandteile (Parts) abegesehen vom ersten in der Hierarchie deaktiviert. 
    /// Zudem muss die Aufgabe wieder als nicht erledigt gesetzt werden und der Wert für den aktuell zubearbeitenden Part auf 0 gesetzt werden.
    /// Wie in Start() muss der Renderer auf den ersten Part des Vulkans gesetzt werden.
    /// </summary>
    /// <param name="reset"></param>
    protected override void ResetTask(OnResetAll reset)
    {
        foreach(GameObject part in vulcanoModel)
        {
            if (part.name.Equals("Explosion") || part.name.Equals("Lavagaenge"))
            {
                foreach (Transform child in vulcanoModel[currentModelIndex].GetComponentInChildren<Transform>())
                {
                    renderer = child.gameObject.GetComponent<MeshRenderer>();
                    renderer.sharedMaterial = previewMaterial;
                }
            } 
            else
            {
                renderer = part.gameObject.GetComponent<MeshRenderer>();
                renderer.sharedMaterial = previewMaterial;
            }
        }

        for(int i = vulcanoModel.Length - 1; i > 0; i--)
        {
            vulcanoModel[i].SetActive(false);
        }

        IsFinished = false;
        currentModelIndex = 0;
        renderer = vulcanoModel[0].GetComponent<MeshRenderer>();
    }
}
