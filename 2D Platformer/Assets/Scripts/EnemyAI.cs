using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Enemy {
	public bool groundAhead;
	public bool characterFound;
	public int currentState;
	public enum enemyState{Default,Guard,Attack,Search, CheckBehind,SeeSomething};
	private GameObject player;
	public int random;
	private Vector2 CharacterPosition;
	public float maxSpeed = 5f;
	private SpriteRenderer spriteRenderer; 
	private float timer;
	private Vector3 initialPosition;
	private Vector3 finalPosition;
	public float travelDistance;
	private bool toInitial;
	private bool toFinal;
	Rigidbody2D rb2D;
	public LayerMask mask;
	public float distance;
	public GameObject projectile;
	private float bulletTimer = 0;
	public float timeInterval = 1;
	public float bulletSpeed = 100;
	private Vector3 direction;
	public float chaseSpeed = 6f;
	private bool jumped;
	private float timer2;
	private bool chasePlayer;
	// Use this for initialization
	void Start () {
		rb2D = GetComponent<Rigidbody2D> ();
		currentState = (int)enemyState.Default;
		spriteRenderer = GetComponent<SpriteRenderer> ();
		initialPosition = transform.position;
		finalPosition = new Vector3 (initialPosition.x + travelDistance, initialPosition.y, initialPosition.z);
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (currentState == (int)enemyState.Search) {
			SearchState ();
		} else if (currentState == (int)enemyState.Attack) {
			AttackState ();
		} else if (currentState == (int)enemyState.Guard) {
			GuardState ();
		} else if (currentState == (int)enemyState.CheckBehind) {
			CheckBehind ();
		} else if (currentState == (int)enemyState.SeeSomething) {
			SeeSomething ();
		} else if (currentState == (int)enemyState.Default) {
			DefaultState ();
		} else {
			DefaultState ();
		}
	}

	void DefaultState(){
		if (characterFound == true) {
			rb2D.velocity = Vector2.zero;
			timer2 = 0;
			jumped = false;
			currentState = (int)enemyState.SeeSomething;
		}
		var direction = new Vector2(transform.localScale.x, 0);
		Debug.DrawRay (transform.position, direction);
		var hit = Physics2D.Raycast (transform.position, direction, distance, mask);
		if (hit.collider && hit.collider.gameObject.GetComponent<PlayerController>().hidden == false) {
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

		random = Random.Range (0, 1000);
		if (random == 1) {
			timer = 0;
			transform.localScale = new Vector3 (-transform.localScale.x,1,1);
			rb2D.velocity = Vector2.zero;
			currentState = (int)enemyState.CheckBehind;
		}
	}
	void SeeSomething(){
		if (!jumped) {
			jumped = true;
			rb2D.AddForce (new Vector2 (0, 5), ForceMode2D.Impulse);
		}
		timer2 += Time.deltaTime;
		if (timer2 > .7f){
			bulletTimer = 1.5f;
			currentState = (int)enemyState.Attack;
		}
	}

	void moveToFinal(){
		float x;
		if (characterFound == true) {
			rb2D.velocity = new Vector2(0,rb2D.velocity.y);
			timer2 = 0;
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
			rb2D.velocity = new Vector2(x*maxSpeed,rb2D.velocity.y);
		}
	}

	void moveToInitial(){
		float x;
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

	void CheckBehind(){
		
		timer += Time.deltaTime;
		//Wait("CheckBehind", 5);
		if (timer >= Random.Range (3, 8)) {
			transform.localScale = new Vector3 (-transform.localScale.x, 1, 1);
			currentState = (int)enemyState.Default;
		} 
		var direction = new Vector2(transform.localScale.x, 0);
		Debug.DrawRay (transform.position, direction);
		var hit = Physics2D.Raycast (transform.position, direction, distance, mask);
		if (hit.collider) {
			characterFound = true;
		} else {
			characterFound = false;
		}
	}
		
	void SearchState (){
		
		if (characterFound == true) {
			currentState = (int)enemyState.Attack;
		}

		if (characterFound == false) {
			currentState = (int)enemyState.Guard;
		}
	}
	void GuardState(){
		if (characterFound == true) {
			currentState = (int)enemyState.Attack;
		}
	}
	void AttackState(){
		if (characterFound == false) {
			currentState = (int)enemyState.Default;
		}
		var hit = Physics2D.Linecast (transform.position, player.GetComponentInParent<Rigidbody2D> ().transform.position, mask);
		Debug.DrawLine (transform.position, player.GetComponentInParent<Rigidbody2D> ().transform.position);
		if (hit.collider.gameObject.GetComponentInParent<Rigidbody2D>() == player.GetComponentInParent<Rigidbody2D> ()) {
			characterFound = true;

			direction = -(transform.position - player.GetComponentInParent<Rigidbody2D> ().transform.position);
			Vector2 finalDirection = new Vector2 (direction.x, direction.y);

			if (direction.x > 0) {
				transform.localScale = new Vector3 (1, 1, 1);
			} else {
				transform.localScale = new Vector3(-1,1,1);
			}

			float distanceToPlayer = Vector2.Distance (rb2D.transform.position, player.GetComponentInParent<Rigidbody2D> ().transform.position);
			if (distanceToPlayer < 10 && distanceToPlayer > 3 && groundAhead) {
				chasePlayer = true;
				rb2D.velocity = new Vector2 (chaseSpeed * finalDirection.normalized.x, rb2D.velocity.y);
			} else {
				rb2D.velocity = Vector2.zero;
					
				chasePlayer = false;
			}

			bulletTimer += Time.deltaTime;
			if (bulletTimer >= timeInterval) {
				GameObject newProjectile;
				newProjectile = Instantiate (projectile, transform.position, transform.rotation) as GameObject;
				finalDirection.Normalize ();
				newProjectile.GetComponent<Rigidbody2D> ().velocity = finalDirection * bulletSpeed;
				bulletTimer = 0;
			}
		} else {
			characterFound = false;
		}
	}
}
