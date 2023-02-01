using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulcanoMaterialBehavior : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        foreach(GameObject elem in GameObject.FindGameObjectsWithTag("Vulcano"))
        {
            if (collision.gameObject.Equals(elem) && !elem.GetComponent<VulcanoPartBehavior>().GetCorrectness())
            {
                new OnVulcanoMaterialCollision(gameObject.name);
                gameObject.GetComponent<InteractablesOtherBehavior>().ResetPosition();
                Debug.Log("OnVulcanoMaterialCollision");
                return;
            }
        }
    }
}
