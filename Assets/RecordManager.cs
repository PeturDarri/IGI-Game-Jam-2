using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

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

    private float prevTime;
    private int _globalFrame;
    private int _localFrame;
    private int _loops;
    private RecordFrame[] _currentRecording;

    private const float FixedTime = 0.02f;

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
        InvokeRepeating("TimelineLoop", 0, FixedTime);
    }

    private void TimelineStart()
    {
        var frames = (int)(RecordingLength / FixedTime);
        _localFrame = 0;

        _currentRecording = new RecordFrame[frames];
    }

    private void TimelineLoop()
    {
        var frames = (int)(RecordingLength / FixedTime);

        if (PlayState != PlaybackState.Paused)
        {
            _localFrame++;
            _globalFrame++;
        }

        //Record
        var newRecording = new RecordFrame
        {
            Position = Player.position
        };

        _currentRecording[_localFrame] = newRecording;

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
            Recordings.Add(_currentRecording);

            var newClone = CreateClone();
            newClone.Index = -1;
            Clones.Add(newClone);

            foreach (var clone in Clones.ToList())
            {
                clone.Index++;
            }

            if (OnResetTimeline != null)
            {
                OnResetTimeline();
            }

            _loops++;
            Debug.Log("Took " + (Time.realtimeSinceStartup - prevTime) + " seconds");
            prevTime = Time.realtimeSinceStartup;
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