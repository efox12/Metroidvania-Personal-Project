using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;

	public float jumpPower; //400
	public bool grounded;

	public bool jumping;
	public float holdJump; //300

	public int currentHealth;
	public int maxHealth;

	public float airSpeed; //5
	public float targetSpeed; //8
	public float acceleration;

	public bool hidden;
	public bool teleport;

	public LayerMask groundMask;
	public LayerMask enemyMask;

	private Animator animator;
	private Collider2D collider;

	private float timeHeld = 0f;

	public float velocity;
	void Start () {
		collider = GetComponent<Collider2D> ();
		rb = GetComponent <Rigidbody2D> ();
		animator = gameObject.GetComponent<Animator> ();
		currentHealth = maxHealth;
	}

	void Update(){
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		if (currentHealth <= 0) {
			//die ();
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
			hitEnemy = Physics2D.BoxCast (transform.position, 
				new Vector2 (0.05f, 0.8f), 0, Vector2.right * transform.localScale.x, 1, enemyMask);
			if (hitEnemy.collider != null) {
				//hitEnemy.collider.GetComponentDecrementHealth ();
			}
		}
	}
		
	private void Move(){
		rb.drag = 0;
		if (grounded) {
			jumping = false;
		}

		//Start jumping on jump button press
		if (Input.GetKeyDown(KeyCode.X) && jumping == false && grounded){
			Debug.Log ("Jump");
			if (rb.velocity.y < 0) {
				rb.velocity = new Vector2 (rb.velocity.x, 0);
			}
			rb.AddForce (Vector2.up * jumpPower);
			jumping = true;
			timeHeld = 0f;
		}

		//Jump higher while holding jump button down
		if(Input.GetKey(KeyCode.X) && !grounded && rb.velocity.y > 0 && timeHeld < .1f){
			timeHeld += Time.fixedDeltaTime;
			rb.AddForce (Vector2.up * jumpPower/holdJump, ForceMode2D.Impulse);
		} 

		//Increase gravity when falling
		if ((rb.velocity.y < 0 && !grounded) || (Input.GetKeyUp (KeyCode.UpArrow) && !grounded)) {
			rb.gravityScale = 2;
		} else {
			rb.gravityScale = 2.5f;
		}

		velocity = Input.GetAxis("Horizontal");

		//Make sure not moving into a wall
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.right * transform.localScale.x, .3f, groundMask);
		//if (hit.collider == null || hit.normal.y != 0) {
			//Ground Movement
			if (grounded && velocity != 0) {
				Vector2 directionVector = GroundAngle (velocity);
				Debug.DrawRay (transform.position, directionVector);	
				Vector2 baseVelocity = new Vector2 (directionVector.x * targetSpeed * Mathf.Abs (velocity), rb.velocity.y);
				if (!(Input.GetKey (KeyCode.X))) {
					rb.velocity = directionVector.normalized * baseVelocity.magnitude;
				} 
				//rb.velocity = new Vector2 (baseVelocity.x, rb.velocity.y);
			} else if (grounded && velocity == 0) {
				//rb.velocity = new Vector2 (targetSpeed * velocity, rb.velocity.y);
				rb.velocity = new Vector2 (rb.velocity.x * .7f, rb.velocity.y);
			}
			//Air Movement
			else if (!grounded) {
				if (Input.GetKey(KeyCode.RightArrow)) {
					if (rb.velocity.x < targetSpeed) {
						rb.velocity = new Vector2 ((rb.velocity.x + acceleration), rb.velocity.y);
					} else {
						rb.velocity = new Vector2 (targetSpeed * velocity, rb.velocity.y);
					}
				} else if (Input.GetKey(KeyCode.LeftArrow)) {
					if (rb.velocity.x > -targetSpeed) {
						rb.velocity = new Vector2 ((rb.velocity.x - acceleration), rb.velocity.y);
					} else {
						rb.velocity = new Vector2 (targetSpeed * velocity, rb.velocity.y);
					}
				} else {
					//rb.velocity = new Vector2 (targetSpeed * velocity, rb.velocity.y);
					rb.velocity = new Vector2 (rb.velocity.x * .98f, rb.velocity.y);
				}
			}

		//}

		//Flip the player based on current scale and velocity
		FlipPlayer(velocity);
		if (grounded && Input.GetKeyDown (KeyCode.DownArrow)) {
			hidden = !hidden;
		}
		animator.SetBool ("grounded", grounded);
		animator.SetFloat ("speed", Mathf.Abs(rb.velocity.x));
	}

	private void die(){
		int scene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}

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

	private void FlipPlayer(float velocityInput){
		//Flip player if facing the wrong direction
		if(rb.velocity.x <0){
		//if (velocityInput < 0 && transform.localScale.x > 0) {
			transform.localScale = new Vector2(-1,1);
		}
		if(rb.velocity.x > 0){
		//if (velocityInput > 0 && transform.localScale.x < 0) {
			transform.localScale = new Vector2(1,1);
		}
	}
}
