using UnityEngine;
using UnityEngine.UI;

public class ButtonTrigger : Trigger
{

    public Image ClockImage;

    public int TriggerFrame;

    public Color UnclickedColor;

    public Color ClickedColor;

    public Color ClickedHoverColor;

    public delegate void OnButtonActivateEvent();
    public event OnButtonActivateEvent OnButtonActivate;

    private bool _finishedLoop;

    private void Start()
    {
        SetColor(Triggered ? ClickedColor : UnclickedColor);
        RecordManager.Instance.OnResetTimeline += OnResetTimeline;
    }

    private void OnResetTimeline()
    {
        _finishedLoop = false;
    }

    public override void Update()
    {
        ClockImage.fillAmount = Mathf.InverseLerp(0, RecordManager.Instance.RecordingLength,
            RecordManager.Instance.GetLocalTime());
        if (_finishedLoop || !Triggered || RecordManager.Instance.GetLocalFrame() < TriggerFrame) return;
        Triggered = false;
        _finishedLoop = true;
        SetColor(UnclickedColor);
        if (OnButtonActivate != null) OnButtonActivate();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Clone")) return;

        Triggered = true;
        SetColor(ClickedHoverColor);
    }

    public override void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Clone")) return;

        SetColor(ClickedColor);
    }

    private void SetColor(Color color)
    {
        GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }
}
