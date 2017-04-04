using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public Color LineColor;

    public Texture LineTexture;

    public float LinePulseSpeed;

    public float LinePulse;

    public bool MovingTime;

    public bool AllowBackwardsTime;

    public float SecondsToMenu = 2;

    private float _menuTimer = float.NaN;

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
        if (LinePulse < 1)
        {
            LinePulse += LinePulseSpeed * RecordManager.Instance.PlaybackSpeed;
        }
        else
        {
            LinePulse = 0;
        }

        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (float.IsNaN(_menuTimer))
        {
            _menuTimer = Input.GetButton("Menu") ? Time.fixedTime : float.NaN;
        }
        else if (Time.fixedTime > _menuTimer + SecondsToMenu)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    private void FixedUpdate()
    {
        if (!MovingTime || LevelManager.Instance.Ending) return;
        if (AllowBackwardsTime) return;
        var speed = RecordManager.Instance.Player.GetComponent<Rigidbody>()
                        .velocity.magnitude / 3f;
        var playSpeed = Mathf.Clamp(Mathf.Abs(Mathf.Round(speed * 100) / 100), 0.0001f, 100);
        RecordManager.Instance.PlaybackSpeed = RecordManager.Instance.PlayState == PlaybackState.Playing ? playSpeed : -playSpeed;

        if (Input.GetButton("Time Forward"))
        {
            RecordManager.Instance.PlaybackSpeed = RecordManager.Instance.PlayState == PlaybackState.Playing ? 2 : -2;
        }
    }
}
