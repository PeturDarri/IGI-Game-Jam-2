using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredDoor : TriggeredObject
{
    public Transform Left;
    public Transform Right;

    public float Width;
    public float OpenAmount;
    public float OpenSpeed;
    public float OpenOffset;

    public override void Update()
    {
        base.Update();

        if (Triggered)
        {
            if (OpenAmount < 1)
            {
                OpenAmount += OpenSpeed * RecordManager.Instance.PlaybackSpeed;
            }
        }
        else
        {
            if (OpenAmount > 0)
            {
                OpenAmount -= OpenSpeed * RecordManager.Instance.PlaybackSpeed;
            }
        }

        var amount = Mathf.Lerp(Width / 4, (Width * (Width / 4)) + OpenOffset, OpenAmount);
        Left.localPosition = new Vector3(-amount, Left.localPosition.y, Left.localPosition.z);
        Right.localPosition = new Vector3(amount, Right.localPosition.y, Right.localPosition.z);
    }
}
