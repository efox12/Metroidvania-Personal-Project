using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {
	private PlayerController p;
	RaycastHit2D hit;
	public bool ground;
	public float distance;
	public LayerMask mask;
	void Start(){
		p = gameObject.GetComponentInParent<PlayerController> ();
	}
	//void Update(){
		//Physics2D.queriesStartInColliders = false;
		//hit = Physics2D.BoxCast(transform.position, new Vector2(1f, .05f), 0, Vector2.down, distance, mask);
		//if (hit.collider != null) {
		//	Debug.DrawLine (transform.position, hit.point);
		//	ground = true;
		//	p.grounded = true;
		//} else {
		//	ground = false;
		//	p.grounded = false;
		//}
	//}
	void OnTriggerEnter2D(Collider2D col){
		ground = true;
		p.grounded = true;
	}

	void OnTriggerStay2D(Collider2D col){
		ground = true;
		p.grounded = true;
	}

	void OnTriggerExit2D(Collider2D col){
		ground = false;
		p.grounded = false;
	}
}
