using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackwardsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        RecordManager.Instance.PlayState = PlaybackState.Reversed;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        RecordManager.Instance.PlayState = PlaybackState.Playing;
    }
}
