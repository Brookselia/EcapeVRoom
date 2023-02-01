using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulcanoPartBehavior : MonoBehaviour
{
    private bool IsCorrect = false;

    private void Awake()
    {
        OnResetAll.RegisterListener(ResetPart);
    }

    private void OnDestroy()
    {
        OnResetAll.UnregisterListener(ResetPart);
    }

    private void ResetPart(OnResetAll reset)
    {
        IsCorrect = false;
    }

    public void SetCorrect()
    {
        IsCorrect = true;
    }

    public bool GetCorrectness()
    {
        return IsCorrect;
    }
}
