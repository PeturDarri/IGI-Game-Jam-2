using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordManager : MonoBehaviour
{

    public static RecordManager Instance;

    public Transform Player;

    public List<Clone> Clones = new List<Clone>();

    public List<RecordFrame[]> Recordings = new List<RecordFrame[]>();

    [Tooltip("The length of each recording in seconds")]
    public float RecordingLength;

    public PlaybackState PlayState;

    public float PlaybackSpeed;

    public GameObject ClonePrefab;

    public delegate void OnResetTimelineEvent();
    public event OnResetTimelineEvent OnResetTimeline;

    private int _globalFrame;
    private int _localFrame;
    private int _loops;
    private RecordFrame[] _currentRecording;
    private float _prevPlaybackSpeed;
    private bool _ending;

    public const float FixedTime = 0.02f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        TimelineStart();
        InvokeRepeating("TimelineLoop", 0, FixedTime / PlaybackSpeed);
        _prevPlaybackSpeed = PlaybackSpeed;
    }

    private void Start()
    {
        LevelManager.Instance.OnEnd += OnEnd;
    }

    private void OnEnd()
    {
        Recordings.Add(_currentRecording);
        _ending = true;
    }

    private void TimelineStart()
    {
        var frames = (int)(RecordingLength / FixedTime);
        _localFrame = 0;

        _currentRecording = new RecordFrame[frames];
    }

    private void TimelineLoop()
    {
        Debug.Log(_localFrame);
        if (Math.Abs(PlaybackSpeed - _prevPlaybackSpeed) > 0.001f)
        {
            CancelInvoke("TimelineLoop");
            _prevPlaybackSpeed = PlaybackSpeed;
            InvokeRepeating("TimelineLoop", 0, FixedTime / PlaybackSpeed);
        }

        var frames = (int)(RecordingLength / FixedTime);

        if (PlayState != PlaybackState.Paused)
        {
            _localFrame++;
            _globalFrame++;
        }

        if (!LevelManager.Instance.Ending)
        {
            //Record
            var newRecording = new RecordFrame
            {
                Position = Player.position
            };

            _currentRecording[_localFrame] = newRecording;
        }

        //Play
        if (Recordings.Count > 0)
        {
            foreach (var clone in Clones.ToList())
            {
                if (!clone)
                {
                    Clones.RemoveAll(x => x.Index == clone.Index);
                    continue;
                }

                var rec = Recordings[clone.Index];

                clone.transform.position = rec[_localFrame].Position;
            }
        }

        if (_localFrame >= frames - 1)
        {
            if (!LevelManager.Instance.Ending)
            {
                Recordings.Add(_currentRecording);

                foreach (var clone in Clones.ToList())
                {
                    clone.Index++;
                }
            }
            else
            {
                if (_ending)
                {
                    foreach (var clone in Clones.ToList())
                    {
                        clone.Index++;
                    }
                    _ending = false;
                }
            }

            var newClone = CreateClone();
            newClone.Index = 0;
            Clones.Add(newClone);

            if (OnResetTimeline != null)
            {
                OnResetTimeline();
            }

            _loops++;
            TimelineStart();
        }
    }

    private Clone CreateClone()
    {
        var clone = Instantiate(ClonePrefab, transform);
        return clone.GetComponent<Clone>();
    }

    public float GetLocalTime()
    {
        return _localFrame * FixedTime;
    }

    public int GetLocalFrame()
    {
        return _localFrame;
    }

    public float GetGlobalTime()
    {
        return _globalFrame * FixedTime;
    }

    public float GetGlobalFrame()
    {
        return _globalFrame;
    }

    public IEnumerator PauseRestart()
    {
        PlayState = PlaybackState.Paused;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public class RecordFrame
{
    public Vector3 Position;
}

public enum PlaybackState
{
    Paused = 0,
    Playing = 1,
    Reversed = 2
}