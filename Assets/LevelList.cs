using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelList : MonoBehaviour
{
    public static LevelList Instance;

    public List<string> Scenes;

    public int CurrentLevelIndex
    {
        get { return Scenes.FindIndex(s => s == SceneManager.GetActiveScene().name); }
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
    }
}
