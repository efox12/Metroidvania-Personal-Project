using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {
	private GameObject player;
	public float distance;
	public LayerMask mask;
	public LayerMask groundMask;
	public bool seePlayer;
	private Vector2 direction; 
	public RaycastHit2D hit;
	public RaycastHit2D hitGround;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		//direction = new Vector2(transform.localScale.x, 0);
		//Debug.DrawRay (transform.position, direction);
		//hit = Physics2D.Raycast (transform.position, direction, distance, mask);
		//if (hit.collider) {
		///	seePlayer = true;
		//	GetComponent<EnemyAI> ().characterFound = true;
		//} else {
		//	seePlayer = false;
		//	GetComponent<EnemyAI> ().characterFound = false;
		//}
		Vector2 groundDirection = new Vector2 (transform.localScale.x, -1f);
		hitGround = Physics2D.Raycast (transform.position, groundDirection, 1, groundMask);
		Debug.DrawRay (transform.position, groundDirection, Color.blue);
		if (hitGround.collider) {
			seePlayer = true;
			GetComponent<EnemyAI> ().groundAhead = true;
		} else {
			seePlayer = false;
			GetComponent<EnemyAI> ().groundAhead = false;
		}
	}
}
