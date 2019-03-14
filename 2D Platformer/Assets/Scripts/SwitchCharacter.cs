using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour {
	public bool wizard;
	public bool theif;
	GameObject Theif;
	GameObject Wizard;
	public bool grounded;
	// Use this for initialization
	void Start () {
		wizard = false;
		theif = true;
		Theif = GameObject.Find ("Theif");
		Wizard = GameObject.Find ("Wizard");
		Theif.SetActive (true);
		Wizard.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (GetComponentInChildren<GroundCheck> ().ground == true) {
			grounded = true;
		} else {
			grounded = false;
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			if (theif == true) {
				Theif.GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Innactive;
				theif = false;
				wizard = true;
				Wizard.transform.position = Theif.transform.position;
				Wizard.SetActive (true);
			} else {
				Debug.Log (Theif.GetComponent<Rigidbody2D> ().transform.position);
				theif = true;
				wizard = false;
				Theif.transform.position = Wizard.transform.position;
				Theif.SetActive (true);
				Theif.GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Basic;
				Wizard.SetActive (false);
			}
		}

		if (theif == true) {
			Camera.main.GetComponent<lockCamera> ().theif = true;
		//	transform.position = Theif.transform.position;
		} else {
			Camera.main.GetComponent<lockCamera> ().theif = false;
		//	transform.position = Wizard.transform.position;
		}
	}
}
