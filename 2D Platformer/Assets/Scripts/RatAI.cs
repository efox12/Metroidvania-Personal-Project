using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAI : Enemy {
	public bool groundAhead;
	public bool characterFound;
	public int currentState;
	public float travelDistance;
	public float distance;
	public float maxSpeed = 5f;
	public float chaseSpeed = 6f;

	private bool toInitial;
	private bool toFinal;
	public bool jumped;
	private bool chasePlayer;
	private float timer;
	private float timer2;
	public float knockback;
	private Rigidbody2D rb2D;
	private GameObject player;
	public LayerMask playerMask;
	private Vector2 CharacterPosition;
	private Vector3 initialPosition;
	private Vector3 finalPosition;
	private Vector3 direction;

	private CharacterLife playerLife;

	public enum enemyState{Default,Guard,Attack,Search, CheckBehind,SeeSomething};

	// Use this for initialization
	void Start () {
		rb2D = GetComponent<Rigidbody2D> ();
		currentState = (int)enemyState.Default;
		initialPosition = transform.position;
		finalPosition = new Vector3 (initialPosition.x + travelDistance, initialPosition.y, initialPosition.z);
		player = GameObject.FindGameObjectWithTag ("Player");
		playerLife = (CharacterLife) player.gameObject.GetComponent<CharacterLife> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentState == (int)enemyState.Attack) {
			AttackState ();
		} else if (currentState == (int)enemyState.SeeSomething) {
			SeeSomething ();
		}  else {
			DefaultState ();
		}
	}

	void DefaultState(){
		if (characterFound == true) {
			rb2D.velocity = Vector2.zero;
			timer = 0;
			jumped = false;
			currentState = (int)enemyState.SeeSomething;
		}

		var direction = new Vector2(transform.localScale.x, 0);
		Debug.DrawRay (transform.position, direction);
		var hit = Physics2D.Raycast (transform.position, direction, distance, playerMask);

		//&& hit.collider.gameObject.GetComponent<PlayerController>().hidden == false
		if (hit.collider && player.GetComponent<CharacterLife>().invincibilityFrames == false) {
			characterFound = true;
		} else {
			characterFound = false;
		}

		if (rb2D.velocity.x > 0) {
			transform.localScale = new Vector3(1, 1, 1);
		} else {
			transform.localScale = new Vector3(-1, 1, 1);
		}

		if (toFinal) {
			moveToFinal ();
		} else if (toInitial) {
			moveToInitial ();
		} else {
			moveToFinal ();
		}
	}
	void SeeSomething(){
		if (!jumped) {
			jumped = true;
			rb2D.AddForce (new Vector2 (0, 5), ForceMode2D.Impulse);
		}
		timer2 = 0;
		currentState = (int)enemyState.Attack;
	}

	void moveToFinal(){
		float x;
		if (characterFound == true) {
			rb2D.velocity = new Vector2(0,rb2D.velocity.y);
			timer = 0;
			jumped = false;
			currentState = (int)enemyState.SeeSomething;
		}
		if (groundAhead == false || transform.position.x > finalPosition.x) {
			transform.localScale = new Vector3(-1*rb2D.transform.localScale.x, 1, 1);
			toFinal = false;
			toInitial = true;
		} else {
			if (travelDistance > 0) {
				x = 1;  
			} else {
				x = -1;
			}
			timer2 = 0;
			rb2D.velocity = new Vector2(x*maxSpeed,rb2D.velocity.y);
		}
	}

	void moveToInitial(){
		float x;
		if (characterFound == true) {
			rb2D.velocity = new Vector2(0,rb2D.velocity.y);
			timer = 0;
			jumped = false;
			currentState = (int)enemyState.SeeSomething;
		}
		if (groundAhead == false || transform.position.x < initialPosition.x) {
			transform.localScale = new Vector3(-1*rb2D.transform.localScale.x, 1, 1);
			toFinal = true;
			toInitial = false;
		} else {
			if (travelDistance > 0) {
				x = -1;  
			} else {
				x = 1;
			}
			rb2D.velocity = new Vector2(x*maxSpeed,rb2D.velocity.y);
		}
	}

	void AttackState(){
		if (characterFound == false) {
			currentState = (int)enemyState.Default;
		}
		var hit = Physics2D.Linecast (transform.position, player.GetComponentInParent<Rigidbody2D> ().transform.position, playerMask);
		Debug.DrawLine (transform.position, player.GetComponentInParent<Rigidbody2D> ().transform.position);
		if (hit.collider) {
			if (hit.collider.gameObject.GetComponentInParent<Rigidbody2D> () == player.gameObject.GetComponentInParent<Rigidbody2D> ()) {
				characterFound = true;
				direction = -(transform.position - player.GetComponentInParent<Rigidbody2D> ().transform.position);
				Vector2 finalDirection = new Vector2 (direction.x, direction.y);

				if (direction.x > 0) {
					transform.localScale = new Vector3 (1, 1, 1);
				} else {
					transform.localScale = new Vector3 (-1, 1, 1);
				}

				float distanceToPlayer = Vector2.Distance (rb2D.transform.position, player.GetComponentInParent<Rigidbody2D> ().transform.position);
				if (distanceToPlayer < 10 && distanceToPlayer > 0 && groundAhead) {
					chasePlayer = true;
					rb2D.velocity = new Vector2 (chaseSpeed * finalDirection.normalized.x, rb2D.velocity.y);
				} else {
					rb2D.velocity = Vector2.zero;
					characterFound = false;
				}
				if (distanceToPlayer < 1.5 && timer2 > .5 && player.GetComponent<CharacterLife> ().invincibilityFrames == false) {
					rb2D.velocity = Vector2.zero;
					rb2D.AddForce (new Vector2 (direction.x * 5, 5), ForceMode2D.Impulse);
					timer2 = 0;
				} else {
					timer2 += Time.deltaTime;
				}

			} else {
				characterFound = false;
			}
		}
	}

	public void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.Equals(player)){
			playerLife.takeDamage (1, rb2D.position, knockback);
				player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
				//player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction.x * 15, 6), ForceMode2D.Impulse);
				float time = 0;
				Camera.main.GetComponent<CameraControl> ().Shake (.2f, 3, 40);
				if (time < .5) {
					time += Time.deltaTime;
			}
		}
	}
	public void OnCollisionStay2D(Collision2D col){
		if(col.gameObject.Equals(player)){
			playerLife.takeDamage (1, rb2D.position, knockback);
				player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
				player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction.x * 15, 6), ForceMode2D.Impulse);
				float time = 0;
				Camera.main.GetComponent<CameraControl> ().Shake (.2f, 3, 40);
				if (time < .5) {
					time += Time.deltaTime;
			}
		}
	}
}
