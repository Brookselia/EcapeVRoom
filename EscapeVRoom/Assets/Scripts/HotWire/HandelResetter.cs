using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandelResetter : MonoBehaviour
{
    private Vector3 InitialPosition;
    private Quaternion InitialRotation;

    // Start is called before the first frame update
    void Start()
    {
        InitialPosition = gameObject.GetComponent<Transform>().position;
        InitialRotation = gameObject.GetComponent<Transform>().rotation;
    }

    private void Awake()
    {
        OnResetAll.RegisterListener(ResetHandel);
    }

    private void OnDestroy()
    {
        OnResetAll.UnregisterListener(ResetHandel);
    }

    private void ResetHandel(OnResetAll _reset)
    {
        gameObject.transform.position = InitialPosition;
        gameObject.transform.rotation = InitialRotation;
    }
}
