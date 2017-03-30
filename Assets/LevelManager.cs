using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;

    public Transform Player;

    public Transform EndCameraTransform;

    public float MaxTimeSpeed;

    public bool Ending;

    public delegate void OnEndEvent();
    public event OnEndEvent OnEnd;

    private bool _continue;

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
    }

    private void Update()
    {
        if (Ending)
        {
            RecordManager.Instance.PlaybackSpeed = MaxTimeSpeed;

            if (Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.Return))
            {
                NextLevel();
            }
        }
    }

    public void EndLevel()
    {
        if (OnEnd != null)
        {
            OnEnd();
        }

        Ending = true;
        var cam = Camera.main.GetComponent<CameraMovement>();
        cam.Player = EndCameraTransform;
        cam.Height = 0;
        cam.SmoothFactor = 0.05f;
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
