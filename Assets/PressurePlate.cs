using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Trigger
{

    public Color TriggeredColor;

    public Color UntriggeredColor;

    private Renderer _renderer;

    public override void Update()
    {
        base.Update();

        if (!_renderer)
        {
            _renderer = GetComponent<Renderer>();
        }

        _renderer.material.SetColor("_EmissionColor", Triggered ? TriggeredColor : UntriggeredColor);
    }
}
