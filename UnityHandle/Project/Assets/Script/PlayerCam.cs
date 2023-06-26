using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
	private Transform playerTransform;
	public float mouseSensitivity = 2f;
	private float cameraVerticalRotation = 0f;

	private void Start(){
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		playerTransform = transform.parent.gameObject.transform;
	}

	void Update(){
		float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
		float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

		cameraVerticalRotation -= inputY;
		cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -20f, 30f);
		transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

		playerTransform.Rotate(Vector3.up * inputX);
	}
}
