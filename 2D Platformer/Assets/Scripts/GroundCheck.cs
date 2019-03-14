using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {
	RaycastHit2D hit;
	public bool ground;
	public float distance;
	public bool canJump;
	public float timePassed = 0;
	public LayerMask groundMask;
	private ContactFilter2D filter;
	
	void Update(){
		jumping();
	}

	void OnTriggerEnter2D(Collider2D col){
		//print(LayerMask.LayerToName(col.gameObject.layer));
		if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))) {
			ground = true;
			canJump = true;
		} 
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))) {
			ground = true;
			canJump = true;
		} 
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))) {
			canJump = true;
			ground = false;
		}
	}

	public void jumping(){
		if(canJump && timePassed < .15f){
			timePassed += Time.fixedDeltaTime;
		} else{
			canJump = false;
			timePassed = 0;
		}
	}
}
