using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTask : MonoBehaviour
{
    protected bool IsFinished = false;

    protected abstract void ResetTask(OnResetAll reset);
}
