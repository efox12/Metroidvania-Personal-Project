using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheifStateMachine : MonoBehaviour {
	public enum theifState{Innactive, Basic, WallJump, Grapple, ClimbLadder, Hide};
	public int currentState;
	public bool grounded;
	// Use this for initialization
	void Start () {
		currentState = (int)theifState.Basic;
	}
	
	// Update is called once per frame
	void Update () {
		// go to the current state
		if (currentState == (int)theifState.Basic) {
			BasicState ();
		} else if (currentState == (int)theifState.WallJump) {
			WallJumpState ();
		} else if (currentState == (int)theifState.Grapple) {
			GrappleState ();
		} else if (currentState == (int)theifState.ClimbLadder) {
			ClimbLadderState ();
		} else if (currentState == (int)theifState.Hide) {
			HideState ();
		} else if (currentState == (int)theifState.Innactive) {
			InnactiveState ();
		}
	}
	void BasicState(){
		GetComponent<PlayerController> ().enabled = true;
		//GetComponent<GrappleHook> ().enabled = true;
		GetComponent<WallJump> ().enabled = true;
		GetComponent<LadderClimb> ().enabled = true;
		//GetComponent<FindClosestGrapple>().enabled = true;
	}
	void WallJumpState(){
		GetComponent<LadderClimb> ().enabled = false;
		//GetComponent<PlayerController> ().enabled = false;
		//Debug.Log ("test");
	}
	void GrappleState(){
		GetComponent<PlayerController> ().enabled = false;
		GetComponent<WallJump> ().enabled = false;
		GetComponent<LadderClimb> ().enabled = false;
	}
	void ClimbLadderState(){
		GetComponent<PlayerController> ().enabled = false;
		GetComponent<WallJump> ().enabled = false;
	}
	void HideState(){
		
	}
	void InnactiveState(){
		GetComponent<PlayerController> ().enabled = false;
		//GetComponent<GrappleHook> ().enabled = false;
		GetComponent<WallJump> ().enabled = false;
		GetComponent<LadderClimb> ().enabled = false;
		//GetComponent<FindClosestGrapple>().enabled = false;
		gameObject.SetActive (false);
	}
}
