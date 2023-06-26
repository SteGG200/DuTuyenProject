using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
	
	private GameObject TV;
	private float moveSpeed = 7f;
	private float mass = 1f;
	CharacterController controller;
	private float jumpSpeed = 7f;

	Vector3 velocity;

	// Start is called before the first frame update
	private void Start()
	{
		TV = GameObject.Find("TV");
		controller = GetComponent<CharacterController>();
	}

	void Gravity(){
		var gravity = Physics.gravity * mass * Time.deltaTime;
		velocity.y = controller.isGrounded ? -1f : velocity.y + gravity.y;
	}

	void Move(){
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		
		Vector3 input = new Vector3();
		input += transform.forward * vertical;
		input += transform.right * horizontal;
		input = Vector3.ClampMagnitude(input, 1f);

		if(Input.GetKey(KeyCode.Space) && controller.isGrounded){
			velocity.y += jumpSpeed;
		}

		controller.Move((input * moveSpeed + velocity) * Time.deltaTime);
	}

	void Update(){
		float dis = Vector3.Distance(transform.position, TV.transform.position);
		Debug.Log("Distance is : " + dis);
		Debug.Log(transform.localEulerAngles);
		Gravity();
		Move();
	}	
}
