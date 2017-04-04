using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeManipulation : MonoBehaviour
{

    public float SlowTime;

    public float FastTime;

    private void Update()
    {
        if (!GameManager.Instance.MovingTime)
        {
            if (Input.GetButton("Time Backwards"))
            {
                RecordManager.Instance.PlaybackSpeed = SlowTime;
            }
            else if (Input.GetButton("Time Forward"))
            {
                RecordManager.Instance.PlaybackSpeed = FastTime;
                RecordManager.Instance.PlayState = PlaybackState.Playing;
            }
            else
            {
                RecordManager.Instance.PlaybackSpeed = 1;
                RecordManager.Instance.PlayState = PlaybackState.Playing;
            }
        }
    }
}
