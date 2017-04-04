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

    public bool Paused = true;

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
    private float _startX;
    private float _prevPlayerX;

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
        _prevPlaybackSpeed = PlaybackSpeed;
        if (!GameManager.Instance.AllowBackwardsTime)
        {
            InvokeRepeating("TimelineLoop", 0, FixedTime / Mathf.Abs(PlaybackSpeed));
        }
    }

    private void Start()
    {
        LevelManager.Instance.OnEnd += OnEnd;
        _startX = Player.position.x;
        _prevPlayerX = _startX;
    }

    private void OnEnd()
    {
        Recordings.Add(_currentRecording);
        _ending = true;
    }

    private void FixedUpdate()
    {
        if (!Paused)
        {
            if (GameManager.Instance.AllowBackwardsTime)
            {
                /*var prevGlobalFrame = _globalFrame;
                _globalFrame = Mathf.Clamp((int) ((Player.position.x - _startX) * 10), 0, int.MaxValue);
                _localFrame += _globalFrame - prevGlobalFrame;
                PlaybackSpeed = (Player.position.x - _prevPlayerX) * 15;
                if (PlaybackSpeed >= 0)
                {
                    PlayState = PlaybackState.Playing;
                    PlaybackSpeed = Mathf.Clamp(PlaybackSpeed, 0.001f, 100);
                }
                else
                {
                    PlayState = PlaybackState.Reversed;
                    PlaybackSpeed = Mathf.Clamp(PlaybackSpeed, -100, -0.001f);
                }
                TimelineLoop();

                _prevPlayerX = Player.position.x;*/
            }
            else
            {
                if (!(Math.Abs(PlaybackSpeed - _prevPlaybackSpeed) > 0.01f)) return;

                CancelInvoke("TimelineLoop");
                _prevPlaybackSpeed = PlaybackSpeed;
                InvokeRepeating("TimelineLoop", 0, FixedTime / Mathf.Abs(PlaybackSpeed));
            }
        }
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

        if (_localFrame < 0 && PlayState == PlaybackState.Reversed)
        {
            if (Clones.Any(c => c.Index == _loops - 1))
            {
                if (!LevelManager.Instance.Ending)
                {

                    foreach (var clone in Clones.ToList())
                    {
                        clone.Index--;
                        if (clone.Index < 0)
                        {
                            Clones.RemoveAll(x => x.Index == clone.Index);
                            Destroy(clone.gameObject);
                        }
                    }
                }
                else
                {
                    if (_ending)
                    {
                        foreach (var clone in Clones.ToList())
                        {
                            clone.Index--;

                            if (clone.Index < 0)
                            {
                                Clones.RemoveAll(x => x.Index == clone.Index);
                                Destroy(clone.gameObject);
                            }
                        }
                        _ending = false;
                    }
                }

                if (OnResetTimeline != null)
                {
                    OnResetTimeline();
                }

                _loops--;
                TimelineStart();
                _localFrame = frames - 1;
            }
        }
        else if (_localFrame >= frames && PlayState == PlaybackState.Playing)
        {
            if (_globalFrame > 0)
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

        if (!LevelManager.Instance.Ending)
        {
            //Record
            var newRecording = new RecordFrame
            {
                Position = Player.position
            };
            try
            {
                _currentRecording[_localFrame] = newRecording;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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

                if (rec[_localFrame] != null)
                {
                    clone.transform.position = rec[_localFrame].Position;
                }
            }
        }

        if (!Paused && !GameManager.Instance.AllowBackwardsTime)
        {
            if (PlayState == PlaybackState.Reversed)
            {
                _localFrame--;
                _globalFrame--;

                if (_localFrame < -1 && _globalFrame >= 0)
                {
                    _localFrame = frames - 1;
                }
            }
            else
            {
                _localFrame++;
                _globalFrame++;

                if (_localFrame >= frames + 1)
                {
                    _localFrame = 0;
                }
            }
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

    public int GetGlobalFrame()
    {
        return _globalFrame;
    }

    public int GetTotalLoops()
    {
        return _loops;
    }

    public IEnumerator PauseRestart()
    {
        PlaybackSpeed = 0.001f;
        Paused = true;
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
    Playing = 0,
    Reversed = 1
}