using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour {
	public float distance;
	public float offVelocity;
	public bool wallJumping;
	public Rigidbody2D rb2D;
	public LayerMask wallMask;
	public float timeHeld = 0;
	private Animator animator;
	public RaycastHit2D hit;
	GameObject player;

	public int direction;

	// Use this for initialization
	void Start () {
		rb2D = GetComponent<Rigidbody2D> ();
		animator = gameObject.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		checkForWall();
		jumping();
	}

	public void checkForWall(){
		// rays for checking for walls
		Physics2D.queriesStartInColliders = false;
		hit = Physics2D.BoxCast (transform.position, new Vector2 (0.05f, 0.7f), 0, Vector2.right * transform.localScale.x, distance, wallMask);
		
		// check to see if there is a wall
		if (hit.collider) {
			GetComponent<PlayerController> ().walled = true;
			// set the wall slide animation to true
			if (hit.collider) {
				animator.SetBool ("wall", true);
			}
			// jump if touching a wall and jump button pressed
			if (GetComponentInChildren<GroundCheck> ().ground == false && Input.GetKeyDown (KeyCode.X) && hit.collider != null) {
				Jump (hit);
			}

			// slow down falling while wall sliding
			if (rb2D.velocity.y < 0 && (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow))) {
				rb2D.velocity = new Vector2 (0, rb2D.velocity.y/2);
			}

		} else {
			//set the wall slide animation to false
			animator.SetBool ("wall", false);
			GetComponent<PlayerController> ().walled = false;
		}
		
		// stop wall sliding if touching the ground
		if (GetComponentInChildren<GroundCheck> ().ground == true) {
			wallJumping = false;
		}
	}

	// perform a wall jump 
	public void Jump(RaycastHit2D hit){
		animator.SetBool ("wall", false);
		wallJumping = true;
		//rb2D.velocity = new Vector2(offVelocity * hit.normal.x, rb2D.velocity.y);
		Debug.DrawRay (transform.position, hit.normal);
		timeHeld = 0;

		if( hit.normal.x > 0){
			direction = 1;
		} else {
			direction = -1;
		}
	}

	public void jumping(){
		if(wallJumping){
			if(timeHeld < .06f){
				GetComponent<PlayerController> ().velocityControlled = false;
				rb2D.velocity = new Vector2(offVelocity * direction, rb2D.velocity.y);
				Debug.Log(offVelocity * direction);
				timeHeld += Time.fixedDeltaTime;
			} else{
				GetComponent<PlayerController> ().velocityControlled = true;
			}
		}
	}
}
