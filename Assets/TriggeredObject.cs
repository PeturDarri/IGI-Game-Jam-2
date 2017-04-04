using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class TriggeredObject : MonoBehaviour
{
    public List<Trigger> Triggers;

    public bool Triggered;

    private List<VectorLine> _lines;

    private void Start()
    {
        _lines = new List<VectorLine>();
        foreach (var unused in Triggers)
        {
            var line = new VectorLine("line", new List<Vector3>(), 2f);
            _lines.Add(line);
        }
    }

    public virtual void Update()
    {
        CheckTriggers();
    }

    public virtual void CheckTriggers()
    {
        if (Triggers.Count > 0)
        {
            var all = true;
            for (var i = 0; i < Triggers.Count; i++)
            {
                var t = Triggers[i];
                if (!t.Triggered)
                {
                    _lines[i].active = false;
                    all = false;
                }
                else
                {
                    if (t.DrawLines)
                    {
                        _lines[i].color = GameManager.Instance.LineColor;
                        _lines[i].texture = GameManager.Instance.LineTexture;
                        _lines[i].textureScale = _lines[i].GetLength() * 10;
                        _lines[i].textureOffset = Mathf.Lerp(-20, 20, GameManager.Instance.LinePulse);
                        _lines[i].active = true;
                        _lines[i].points3 = new List<Vector3> {transform.position, t.transform.position};
                        _lines[i].Draw();
                    }
                }
            }

            Triggered = all;
        }
    }
}
