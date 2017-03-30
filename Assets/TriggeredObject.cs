using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggeredObject : MonoBehaviour
{
    public List<Trigger> Triggers;

    public bool Triggered;

    public virtual void Update()
    {
        CheckTriggers();
    }

    public virtual void CheckTriggers()
    {
        if (Triggers.Count > 0)
        {
            Triggered = Triggers.All(trigger => trigger.Triggered);
        }
    }
}
