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
                if (SceneManager.GetActiveScene().name == "Challenge")
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                else
                {
                    NextLevel();
                }
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

    public void NextLevel()
    {
        var nextIndex = LevelList.Instance.CurrentLevelIndex + 1;
        Debug.Log(nextIndex);

        if (nextIndex >= LevelList.Instance.Scenes.Count)
        {
            nextIndex = 0;
        }

        SceneManager.LoadScene(LevelList.Instance.Scenes[nextIndex]);
    }
}
