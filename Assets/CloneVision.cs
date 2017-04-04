using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CloneVision : MonoBehaviour
{

    public Image AwarenessClock;
    public LayerMask Layers;
    public bool SeePlayer;
    [Tooltip("Ranges from 0.0 to 1.0")] public float AwarenessMeter;
    [Tooltip("How quickly AwarenessMeter goes up")] public float Awareness;

    private Vector3 prevPos;
    private bool shootRay;
    private Transform player;

    private void Update()
    {
        UpdateRotation();
        UpdateAwareness();
    }

    private void FixedUpdate()
    {
        if (!shootRay) return;
        FireRay();
    }

    private void OnGUI()
    {
        if (!RecordManager.Instance.Paused)
        {
            AwarenessClock.fillAmount = AwarenessMeter;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        shootRay = true;
        player = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shootRay = false;
            SeePlayer = false;
        }
    }

    private void UpdateRotation()
    {
        if (!(Vector3.Distance(transform.position, prevPos) > 0.1f)) return;

        var add = ((transform.position - prevPos) - transform.forward) * 0.8f;
        transform.forward += add;
        transform.eulerAngles = new Vector3(-90f, RecordManager.Instance.PlayState == PlaybackState.Playing ? transform.eulerAngles.y : -transform.eulerAngles.y, 0);

        prevPos = transform.position;
    }

    private void FireRay()
    {
        RaycastHit hit;
        var ray = new Ray(transform.position, player.position - transform.position);

        if (!Physics.Raycast(ray, out hit, 10, Layers)) return;
        if (hit.collider.CompareTag("Player"))
        {
            SeePlayer = true;
        }
        else
        {
            SeePlayer = false;
        }
    }

    private void UpdateAwareness()
    {
        if (SeePlayer)
        {
            //Multiply the amount awareness goes up depending on how close the player is
            if (AwarenessMeter < 1)
            {
                AwarenessMeter += (Awareness * (Mathf.Abs(Vector3.Distance(transform.position, player.position) - 11) / 3)) * RecordManager.Instance.PlaybackSpeed;
            }
            else
            {
                //Player seen, restart level
                if (SceneManager.GetActiveScene().name == "Challenge")
                {
                    LevelManager.Instance.EndLevel();
                    RecordManager.Instance.Paused = true;
                    RecordManager.Instance.PlaybackSpeed = 0.001f;
                }
                else
                {
                    StartCoroutine(RecordManager.Instance.PauseRestart());
                }
            }
        }
        else
        {
            if (AwarenessMeter > 0)
            {
                AwarenessMeter -= Awareness * RecordManager.Instance.PlaybackSpeed;
            }
            else
            {
                AwarenessMeter = 0;
            }
        }
    }
}
