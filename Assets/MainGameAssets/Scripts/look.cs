using System;
using UnityEngine;

public class look : MonoBehaviour {

	float rotUpDown = 0f;
	float rotLeftRight = 0f;

	public float constantSpeed;
	public float mouseSensitivity = 5.0f;
	public float upDownRange = 60.0f;
	public Camera playerCamera;

    private bool isAI = true; // Set to false when there is an oculus player.

	void Update () {
        if (isAI) {
            // Move this whale all around, take care of dark spot when close to surface.

        } else {            
		    rotLeftRight = Input.GetAxis ("Mouse X") * mouseSensitivity;
		    transform.Rotate (0, rotLeftRight, 0);

		    rotUpDown -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		    rotUpDown = Mathf.Clamp (rotUpDown, -upDownRange, upDownRange);
		    playerCamera.transform.localRotation = Quaternion.Euler (rotUpDown, 0, 0);


		    Vector3 speed = new Vector3 (constantSpeed,0,0);
		    speed = transform.rotation * speed;

		    transform.position += transform.forward * Time.deltaTime * constantSpeed;
        }
	}
}