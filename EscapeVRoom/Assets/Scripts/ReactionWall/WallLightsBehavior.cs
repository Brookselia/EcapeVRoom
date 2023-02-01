using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WallLightsBehavior : MonoBehaviour
{
    public UnityEvent LightTouched;

    private bool IsActive = false;
    private Color GlassColor;

    [SerializeField]
    private AudioClip Clip;
    [SerializeField]
    private Material Glass;


    /// <summary>
    /// Speichert die Farbe des Startmaterials der Lichter
    /// </summary>
    private void Start()
    {
        GlassColor = gameObject.GetComponent<Renderer>().material.color;
    }

    private void Awake()
    {
        OnTaskCompleted.RegisterListener(WinGame);
        OnResetAll.RegisterListener(ResetLight);
    }

    private void OnDestroy()
    {
        OnTaskCompleted.UnregisterListener(WinGame);
        OnResetAll.UnregisterListener(ResetLight);
    }

    /// <summary>
    /// Wenn es aktiviert werden soll, wird zunächst überprüft, ob es nicht schon aktiviert ist.
    /// Sollte dies nicht der Fall sein, wird das Licht auf die Farbe rot gesetzt.
    /// </summary>
    public void Activate()
    {
        if(!IsActive)
        {
            IsActive = true;
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    /// <summary>
    /// Wenn ein Licht aktiv ist und der User es mit der Hand berührt, soll es inaktiv werden, ein Erfolgssound wird eingespielt und das Event
    /// LightTouched wird gefeuert
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
       if(IsActive && other.gameObject.CompareTag("Hand"))
        {
            IsActive = false;
            LightToGlass();
            PlaySound();
            LightTouched.Invoke();
        }
    }

    /// <summary>
    /// wird ausgeführt wenn Anzahl an Lichtern getroffen wurde. Die Lichter werden grün, um das zu verdeutlichen
    /// </summary>
    public void WinGame(OnTaskCompleted _completedTask)
    {
        if (_completedTask.TaskName.Equals("Reaction Wall"))
        {
            IsActive = false;
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    /// <summary>
    /// Reset der entsprechenden Werte und Materialien bei einem Zurücksetzen des Raumes.
    /// </summary>
    /// <param name="_reset"></param>
    private void ResetLight(OnResetAll _reset)
    {
        LightToGlass();
        IsActive = false;
    }

    /// <summary>
    /// Zurücksetzen der Farbe zur Ausgangsfarbe
    /// </summary>
    private void LightToGlass()
    {
        gameObject.GetComponent<Renderer>().material.color = GlassColor;
    }

    /// <summary>
    /// wird abgespielt, wenn man ein aktives Licht getroffen hat
    /// </summary>
    private void PlaySound()
    {
        AudioSource.PlayClipAtPoint(Clip, transform.position);
    }

}
