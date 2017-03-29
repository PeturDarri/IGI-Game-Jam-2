using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordTrigger : MonoBehaviour
{

    public PlaybackState SetState;
    public bool OneTimeUse;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        RecordManager.Instance.PlayState = SetState;

        if (OneTimeUse)
        {
            Destroy(gameObject);
        }
    }
}
