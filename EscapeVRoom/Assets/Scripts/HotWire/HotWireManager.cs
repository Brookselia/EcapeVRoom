using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [RequireComponent(typeof(AudioSource))]
public class HotWireManager : AbstractTask
{

    [SerializeField]
    private float WaWter = 0.5f;

    [SerializeField]
    private GameObject WinArea;

    [SerializeField]
    private GameObject Handle;

    private bool WireActive = true;

    [SerializeField]
    private AudioClip WinClip;
    [SerializeField]
    private AudioClip TouchClip;

    [SerializeField]
    [Range(0, 10)]
    private int PenaltyTime = 5;

    void Start()
    {
        WinArea.GetComponent<WinAreaPipeBehavior>().WireWon.AddListener(WinWire);
    }

    /// <summary>
    ///checkt zunächst, ob Draht aktiv ist. Falls ja, wird gecheckt, ob es eine Collision mit dem Handle gibt.  
    ///Der Draht wird für kurze Zeit (einstellbar) deaktiviert, da sonst zu viele Collisions auf einmal
    ///es werden 3 Sekunden bei der Gesamtzeit abgezogen
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (WireActive && !IsFinished)
        {
            if (collision.gameObject.CompareTag("Handle"))
            {
                WireActive = false;
                TouchWire();
                StartCoroutine(ShortWaiter(WaWter));
                WireActive = true;
            }
        }
    }

    /// <summary>
    /// Timer, bis Draht wieder aktiv sein soll
    /// </summary>
    private IEnumerator ShortWaiter(float waiter)
    {
        yield return new WaitForSeconds(waiter);
    }

    /// <summary>
    /// Sound wird gespielt, wenn Handle den Draht berührt
    /// </summary>
    private void TouchWire()
    {
        AudioSource.PlayClipAtPoint(TouchClip, transform.position);
        new OnPenatly(PenaltyTime);
    }

    /// <summary>
    /// Spiel gewonnen, CompletionSound wird abgespielt, wird an die EventKlasse weitergegeben
    /// </summary>
    private void WinWire()
    {
        if (!IsFinished)
        {
            WireActive = false;
            IsFinished = true;
            AudioSource.PlayClipAtPoint(WinClip, transform.position);
            new OnTaskCompleted("Der heiße Draht");
        }
    }

    protected override void ResetTask(OnResetAll reset)
    {
        IsFinished = false;
    }

}
