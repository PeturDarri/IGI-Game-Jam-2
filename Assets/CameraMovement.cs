using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform Player;

    public float SmoothFactor;

    public float Height;

    public float MoveMultiplier;
	
	// Update is called once per frame
	private void FixedUpdate()
	{
	    UpdateCamera();
	}

    private void UpdateCamera()
    {
        var targetPos = new Vector3(Player.position.x, Player.position.y + Height, Player.position.z);

        targetPos.x += Input.GetAxis("Right Horizontal") * MoveMultiplier;
        targetPos.z += Input.GetAxis("Right Vertical") * MoveMultiplier;

        var newPos = transform.position;
        newPos += (targetPos - newPos) * SmoothFactor;
        transform.position = newPos;
    }
}
