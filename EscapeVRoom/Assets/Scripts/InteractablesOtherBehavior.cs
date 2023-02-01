using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesOtherBehavior : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Time before the used object will reseted to inital transform")]
    [Range(0f, 5f)]
    private float resetBreak = 2f;

    private Vector3 InitialPosition;
    private Quaternion InitialRotation;
    private Rigidbody rigidbody;

    private void Awake()
    {
        OnResetAll.RegisterListener(ResetInteractable);
    }

    private void OnDestroy()
    {
        OnResetAll.UnregisterListener(ResetInteractable);
    }

    /// <summary>
    /// Speichern der Ausgangsposition.
    /// Entfernen des Gravitationseinflusses, damit Element keine ungewollten Bewegungen machen.
    /// </summary>
    void Start()
    {
        InitialPosition = gameObject.GetComponent<Transform>().position;
        InitialRotation = gameObject.GetComponent<Transform>().rotation;
        rigidbody = gameObject.GetComponent<Rigidbody>();
        StayAtPosition(true);
    }

    /// <summary>
    /// Toggeln des Gravitationseinflusses und Einfriehren der Position und Rotation
    /// </summary>
    /// <param name="_value"></param>
    private void StayAtPosition(bool _value)
    {
        rigidbody.useGravity = !_value;
        if (_value)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    /// <summary>
    /// Aufzurufen beim Selektieren des interaktiven Objekts
    /// </summary>
    public void DoInteraction()
    {
        StayAtPosition(false);
    }

    /// <summary>
    /// Starten einer Coroutine zum Zurücksetzen der Position des Objekts, beidem zunächst gewartet wird.
    /// </summary>
    public void ResetPositionWithWait()
    {
        StartCoroutine(WaitBeforeReset());
    }

    public IEnumerator WaitBeforeReset()
    {
        yield return new WaitForSeconds(resetBreak);
        ResetPosition();
    }

    /// <summary>
    /// Zurücksetzen des Objektes ohne vorheriges warten.
    /// </summary>
    public void ResetPosition()
    {
        StayAtPosition(true);
        rigidbody.velocity = new Vector3(0, 0, 0);
        rigidbody.angularVelocity = new Vector3(0, 0, 0);
        gameObject.transform.position = InitialPosition;
        gameObject.transform.rotation = InitialRotation;
    }

    /// <summary>
    /// Auch wenn eigentlich das Objekt bereits zurück gesetzt sein sollte, wird es sicherheitshalber beim Zurücksetzen des Raumes auch zurückgesetzt.
    /// </summary>
    /// <param name="reset">Reset Event. Muss übergeben werden, auch wenn keine Infromationen daraus genutzt werden.</param>
    private void ResetInteractable(OnResetAll reset)
    {
        ResetPosition();
    }
}
