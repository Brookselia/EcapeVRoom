using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnResetAll : EventCallbacks.Event<OnResetAll>
{
    public OnResetAll() : base("Event, that will be fired, when all tasks will be reseted.")
    {
        FireEvent(this);
    }
}
