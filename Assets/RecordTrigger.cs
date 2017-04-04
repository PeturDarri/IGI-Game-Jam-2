using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordTrigger : MonoBehaviour
{

    public bool SetToPlay;
    public bool OneTimeUse;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        RecordManager.Instance.Paused = !SetToPlay;

        if (OneTimeUse)
        {
            Destroy(gameObject);
        }
    }
}
