using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFlagPlacement : EventCallbacks.Event<OnFlagPlacement>
{
    public readonly string PlaceName;
    public readonly GameObject MapFlag;
    public OnFlagPlacement(string _placeName, GameObject _mapFlag) : base("Event, that will be fired, when a task is completed.")
    {
        PlaceName = _placeName;
        MapFlag = _mapFlag;
        FireEvent(this);
    }
}
