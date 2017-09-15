using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour {
	public float Length = 4;

	[HideInInspector]
	public bool IsEnabled
	{
		get { return _points.Count > 0; }
	}

	private readonly List<GameObject> _points = new List<GameObject>();
	public LineRenderer _line;
	GameObject _grapple;
	GameObject _previousGrapple;
	private DistanceJoint2D _joint;
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

		_grapple = new GameObject("Grapple");
		_grapple.AddComponent<CircleCollider2D>().radius = .01f;
		_grapple.AddComponent<Rigidbody2D>();
		_grapple.GetComponent<Rigidbody2D> ().isKinematic = true;
		//_grapple.layer = mask;
		_grapple.SetActive (false);

		_previousGrapple = (GameObject)Instantiate(_grapple);
		_previousGrapple.name = "Previous Grapple";

		_joint = gameObject.AddComponent<DistanceJoint2D>();
		_joint.enabled = false;
		grappled = false;
		deployGrapple = false;
	}
	void Update(){
		if (Input.GetKey (KeyCode.Z) && GetComponent<PlayerController> ().grounded == false) {
			deployGrapple = true;
			//grappled = true;
		//} //else if (Input.GetKeyDown (KeyCode.Z) && grappled == true) {
			//deployGrapple = false;
			//grappled = true;
		} else {
			deployGrapple = false;
			//grappled = false;
		}

	}

	void FixedUpdate(){
		if (IsEnabled) UpdateGrapple();
		else CheckForGrapple();
	}

	private void CheckForGrapple(){
		if(deployGrapple == true){
		//if (Input.GetKey(KeyCode.Z) && _joint.enabled == false){
			//grappled = false;
			//var mousePosition = Input.mousePosition;
			//mousePosition.z = -Camera.main.transform.position.z;
			//var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
			//var grapplePoint = transform.position + (worldPosition - transform.position) * Length;

			var hit = Physics2D.Linecast(transform.position, grapplePoint, mask);
			var distance = Vector3.Distance(transform.position, hit.point);
			if (hit.collider != null && distance <= Length){
				//TheifStateMachine;
				GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Grapple;
				//rb2d.drag = 1;
				animator.SetBool ("rope", true);
				_line.positionCount = 2;
				_line.SetPosition(0, hit.point);
				//_line.SetPosition(0, grapplePoint);
				_line.SetPosition(1, transform.position);
				_line.gameObject.SetActive(true);

				_points.Add(CreateGrapplePoint(hit));

				_grapple.SetActive (true);
				Debug.Log(_previousGrapple.transform.position);
				_previousGrapple.SetActive (true);
				_grapple.transform.position = hit.point;
				_previousGrapple.transform.position = hit.point;
				//_grapple.transform.position = grapplePoint;
				//_previousGrapple.transform.position = grapplePoint;
				SetParent(_grapple.transform, hit.collider.transform);
				//Debug.Log(_previousGrapple.transform.position);
				//Debug.Log(hit.point);
				_joint.enabled = true;
				_joint.connectedBody = _grapple.GetComponent<Rigidbody2D>();
				_joint.distance = Vector2.Distance(hit.point, transform.position);
				//_joint.distance = Vector2.Distance(grapplePoint, transform.position);
				//}
			}
		}
	}

	private GameObject CreateGrapplePoint(RaycastHit2D hit){
		var p = new GameObject("GrapplePoint");
		SetParent(p.transform, hit.collider.transform);
		p.transform.position = hit.point;
		return p;
	}

	private void UpdateGrapple(){
		rb2d.drag = .5f;
		rb2d.gravityScale = 3;
		UpdateLineDrawing();

		var hit = Physics2D.Linecast(transform.position, _grapple.transform.position, mask1);
		var hitPrev = Physics2D.Linecast(transform.position, _previousGrapple.transform.position, mask1);

		if (hit.collider.gameObject != _grapple && hit.collider.gameObject != _previousGrapple){
			// if you lose line of sight on the grappling hook, then add a new point to wrap around

			_points.Add(CreateGrapplePoint(hit));

			UpdateLineDrawing();

			_previousGrapple.transform.position = _grapple.transform.position;
			SetParent(_previousGrapple.transform, _grapple.transform.parent);
			_grapple.transform.position = hit.point;
			SetParent(_grapple.transform, hit.collider.transform);
			_joint.distance = Vector2.Distance(transform.position, _grapple.transform.position);

		} else if (Vector2.Distance(_grapple.transform.position, _previousGrapple.transform.position) <= .1f){
			RemoveLastCollider();
		}
		if(deployGrapple == false){

			// jump off
			if (Input.GetKey (KeyCode.UpArrow) && transform.position.y < _grapple.transform.position.y)
				rb2d.velocity = new Vector2 (rb2d.velocity.x, 5);

			RetractRope();
		}
		_line.SetPosition(_points.Count, transform.position);
			if (Input.GetKey (KeyCode.LeftArrow)) {
				rb2d.transform.localScale = new Vector2 (-1, 1);
				rb2d.AddForce (Vector2.right * (-pushForce / 20));
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				rb2d.transform.localScale = new Vector2 (1, 1);
				rb2d.AddForce (Vector2.right * (pushForce / 20));
			} 

		if (_joint.distance > .5) {
			if (Input.GetKey (KeyCode.UpArrow)) {
				_joint.distance -= step;
				animator.SetBool ("climbing", true);

			} else if (Input.GetKey (KeyCode.DownArrow)) {
				_joint.distance += step;
				animator.SetBool ("climbing", true);
			} else {
				animator.SetBool ("climbing", false);
			}
		}

		if (GetComponentInChildren<GroundCheck>().ground) {
			RetractRope();
		}

		// if you can see previous point then unroll back to that point
		if (hitPrev.collider != null && hitPrev.transform == _previousGrapple.transform)
			RemoveLastCollider();
	}

	private void RetractRope(){
		animator.SetBool ("rope", false);
		rb2d.drag = 0;
		rb2d.gravityScale = 2.5f;
		deployGrapple = false;
		_joint.enabled = false;
		_line.gameObject.SetActive(false);
		_points.ForEach(Destroy);
		_points.Clear();
		_grapple.SetActive (false);
		_previousGrapple.SetActive(false);
		GetComponent<TheifStateMachine> ().currentState = (int)TheifStateMachine.theifState.Basic;
	}

	private void RemoveLastCollider(){
		if (_points.Count > 1){
			Destroy(_points[_points.Count - 1]);
			_points.RemoveAt(_points.Count - 1);

			UpdateLineDrawing();
			_grapple.transform.position = _previousGrapple.transform.position;

			_joint.distance = Vector2.Distance(transform.position, _grapple.transform.position);

			SetParent(_grapple.transform, _previousGrapple.transform.parent);
		}

		if (_points.Count > 1)
			_previousGrapple.transform.position = _points [_points.Count - 2].transform.position;
		else {
			
			_previousGrapple.transform.position = _grapple.transform.position;
		}
	}

	private void UpdateLineDrawing(){
		_line.positionCount = _points.Count + 1;
		for (var i = 0; i < _points.Count; i++)
			_line.SetPosition(i, _points[i].transform.position);
		_line.SetPosition(_points.Count, transform.position);
	}

	private void SetParent(Transform child, Transform parent){
		child.SetParent(parent);
		if (parent != null)
			child.localScale = new Vector3(1 / parent.localScale.x, 1 / parent.localScale.y, 1 / parent.localScale.z);
	}
}
