using System.Collections;
using UnityEngine;

public class TrapLevelScript : MonoBehaviour
{

    public TriggeredDoor ClosingWall;

    public ButtonTrigger Button;

    public float MinOpen;

    private bool _continue;

    private void Start()
    {
        Button.OnButtonActivate += OnButtonActivate;
        RecordManager.Instance.OnResetTimeline += OnResetTimeline;
    }

    private void Update()
    {
        //Restart level if the closing wall ever closes completely
        if (ClosingWall.OpenAmount < MinOpen && !RecordManager.Instance.Paused)
        {
            if (Button.Triggered)
            {
                ClosingWall.OpenAmount = MinOpen;
            }
            else
            {
                Camera.main.GetComponent<CameraMovement>().Player = ClosingWall.transform;
                StartCoroutine(RecordManager.Instance.PauseRestart());
            }
        }
    }

    private void OnResetTimeline()
    {
        _continue = true;
    }

    private void OnButtonActivate()
    {
        StartCoroutine(ButtonActionSequence());
    }

    private IEnumerator ButtonActionSequence()
    {
        ClosingWall.enabled = false;

        var curTime = RecordManager.Instance.GetGlobalFrame();
        while (curTime + (2 / RecordManager.FixedTime) > RecordManager.Instance.GetGlobalFrame())
        {
            yield return 0;
        }

        ClosingWall.enabled = true;
        var trigger = ClosingWall.Triggers[0];
        trigger.Triggered = true;
        trigger.enabled = false;

        while (!_continue)
        {
            yield return 0;
        }

        trigger.enabled = true;
        trigger.Triggered = true;
    }
}
