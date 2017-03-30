﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public TriggerType TriggerType;
    public bool Inverse;
    public bool Triggered;

    private bool _oneTimeUsed;
    private float _timeToReset;

    public virtual void Update()
    {
        if (_oneTimeUsed && RecordManager.Instance.GetGlobalTime() >= _timeToReset)
        {
            Debug.Log("Reset trigger");
            Triggered = false;
            _oneTimeUsed = false;
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Clone")) return;

        if (TriggerType == TriggerType.OneTime)
        {
            if (!_oneTimeUsed)
            {
                _oneTimeUsed = true;
                _timeToReset = RecordManager.Instance.GetGlobalTime() + RecordManager.Instance.RecordingLength;
                Triggered = !Inverse;
            }
        }
        else
        {
            Triggered = !Inverse;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (TriggerType == TriggerType.OneTime) return;

        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            Triggered = Inverse;
        }
    }
}

public enum TriggerType
{
    Stay,
    OneTime
}