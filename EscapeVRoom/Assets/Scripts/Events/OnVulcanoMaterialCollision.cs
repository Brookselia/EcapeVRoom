using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnVulcanoMaterialCollision : EventCallbacks.Event<OnVulcanoMaterialCollision>
{
    public readonly string ObjectName;
    public OnVulcanoMaterialCollision(string _objectName) : base("Event, that will be fired, when a vulcano material object collieds with a vulcano part.")
    {
        ObjectName = _objectName;
        FireEvent(this);
    }
}
