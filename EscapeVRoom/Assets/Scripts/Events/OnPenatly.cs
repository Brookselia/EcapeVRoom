using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPenatly : EventCallbacks.Event<OnPenatly>
{
    public readonly int PenaltyTimeInSeconds;
    public OnPenatly(int _penaltyTimeInSeconds) : base("Event, that will be fired if a mistake is made")
    {
        this.PenaltyTimeInSeconds = _penaltyTimeInSeconds;
        FireEvent(this);
    }
}
