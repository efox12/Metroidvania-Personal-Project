﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimb : MonoBehaviour {
	private Collider2D col;
	public LayerMask mask;
	public bool ladder;
	private Rigidbody2D rb2d;
	public float climbSpeed;
	public Collider2D[] contacts = new Collider2D[1];
	private ContactFilter2D filter;
	public bool isClimbing;
	public float center;
	private Animator animator;
	// Use this for initialization
	void Start () {
		col = GetComponent<Collider2D> ();
		rb2d = GetComponent<Rigidbody2D> ();
		filter.layerMask = mask;
		animator = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (isClimbing == true){
			if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.DownArrow)) {
				animator.SetBool ("ladder", true);
				ladder = true;
			}
			if (ladder == true) {
				rb2d.isKinematic = true;
				ClimbLadder ();
			}
		} else {
			animator.enabled = true;
			animator.SetBool ("ladder", false);
			rb2d.isKinematic = false;
			GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Basic;
			ladder = false;
		}
	}
	void ClimbLadder(){
		GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.ClimbLadder;
		transform.position = new Vector2 (center, transform.position.y);
		rb2d.gravityScale = 0;
		if (Input.GetKey (KeyCode.UpArrow)) {
			animator.enabled = true;
			rb2d.velocity = new Vector2 (0, climbSpeed);
		} else if(Input.GetKey (KeyCode.DownArrow)){
			animator.enabled = true;
			rb2d.velocity = new Vector2 (0, -2*climbSpeed);
		} else {
			animator.enabled = false;
			rb2d.velocity = Vector2.zero;
		}
		//if (Input.GetKeyDown (KeyCode.X) || GetComponentInParent<SwitchCharacter>().grounded == true) {
		if (GetComponent<TheifStateMachine> ().currentState == (int)TheifStateMachine.theifState.ClimbLadder 
			&& Input.GetKeyDown (KeyCode.X)) {
			animator.enabled = true;
			animator.SetBool ("ladder", false);
			rb2d.isKinematic = false;
			GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Basic;
			ladder = false;
		}
	}
}
