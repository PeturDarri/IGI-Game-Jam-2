using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Transform Player;

    public Text Text;

    public Trigger LevelsTrigger;

    public Trigger ChallengeTrigger;

    public Trigger QuitTrigger;

    public float FadeSpeed;

    public string DefaultText;

    private float _textAlpha = 1;

    private MenuButtons _nextText = MenuButtons.Default;

    private bool _doFadeOut;

    private bool _buttonPressed;

    private void Update()
    {
        Text.color = new Color(1, 1, 1, _textAlpha);

        UpdateTriggers();
        UpdateFade();
        GetInput();
    }

    private void UpdateTriggers()
    {
        if (LevelsTrigger.Triggered)
        {
            SetText(MenuButtons.Levels);
        }
        else if (ChallengeTrigger.Triggered)
        {
            SetText(MenuButtons.Challenge);
        }
        else if (QuitTrigger.Triggered)
        {
            SetText(MenuButtons.Quit);
        }
        else
        {
            SetText(MenuButtons.Default);
        }
    }

    private void UpdateFade()
    {
        if (_doFadeOut)
        {
            if (_textAlpha > 0)
            {
                _textAlpha -= FadeSpeed;
            }
            else
            {
                _doFadeOut = false;
                Text.text = _nextText == MenuButtons.Default ? DefaultText : _nextText.ToString();
            }
        }
        else
        {
            if (_textAlpha < 1)
            {
                _textAlpha += FadeSpeed;
            }
        }
    }

    private void GetInput()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            if (_nextText != MenuButtons.Default)
            {
                _buttonPressed = true;
            }
        }
        else if (Input.GetButtonUp("Fire3"))
        {
            if (_buttonPressed)
            {
                ButtonPress(_nextText);
            }
        }
    }

    private void SetText(MenuButtons button)
    {
        if (_nextText != button)
        {
            _nextText = button;
            _doFadeOut = true;
            _buttonPressed = false;
        }
    }

    private void ButtonPress(MenuButtons button)
    {
        switch (button)
        {
            case MenuButtons.Quit:
                Application.Quit();
                break;
            case MenuButtons.Levels:
                LevelManager.Instance.NextLevel();
                break;
            case MenuButtons.Challenge:
                SceneManager.LoadScene("Challenge");
                break;
        }
    }
}

public enum MenuButtons
{
    Default,
    Levels,
    Challenge,
    Quit
}
