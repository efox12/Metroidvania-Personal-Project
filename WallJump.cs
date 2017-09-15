using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour {
	GroundCheck groundCheck;
	public float distance;
	public float offVelocity;
	public float upVelocity;
	public bool wallJumping;
	public Rigidbody2D rb2D;
	public LayerMask wallMask;
	public float time = 0;
	private Animator animator;
	// Use this for initialization
	void Start () {
		groundCheck = GetComponentInChildren<GroundCheck> ();
		rb2D = GetComponent<Rigidbody2D> ();
		animator = gameObject.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		Physics2D.queriesStartInColliders = false;
		RaycastHit2D hit = Physics2D.BoxCast (transform.position, new Vector2 (0.05f, 0.7f), 0, Vector2.right * transform.localScale.x, distance, wallMask);
		RaycastHit2D hit2 = Physics2D.BoxCast (transform.position, new Vector2 (0.01f, 0.7f), 0, -(Vector2.right * transform.localScale.x), distance, wallMask);
		if (hit.collider || hit2.collider) {
			if (hit.collider) {
				animator.SetBool ("wall", true);
			}
			if (groundCheck.ground == false && Input.GetKeyDown (KeyCode.X) && hit.collider != null) {
				Jump (hit);
			} else if (groundCheck.ground == false && Input.GetKeyDown (KeyCode.X) && hit2.collider != null) {
				Jump (hit2);
			}
			if (rb2D.velocity.y < 0 && (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow))) {
				GetComponent<Rigidbody2D> ().velocity -= new Vector2 (0, rb2D.velocity.y / 10);
			}
		} else {
			animator.SetBool ("wall", false);
		}

	
		if (GetComponentInChildren<GroundCheck> ().ground == true) {
			wallJumping = false;
		}
	}
	public void Jump(RaycastHit2D hit){
		animator.SetBool ("wall", false);
		//transform.localScale = new Vector3 (-transform.localScale.x, 1, 1);
		rb2D.velocity = Vector2.zero;
		GetComponent<PlayerController> ().enabled = false;
		rb2D.AddForce (new Vector2 (offVelocity * hit.normal.x, upVelocity), ForceMode2D.Impulse);
		Debug.DrawRay (transform.position, hit.normal);
	}
}
