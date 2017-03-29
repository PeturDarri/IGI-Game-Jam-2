using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    public Image ClockImage;

    private bool inverse;

    private void Start()
    {
        RecordManager.Instance.OnResetTimeline += OnResetTimeline;
    }

    private void OnGUI()
    {
        var time = RecordManager.Instance.GetLocalTime();
        ClockImage.fillAmount = inverse ? Mathf.InverseLerp(RecordManager.Instance.RecordingLength, 0, time) : Mathf.InverseLerp(0, RecordManager.Instance.RecordingLength, time);
    }

    private void OnResetTimeline()
    {
        if (inverse)
        {
            inverse = false;
            ClockImage.fillClockwise = true;
        }
        else
        {
            inverse = true;
            ClockImage.fillClockwise = false;
        }
    }
}
