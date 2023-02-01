using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTimerStart : EventCallbacks.Event<OnTimerStart>
{
    public OnTimerStart() : base("Event, that will be fired, when the timer is suppost to be started.")
    {
        FireEvent(this);
    }
}
