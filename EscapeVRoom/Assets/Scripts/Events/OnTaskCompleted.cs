using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTaskCompleted : EventCallbacks.Event<OnTaskCompleted>
{
    public readonly string TaskName;
    public OnTaskCompleted(string _taskName) : base("Event, that will be fired, when a task is completed.")
    {
        TaskName = _taskName;
        FireEvent(this);
    }
}
