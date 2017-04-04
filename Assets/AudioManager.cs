using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public AudioManager Instance;

    private AudioSource _source;

    public float _currentPitch;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
    {
        if (GameObject.Find("AudioPlayer") != gameObject)
        {
            Destroy(gameObject);
        }
    }

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

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!_source)
        {
            _source = GetComponent<AudioSource>();
        }

        var targetPitch = RecordManager.Instance != null? RecordManager.Instance.PlaybackSpeed : 1;

        if (LevelManager.Instance.Ending)
        {
            targetPitch = 1;
        }

        _currentPitch += (targetPitch - _currentPitch) *
                         0.1f;

        _source.pitch = _currentPitch;
    }
}
