using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackwardsPressurePlate : PressurePlate
{

    public override void Update()
    {
        base.Update();
        Disabled = RecordManager.Instance.PlayState == PlaybackState.Playing;

        if (Disabled)
        {
            Triggered = false;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Clone")) return;

        if (RecordManager.Instance.PlayState == PlaybackState.Reversed)
        {
            Triggered = !Inverse;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Clone")) return;
        if (RecordManager.Instance.PlayState == PlaybackState.Reversed)
        {
            Triggered = Inverse;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Clone")) return;
        if (RecordManager.Instance.PlayState == PlaybackState.Reversed && !Disabled)
        {
            Triggered = !Inverse;
        }
    }
}
