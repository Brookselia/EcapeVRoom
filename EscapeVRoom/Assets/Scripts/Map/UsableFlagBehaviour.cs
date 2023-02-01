using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableFlagBehaviour : MonoBehaviour
{
    [SerializeField]
    private string Answer = "N/A";

    private void OnCollisionEnter(Collision collision)
    {
        foreach (GameObject elem in GameObject.FindGameObjectsWithTag("MapFlag"))
        {
            if (collision.gameObject.Equals(elem) && (!elem.GetComponent<MapFlagBehaviour>().GetCorrecness()) && (!elem.GetComponent<MapFlagBehaviour>().GetProcessing()))
            {
                new OnFlagPlacement(Answer, elem);
                gameObject.GetComponent<InteractablesOtherBehavior>().ResetPosition();
                return;
            }
        }
    }
}
