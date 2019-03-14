using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class LadderClimb : MonoBehaviour {
	private Collider2D col;
	public LayerMask ladderMask;
	public LayerMask groundMask;
	public bool ladder;
	private Rigidbody2D rb2d;
	public float climbSpeed;
	public Collider2D[] contacts = new Collider2D[1];
	private ContactFilter2D filter;
	public bool isClimbing;
	public float center;
	RaycastHit2D groundCheck;
	public bool ground;
	RaycastHit2D ladderCheck;
	private Animator animator;
	public Tilemap tilemap;

	// Use this for initialization
	void Start () {
		col = GetComponent<Collider2D> ();
		rb2d = GetComponent<Rigidbody2D> ();
		//tilemap = ladderMap.GetComponentsInChildren;
		tilemap = GameObject.FindGameObjectWithTag("Ladder").GetComponent<Tilemap>();
		ladder = false;
		filter.SetLayerMask(ladderMask);
		animator = gameObject.GetComponent<Animator> ();
	}
	// Update is called once per frame
	void Update () {
		if (tilemap.HasTile (tilemap.WorldToCell (rb2d.position))) {
			ladder = true;
		} 
		if (ladder){
			if ((Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.DownArrow)) && isClimbing == false) {
				//animator.SetBool ("ladder", true);
				isClimbing = true;
			}
			if (isClimbing == true) {
				rb2d.isKinematic = true;
				//animator.SetBool ("ladder", true);
				ClimbLadder ();
			}
		} else {
			animator.enabled = true;
			animator.SetBool ("ladder", false);
			rb2d.isKinematic = false;
			GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Basic;
			isClimbing = false;
		}
	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.tag == "Ladder") {
			ladder = true; 
			center = tilemap.WorldToCell (rb2d.position).x + .5f;
		}
	}
	void OnTriggerStay2D (Collider2D col){
		if (col.tag == "Ladder") {
			ladder = true;
			center = tilemap.WorldToCell (rb2d.position).x + .5f;
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if (col.tag == "Ladder") {
			ladder = false;
		}
	}

	void ClimbLadder(){
		Camera.main.GetComponent<CameraControl> ().SetPosition(rb2d.position);
		if (tilemap.HasTile (tilemap.WorldToCell (rb2d.position))) {
			animator.SetBool ("ladder", true);
			GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.ClimbLadder;
			transform.position = new Vector2 (center, transform.position.y);
			rb2d.gravityScale = 0;
			groundCheck = Physics2D.Raycast (rb2d.position, Vector2.down, 1f, groundMask);
			//groundCheck = Physics2D.Raycast (rb2d.position, Vector2.down, .1f, ladderMask);
			Debug.DrawLine (rb2d.position, new Vector2 (rb2d.position.x, rb2d.position.y - 1), Color.blue);
			if (!groundCheck.collider) {
				ground = false;
				if (Input.GetKey (KeyCode.X)) {
					animator.enabled = true;
					animator.SetBool ("ladder", false);
					rb2d.isKinematic = false;
					GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Basic;
					isClimbing = false;
				}
				if (Input.GetKey (KeyCode.UpArrow)) {
					animator.enabled = true;
					animator.SetBool ("climbing", true);
					rb2d.velocity = new Vector2 (0, climbSpeed);

				} else if (Input.GetKey (KeyCode.DownArrow)) {
					animator.enabled = true;
					animator.SetBool ("climbing", true);
					rb2d.velocity = new Vector2 (0, -2 * climbSpeed);
				} else {
					animator.enabled = true;
					animator.SetBool ("climbing", true);
					rb2d.velocity = Vector2.zero;
				}
			} else {
				ground = true;
				if (Input.GetKey (KeyCode.UpArrow)) {
					animator.enabled = true;
					rb2d.velocity = new Vector2 (0, climbSpeed);
				}
				if (Input.GetKey (KeyCode.DownArrow)) {
					animator.enabled = true;
					animator.SetBool ("ladder", false);
					rb2d.isKinematic = false;
					GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Basic;
					isClimbing = false;
				}
			}
		}
	}
}
