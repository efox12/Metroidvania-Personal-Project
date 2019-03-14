using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour {
	public float Length = 4;

	[HideInInspector]
	public bool IsEnabled
	{
		get { return joint.enabled; }
	}

	private  Stack<Vector2> previousPointsStack = new Stack<Vector2>();
	private  Stack<bool> movingClockwiseStack = new Stack<bool>();
	private  Stack<Vector2> unwindVectorsStack = new Stack<Vector2>();
	public LineRenderer ropeLine;
	GameObject grapple;
	private DistanceJoint2D joint;
	private Rigidbody2D rb2d;
	public float pushForce = 10f;
	public float step = .1f;
	public bool grappled;
	public bool hasGrapple;
	public LayerMask mask;
	public LayerMask mask1;
	public Vector3 grapplePoint;
	public bool deployGrapple;
	private Animator animator;
	private GameObject player;

	
	void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		ropeLine = player.GetComponentInChildren<LineRenderer> ();
		rb2d = player.GetComponent<Rigidbody2D>();
		animator = player.GetComponent<Animator> ();
		
		grapple = new GameObject("Grapple");
		grapple.AddComponent<CircleCollider2D>().radius = .01f;
		grapple.AddComponent<Rigidbody2D>();
		grapple.GetComponent<Rigidbody2D> ().isKinematic = true;
		grapple.SetActive (false);

		joint = player.AddComponent<DistanceJoint2D>();
		joint.enabled = false;
		grappled = false;
		deployGrapple = false;
		hasGrapple = GlobalObject.Instance.playerData.GetHasGrapple();

		GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Grapple;
		player.GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Basic;
	}

	void Update(){
		
		if (hasGrapple) {
			// handles input for the grapple button
			if (Input.GetKey (KeyCode.Z) && GetComponentInChildren<GroundCheck> ().canJump == false) {
				deployGrapple = true;
			} else {
				deployGrapple = false;
			}
		}
	}

	void FixedUpdate(){
		// handles physics for the grapple
		if(IsEnabled){
			UpdateGrapple();
		}
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
				grapple.SetActive (true);

				grapple.transform.position = hit.point;
				
				joint.autoConfigureDistance = false;
				joint.maxDistanceOnly = true;
				joint.enabled = true;
				
				joint.connectedBody = grapple.GetComponent<Rigidbody2D>();
				joint.distance = Vector2.Distance(transform.position, grapple.transform.position);
				if(joint.distance < 3)
					joint.distance = 3;

				UpdateLineDrawing();
				ropeLine.gameObject.SetActive(true);
			}
		}
	}

	private bool isClockwise(){
		Vector2 vec = (grapple.transform.position - transform.position);
		vec = new Vector2(vec.y,-vec.x);
		if(Vector2.Dot(rb2d.velocity,vec) > 0){
			//counter
			return false;
		} else {
			//clock
			return true;
		}	
	}
	// updates various components in the grapple
	
	private void UpdateGrapple(){
		UpdateLineDrawing();

		Camera.main.GetComponent<CameraControl> ().SetPosition(rb2d.position);
		rb2d.velocity *= 0.995f;

		var hit = Physics2D.Linecast(transform.position, grapple.transform.position, mask1);
		
		/*-------------------------------------------------- ROPE WRAPING LOGIC --------------------------------------------------------*/
		// if you can see previous point then unroll back to that point
		if(unwindVectorsStack.Count > 0){		
			var vec0 = grapple.transform.position-transform.position;
			var vec = unwindVectorsStack.Peek();

			if(movingClockwiseStack.Peek() == true){
				Debug.DrawRay(transform.position,grapple.transform.position-transform.position,Color.red,1);
				if (vec0.y*vec.x > vec0.x*vec.y)	
					RemoveLastCollider();
			} else if(movingClockwiseStack.Peek() == false){
				Debug.DrawRay(transform.position,grapple.transform.position-transform.position,Color.green,1);
				if(vec0.y*vec.x < vec0.x*vec.y)
					RemoveLastCollider(); 
			}	
		}

		if (hit.collider.gameObject != grapple){
			// if you lose line of sight on the grappling hook, then add a new point to wrap around
			if(unwindVectorsStack.Count > 0){
				Vector2 vec0 = previousPointsStack.Peek()-(Vector2)grapple.transform.position;
				if(movingClockwiseStack.Peek() == true && isClockwise()){
					AddCollider(hit);
				} else if(movingClockwiseStack.Peek() == false &&  !isClockwise()){
					AddCollider(hit);
				}
			} else {
				AddCollider(hit);
			}
		}
		
		/*------------------------------------------------ ROPE CONTROLS ---------------------------------------------------*/
		// check to see if player let go of the grapple button or is touching the ground
		if(deployGrapple == false || player.GetComponentInChildren<GroundCheck>().ground){
			// jump off
			if (Input.GetKey (KeyCode.UpArrow))
				rb2d.velocity = new Vector2 (rb2d.velocity.x, 5);
			RetractRope();
			return;
		}
		
		//Swinging controls
		if (Input.GetKey (KeyCode.LeftArrow)) {
			rb2d.transform.localScale = new  Vector3(-1,1,transform.localScale.z);
			rb2d.AddForce (Vector2.right * (-pushForce / 20));
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			rb2d.transform.localScale = new  Vector3(1,1,transform.localScale.z);
			rb2d.AddForce (Vector2.right * (pushForce / 20));
		}
		
		// allow the rope to retract and extend as to certain lengths
		if (joint.distance > .5f) {
			var prevDistance = joint.distance;
			if (Input.GetKey (KeyCode.UpArrow)) {
				joint.distance -= step;
				animator.SetBool ("climbing", true);
				var changeInVelocity = (2*Mathf.PI*Mathf.Sqrt(prevDistance/3)) / (2*Mathf.PI*Mathf.Sqrt(joint.distance/3));
				rb2d.velocity = new Vector2(rb2d.velocity.x*changeInVelocity,rb2d.velocity.y);
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				joint.distance += step;
				animator.SetBool ("climbing", true);
				var changeInVelocity = (2*Mathf.PI*Mathf.Sqrt(prevDistance/3)) / (2*Mathf.PI*Mathf.Sqrt(joint.distance/3));
				rb2d.velocity = new Vector2(rb2d.velocity.x*changeInVelocity,rb2d.velocity.y);
			} else {
				animator.SetBool ("climbing", false);
			}
		}
	}

	// destroy the rope and all the grapplePoints
	private void RetractRope(){
		animator.SetBool ("rope", false);
		deployGrapple = false;
		joint.enabled = false;
		ropeLine.gameObject.SetActive(false);
		movingClockwiseStack.Clear();
		unwindVectorsStack.Clear();
		previousPointsStack.Clear();
		grapple.SetActive (false);
		player.GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Basic;
	}

	private void AddCollider(RaycastHit2D hit){
		if((hit.point-(Vector2)grapple.transform.position).magnitude > 0.5f){
			if(Vector2.Angle(transform.position-grapple.transform.position, (Vector2)grapple.transform.position-hit.point) > 10){
				hit.point = new Vector2(Mathf.Round(hit.point.x), Mathf.Round(hit.point.y));
				
				previousPointsStack.Push(grapple.transform.position);
				grapple.transform.position = hit.point;
				
				joint.distance = Vector2.Distance(transform.position, grapple.transform.position);
					
				movingClockwiseStack.Push(isClockwise());
				unwindVectorsStack.Push(previousPointsStack.Peek()-(Vector2)grapple.transform.position);
			}
		}
	}

	// remove the last collider and switch to the previous joint
	private void RemoveLastCollider(){
		if (previousPointsStack.Count > 0){
			movingClockwiseStack.Pop();
			unwindVectorsStack.Pop();
			
			joint.distance += Vector2.Distance(previousPointsStack.Peek(), grapple.transform.position);
			grapple.transform.position = previousPointsStack.Pop();
		}
	}

	// update the line representing the rope
	private void UpdateLineDrawing(){
		var totalPoints = previousPointsStack.Count+2;
		ropeLine.positionCount = totalPoints;	
		
		var count = 1;
		foreach(Vector2 point in previousPointsStack){
			ropeLine.SetPosition(previousPointsStack.Count-count, point);
			count++;
		}

		ropeLine.SetPosition(totalPoints-2, grapple.transform.position);
		ropeLine.SetPosition(totalPoints-1, transform.position);
	}

	public void setHasGrapple(bool hasGrapple){
		this.hasGrapple = hasGrapple;
		GlobalObject.Instance.playerData.SetHasGrapple (hasGrapple);
	}
}