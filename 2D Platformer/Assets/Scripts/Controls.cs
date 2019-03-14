using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (PlayerController))]
public class Controls : MonoBehaviour {

	// Use this for initialization
	public class Platformer2DUserControl : MonoBehaviour
	{
		private PlayerController character;
		private bool jump;


		private void Awake()
		{
			character = GetComponent<PlayerController>();
		}


		private void Update()
		{
			if (!jump)
			{
				// Read the jump input in Update so button presses aren't missed.
				jump = Input.GetButtonDown("Jump");
			}
		}


		private void FixedUpdate()
		{
			// Read the inputs.
			//bool crouch = Input.GetKey(KeyCode.LeftControl);
			float h = Input.GetAxis("Horizontal");
			// Pass all parameters to the character control script.
			//character.Move(h, jump);
			jump = false;
		}
	}
}
