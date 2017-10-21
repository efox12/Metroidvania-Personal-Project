using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour {
	public float Length = 4;

	[HideInInspector]
	public bool IsEnabled
	{
		get { return points.Count > 0; }
	}

	private readonly List<GameObject> points = new List<GameObject>();
	public LineRenderer ropeLine;
	GameObject grapple;
	GameObject previousGrapple;
	private DistanceJoint2D joint;
	private Rigidbody2D rb2d;
	public float pushForce = 10f;
	public float step = .1f;
	public bool pushright;
	public bool pushLeft;
	public bool grappled;
	public LayerMask mask;
	public LayerMask mask1;
	public Vector3 grapplePoint;
	public bool deployGrapple;
	public int currentState;
	private Animator animator;


	void Start(){
		rb2d = GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator> ();

		grapple = new GameObject("Grapple");
		grapple.AddComponent<CircleCollider2D>().radius = .01f;
		grapple.AddComponent<Rigidbody2D>();
		grapple.GetComponent<Rigidbody2D> ().isKinematic = true;
		grapple.SetActive (false);

		previousGrapple = (GameObject)Instantiate(grapple);
		previousGrapple.name = "Previous Grapple";

		joint = gameObject.AddComponent<DistanceJoint2D>();
		joint.enabled = false;
		grappled = false;
		deployGrapple = false;
	}

	void Update(){
		// handles input for the grapple button
		if (Input.GetKey (KeyCode.Z) && GetComponent<PlayerController> ().grounded == false) {
			deployGrapple = true;
		} else {
			deployGrapple = false;
		}

	}

	void FixedUpdate(){
		// handles physics for the grapple
		if (IsEnabled) UpdateGrapple();
		else CheckForGrapple();
	}

	// Checks to see if a grapple is in range
	private void CheckForGrapple(){
		if(deployGrapple == true){
			var hit = Physics2D.Linecast(transform.position, grapplePoint, mask);
			var distance = Vector3.Distance(transform.position, hit.point);
			if (hit.collider != null && distance <= Length){
				GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Grapple;
				animator.SetBool ("rope", true);
				ropeLine.positionCount = 2;
				ropeLine.SetPosition(0, hit.point);
				ropeLine.SetPosition(1, transform.position);
				ropeLine.gameObject.SetActive(true);
				points.Add(CreateGrapplePoint(hit));
				grapple.SetActive (true);
				Debug.Log(previousGrapple.transform.position);
				previousGrapple.SetActive (true);
				grapple.transform.position = hit.point;
				previousGrapple.transform.position = hit.point;
				SetParent(grapple.transform, hit.collider.transform);
				joint.enabled = true;
				joint.connectedBody = grapple.GetComponent<Rigidbody2D>();
				joint.distance = Vector2.Distance(hit.point, transform.position);
			}
		}
	}

	// creates a new hinge joint at the point where a raycast hits an object
	private GameObject CreateGrapplePoint(RaycastHit2D hit){
		var p = new GameObject("GrapplePoint");
		SetParent(p.transform, hit.collider.transform);
		p.transform.position = hit.point;
		return p;
	}

	// updates various components in the grapple
	private void UpdateGrapple(){
		rb2d.drag = .5f;
		rb2d.gravityScale = 3;
		UpdateLineDrawing();

		var hit = Physics2D.Linecast(transform.position, grapple.transform.position, mask1);
		var hitPrev = Physics2D.Linecast(transform.position, previousGrapple.transform.position, mask1);

		if (hit.collider.gameObject != grapple && hit.collider.gameObject != previousGrapple){
			// if you lose line of sight on the grappling hook, then add a new point to wrap around

			points.Add(CreateGrapplePoint(hit));

			UpdateLineDrawing();

			previousGrapple.transform.position = grapple.transform.position;
			SetParent(previousGrapple.transform, grapple.transform.parent);
			grapple.transform.position = hit.point;
			SetParent(grapple.transform, hit.collider.transform);
			joint.distance = Vector2.Distance(transform.position, grapple.transform.position);

		} else if (Vector2.Distance(grapple.transform.position, previousGrapple.transform.position) <= .1f){
			RemoveLastCollider();
		}
		// check to see if player let go of the grapple button
		if(deployGrapple == false){

			// jump off
			if (Input.GetKey (KeyCode.UpArrow) && transform.position.y < grapple.transform.position.y)
				rb2d.velocity = new Vector2 (rb2d.velocity.x, 5);

			RetractRope();
		}
		ropeLine.SetPosition(points.Count, transform.position);
			if (Input.GetKey (KeyCode.LeftArrow)) {
				rb2d.transform.localScale = new Vector2 (-1, 1);
				rb2d.AddForce (Vector2.right * (-pushForce / 20));
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				rb2d.transform.localScale = new Vector2 (1, 1);
				rb2d.AddForce (Vector2.right * (pushForce / 20));
			} 

		// allow the rope to retract and extend as to certain lengths
		if (joint.distance > .5) {
			if (Input.GetKey (KeyCode.UpArrow)) {
				joint.distance -= step;
				animator.SetBool ("climbing", true);

			} else if (Input.GetKey (KeyCode.DownArrow)) {
				joint.distance += step;
				animator.SetBool ("climbing", true);
			} else {
				animator.SetBool ("climbing", false);
			}
		}

		// get rid of the rope if the player touches the ground
		if (GetComponentInChildren<GroundCheck>().ground) {
			RetractRope();
		}

		// if you can see previous point then unroll back to that point
		if (hitPrev.collider != null && hitPrev.transform == previousGrapple.transform)
			RemoveLastCollider();
	}

	// destroy the rope and all the grapplePoints
	private void RetractRope(){
		animator.SetBool ("rope", false);
		rb2d.drag = 0;
		rb2d.gravityScale = 2.5f;
		deployGrapple = false;
		joint.enabled = false;
		ropeLine.gameObject.SetActive(false);
		points.ForEach(Destroy);
		points.Clear();
		grapple.SetActive (false);
		previousGrapple.SetActive(false);
		GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Basic;
	}

	// remove the last collider and switch to the previous joint
	private void RemoveLastCollider(){
		if (points.Count > 1){
			Destroy(points[points.Count - 1]);
			points.RemoveAt(points.Count - 1);

			UpdateLineDrawing();
			grapple.transform.position = previousGrapple.transform.position;

			joint.distance = Vector2.Distance(transform.position, grapple.transform.position);

			SetParent(grapple.transform, previousGrapple.transform.parent);
		}

		if (points.Count > 1)
			previousGrapple.transform.position = points [points.Count - 2].transform.position;
		else {
			
			previousGrapple.transform.position = grapple.transform.position;
		}
	}

	// update the line representing the rope
	private void UpdateLineDrawing(){
		ropeLine.positionCount = points.Count + 1;
		for (var i = 0; i < points.Count; i++)
			ropeLine.SetPosition(i, points[i].transform.position);
		ropeLine.SetPosition(points.Count, transform.position);
	}

	// sets a childs parent
	private void SetParent(Transform child, Transform parent){
		child.SetParent(parent);
		if (parent != null)
			child.localScale = new Vector3(1 / parent.localScale.x, 1 / parent.localScale.y, 1 / parent.localScale.z);
	}
}