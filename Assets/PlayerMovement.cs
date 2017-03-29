using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // Normal Movements Variables
    public float WalkSpeed;

    public float SprintSpeed;

    void FixedUpdate()
    {
        var rigid = GetComponent<Rigidbody>();
        var speed = Input.GetButton("Fire3") ? SprintSpeed : WalkSpeed;

        rigid.velocity = new Vector3(Mathf.Lerp(0, Input.GetAxisRaw("Horizontal") * speed, 0.8f),
            rigid.velocity.y, Mathf.Lerp(0, Input.GetAxisRaw("Vertical") * speed, 0.8f));
    }
}
