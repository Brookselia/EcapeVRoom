using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class LightsBehavior : MonoBehaviour
{
    [SerializeField]
    private string TaskName;
    [SerializeField]
    private Material LitMat;
    [SerializeField]
    private Material UnlitMat;

    [SerializeField]
    private AudioClip LitSound;

    private void Awake()
    {
        OnTaskCompleted.RegisterListener(MarkTask);
        OnResetAll.RegisterListener(ResetLights);
    }

    private void OnDestroy()
    {
        OnTaskCompleted.UnregisterListener(MarkTask);
        OnResetAll.UnregisterListener(ResetLights);
    }

    /// <summary>
    /// Ändert das Material zu einer beleuchtet wirkenden Lampe, wenn die Aufgabe mit der gleichen Bezeichnung als erledigt markiert wird.
    /// </summary>
    /// <param name="completedTask"></param>
    private void MarkTask(OnTaskCompleted completedTask)
    {
        if (completedTask.TaskName.Equals(this.TaskName))
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.sharedMaterial = LitMat;
            AudioSource.PlayClipAtPoint(LitSound, transform.position);

        }
    }

    private void ResetLights(OnResetAll reset)
    {
         Renderer renderer = gameObject.GetComponent<Renderer>();
         renderer.sharedMaterial = UnlitMat;
    }
}
