using UnityEngine;

public class EndTrigger : Trigger {

    public override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Clone")) return;

        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
        }
        else
        {
            Destroy(other.gameObject);
        }

        LevelManager.Instance.EndLevel();
    }
}
