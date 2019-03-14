using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
[System.Serializable]
public class PlayerController : MonoBehaviour {
	
	Rigidbody2D rb;
	public bool grounded;
	public bool walled;
	public bool velocityControlled = true;
	public bool jumping;
	public int currentHealth;
	public int maxHealth;
	public float targetSpeed;
	public float deceleration = .2f;
	public bool hidden;
	public LayerMask groundMask;
	public LayerMask enemyMask;
	private Animator animator;
	private float timeHeld = 0f;
	public float velocity;

	void Start () {
		rb = GetComponent <Rigidbody2D> ();
		grounded = GetComponentInChildren<GroundCheck>().ground;
		animator = gameObject.GetComponent<Animator> ();
		currentHealth = maxHealth;
		velocityControlled = true;
	}

	void Update(){
		grounded = GetComponentInChildren<GroundCheck>().ground;
		Camera.main.GetComponent<CameraControl> ().SetPosition(rb.position);
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		if (currentHealth <= 0) {
			//die ();
		}

		if (grounded) {
			GetComponent<Collider2D> ().offset = new Vector2 (0.065f, -0.1f);
		} else {
			GetComponent<Collider2D> ().offset = new Vector2 (0.065f, 0.1f);
		}
		Move ();
		Attack ();
	}

	void Attack(){
		//bool melee;
		//bool hitEnemy;
		RaycastHit2D hitEnemy;
		if (Input.GetKeyDown (KeyCode.C)) {
			//melee = true;
			hitEnemy = Physics2D.BoxCast (transform.position, new Vector2 (0.05f, 0.8f), 0, Vector2.right * transform.localScale.x, 1, enemyMask);
			if (hitEnemy.collider != null) {
				//hitEnemy.collider.GetComponentDecrementHealth ();
			}
		}
	}
		
	private void Move(){
		/*----------------------------------------------------- JUMPING -------------------------------------------------------------*/
		rb.gravityScale = 3f;
		if (grounded || walled) {
			jumping = false;
		}

		//Start jumping on jump button press
		if (Input.GetKeyDown(KeyCode.X) && jumping == false && (grounded || walled || GetComponentInChildren<GroundCheck> ().canJump)){
			rb.velocity = new Vector2 (rb.velocity.x, 10);
			jumping = true;
			timeHeld = 0f;
		}

		//Jump higher while holding jump button down
		if(Input.GetKey(KeyCode.X) && !grounded && rb.velocity.y > 0 && timeHeld < .075f){
			timeHeld += Time.fixedDeltaTime;
			rb.velocity = new Vector2 (rb.velocity.x, 10);
		}

		//Stop moving up when the jump key is released
		if(Input.GetKeyUp(KeyCode.X) && !grounded && rb.velocity.y > 0 && velocityControlled == true)
			rb.velocity = new Vector2 (rb.velocity.x, 0);

		animator.SetBool ("grounded", grounded);

		/*----------------------------------------------------- HORIZONTAL MOVEMENT -------------------------------------------------------------*/
		//Make sure not moving into a wall
		//RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, .3f, groundMask);
		if(velocityControlled == true){
			velocity = Input.GetAxisRaw("Horizontal");
			if(velocity != 0){
				Vector2 baseVelocity = new Vector2 (targetSpeed * velocity, rb.velocity.y);
				rb.velocity = baseVelocity;
			} else {
				rb.velocity = new Vector2 (rb.velocity.x * deceleration, rb.velocity.y);
			}
		}

		//Flip the player based on current scale and velocity
		FlipPlayer(velocity);

		animator.SetFloat ("speed", Mathf.Abs(rb.velocity.x));

		/*----------------------------------------------------- MISCELLANEOUS -------------------------------------------------------------*/
		if (grounded && Input.GetKeyDown (KeyCode.DownArrow)) {
			hidden = !hidden;
		}	
	}

	private void FlipPlayer(float velocityInput){
		//Flip player if facing the wrong direction
		if(rb.velocity.x < 0){
		//if (velocityInput < 0 && transform.localScale.x > 0) {
			transform.localScale = new Vector3(-1,1,transform.localScale.z);
		}
		if(rb.velocity.x > 0){
		//if (velocityInput > 0 && transform.localScale.x < 0) {
			transform.localScale = new Vector3(1,1, transform.localScale.z);
		}
	}

	/*
	private void die(){
		int scene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}
	*/

	// Returns a vector parellel to the ground in the direction the player is facing
	/*
	private Vector2 GroundAngle(float velocityInput){
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, 1, groundMask);
		Debug.DrawRay (transform.position, hit.normal);
		Vector2 groundAngle;
		Vector2 normal = hit.normal;
		if (hit.collider != null && normal.y > .3f) {
			if (velocityInput > 0) {
				groundAngle = new Vector2 (normal.y, -normal.x);
			} else {
				groundAngle = new Vector2 (-normal.y, normal.x);
			}
		} else {
			groundAngle = new Vector2 (transform.localScale.x, 0);
		}
		return groundAngle;
	}
	*/
}
