using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // Normal Movements Variables
    public float WalkSpeed;

    public float SprintSpeed;

    public Transform Light;

    private Vector3 prevPos;

    private void LateUpdate()
    {
        UpdateRotation();
    }

    void FixedUpdate()
    {
        var rigid = GetComponent<Rigidbody>();
        var speed = Input.GetButton("Fire3") ? SprintSpeed : WalkSpeed;

        rigid.velocity = new Vector3(Mathf.Lerp(0, Input.GetAxisRaw("Horizontal") * speed, 0.8f),
            rigid.velocity.y, Mathf.Lerp(0, Input.GetAxisRaw("Vertical") * speed, 0.8f));
    }

    private void UpdateRotation()
    {
        if (Light == null) return;
        if (!(Vector3.Distance(Light.transform.position, prevPos) > 0.1f)) return;

        var add = ((Light.transform.position - prevPos) - Light.transform.forward) * 0.8f;
        Light.transform.forward += add;
        Light.transform.eulerAngles = new Vector3(0, RecordManager.Instance.PlayState == PlaybackState.Playing ? Light.transform.eulerAngles.y : -Light.transform.eulerAngles.y, 0);

        prevPos = Light.transform.position;
    }
}
