using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    public float CurrentTime;

    public bool RecordTime;

    public Text TimerText;

    public Text BestText;

    public Color WorseColor;

    public Color BetterColor;

    private float _best;

    private void Start()
    {
        LevelManager.Instance.OnEnd += OnEnd;
        
        _best = PlayerPrefs.GetFloat("chBest");

        var minutes = Mathf.FloorToInt(_best / 60);
        var seconds = Mathf.FloorToInt(_best - minutes * 60);
        var milli =  Mathf.FloorToInt((float)((_best - Math.Truncate(_best)) * 100f));
        var bestString = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milli);

        BestText.text = "Best: " + bestString;
        InvokeRepeating("IncrementTimer", 0, RecordManager.FixedTime);
    }

    private void OnEnd()
    {
        if (CurrentTime > _best)
        {
            PlayerPrefs.SetFloat("chBest", CurrentTime);
        }
    }

    private void Update()
    {
        RecordTime = !RecordManager.Instance.Paused;
    }

    private void IncrementTimer()
    {
        if (RecordTime)
        {
            CurrentTime += RecordManager.FixedTime * RecordManager.Instance.PlaybackSpeed;
        }
    }

    private void OnGUI()
    {
        var minutes = Mathf.FloorToInt(CurrentTime / 60);
        var seconds = Mathf.FloorToInt(CurrentTime - minutes * 60);
        var milli =  Mathf.FloorToInt((float)((CurrentTime - Math.Truncate(CurrentTime)) * 100f));
        var niceTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milli);

        TimerText.text = niceTime;

        if (CurrentTime > _best)
        {
            TimerText.color = BetterColor;
        }
        else
        {
            TimerText.color = WorseColor;
        }
    }
}
