using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackwardsLevel : MonoBehaviour
{

    public BackwardsPressurePlate Button;

    public Image ButtonClockImage;

    private bool _prevTriggered;

    private float _timer;

    private float _targetTime;

    private void Start()
    {
        InvokeRepeating("IncrementTimer", 0, RecordManager.FixedTime);
    }

    private void Update()
    {
        if (Button.Triggered && !_prevTriggered)
        {
            _prevTriggered = true;
            _targetTime = RecordManager.Instance.RecordingLength / RecordManager.FixedTime;
        }
        else if (_prevTriggered)
        {
            Button.Disabled = false;
            Button.Triggered = true;

            if (_timer >= _targetTime)
            {
                Button.Triggered = false;
                _prevTriggered = false;
            }
        }

        if (_prevTriggered)
        {
            ButtonClockImage.fillAmount = Mathf.InverseLerp(0, _targetTime, _timer);
        }
        else
        {
            ButtonClockImage.fillAmount = 0;
        }
    }

    private void IncrementTimer()
    {
        if (_prevTriggered)
        {
            _timer += 1 * Mathf.Abs(RecordManager.Instance.PlaybackSpeed);
        }
        else
        {
            _timer = 0;
        }
    }
}
