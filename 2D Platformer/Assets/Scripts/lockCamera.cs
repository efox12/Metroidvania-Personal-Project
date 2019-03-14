using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockCamera : MonoBehaviour {

	private BoxCollider2D cameraBox;
	private Transform player;
	public LayerMask mask;
	public bool colliding;
	public Vector3 cameraSize;
	public bool theif;
	private float height;
	private float width;
	public Vector3 offset;
	private Vector3 velocity = Vector3.zero;
	public float smoothTime;
	public float leftBound;
	public float rightBound;
	public float topBound;
	public float bottomBound;
	private bool leftBoundSet;
	private bool rightBoundSet;
	private bool bottomBoundSet;
	private bool topBoundSet;
	// Use this for initialization
	void Start () {
		height = 2f * Camera.main.orthographicSize;
		width = height * Camera.main.aspect;
		cameraBox = GetComponent<BoxCollider2D> ();
		cameraBox.size = new Vector2 (width, height);
		//player = GameObject.Find("Player").GetComponent<SwitchCharacter> ();
		//theif = GameObject.Find("Theif").GetComponent<Transform> ();
		//wizard = GameObject.Find("Wizard").GetComponent<Transform> ();
		if (theif == true) {
			player = GameObject.Find("Theif").GetComponent<Transform> ();
		} else {
			player = GameObject.Find("Wizard").GetComponent<Transform> ();
		}


		cameraSize = cameraBox.size;
		bottomBoundSet = false;
		topBoundSet = false;
		rightBoundSet = false;
		leftBoundSet = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		height = 2f * Camera.main.orthographicSize;
		width = height * Camera.main.aspect;
		cameraBox.size = new Vector2 (width, height);
		cameraSize = cameraBox.size;

		followPlayer ();
	}
	void followPlayer(){
		if (theif == true) {
			player = GameObject.Find("Theif").GetComponent<Transform> ();
		}else{
			//player = GameObject.Find("Wizard").GetComponent<Transform> ();
		}
		
		Vector2 playerPosition = new Vector2(player.position.x, player.position.y);
		RaycastHit2D leftCast = Physics2D.Raycast (transform.position, Vector2.left, cameraBox.size.x/2, mask);
		RaycastHit2D rightCast = Physics2D.Raycast (transform.position, Vector2.right, cameraBox.size.x/2, mask);
		RaycastHit2D upCast = Physics2D.Raycast (transform.position, Vector2.up, cameraBox.size.y/2, mask);
		RaycastHit2D downCast = Physics2D.Raycast (transform.position, Vector2.down, cameraBox.size.y/2, mask);
		Vector3 targetPosition;
		float distance = (player.position + transform.position).magnitude;

		if (leftCast.collider == null && rightCast.collider == null) {
			//targetPosition = new Vector3 (player.position.x, player.position.y, transform.position.z);
			transform.position = Vector3.SmoothDamp(transform.position, 
				new Vector3 (player.position.x, player.position.y, transform.position.z), ref velocity, smoothTime/distance);
		}
		//Vector3 targetPosition = Vector3.SmoothDamp(transform.position, 
			//new Vector3 (player.position.x, player.position.y, transform.position.z), ref velocity, smoothTime);
		else {
			if (leftCast.collider != null) {
				//if (leftBoundSet == false) {
					//leftBoundSet = true;
					//leftBound = transform.position.x;
				transform.position = Vector3.SmoothDamp(transform.position, 
					new Vector3 (transform.position.x, player.position.y, transform.position.z), ref velocity, smoothTime/distance);
				if(player.position.x > transform.position.x){
					transform.position = Vector3.SmoothDamp(transform.position, 
						new Vector3 (player.position.x, player.position.y, transform.position.z), ref velocity, smoothTime/distance);
				}
					//targetPosition = new Vector3 (transform.position.x, player.position.y, transform.position.z);
				//}
				//leftBound = leftCast.point.x + cameraBox.size.x/2;
				//rightBound = playerPosition.x+1;
			} else {
				//leftBoundSet = false;
				//leftBound = player.position.x - 1;
				//targetPosition = new Vector3 (player.position.x, player.position.y, transform.position.z);
			}
			if (rightCast.collider != null) {
				//if (rightBoundSet == false) {
					//rightBoundSet = true;
					//rightBound = transform.position.x;
					//targetPosition = new Vector3 (transform.position.x, player.position.y, transform.position.z);
				//}
				//rightBound = rightCast.point.x - cameraBox.size.x/2;
				//leftBound = player.position.x-1;
				transform.position = Vector3.SmoothDamp(transform.position, 
					new Vector3 (transform.position.x, player.position.y, transform.position.z), ref velocity, smoothTime/distance);
				if(player.position.x < transform.position.x){
					transform.position = Vector3.SmoothDamp(transform.position, 
						new Vector3 (player.position.x, player.position.y, transform.position.z), ref velocity, smoothTime/distance);
				}
			} else {
				//rightBoundSet = false;
				//rightBound = transform.position.x + 1;
				//targetPosition = new Vector3 (player.position.x, player.position.y, transform.position.z);
			}
			bottomBound = player.position.y - 1;
			topBound = player.position.y + 1;

			//transform.position = new Vector3(Mathf.Clamp(targetPosition.x, leftBound, rightBound), 
				//Mathf.Clamp(targetPosition.y, bottomBound, topBound), targetPosition.z);
		}
		/*
		if (downCast.collider != null) {
			//Debug.DrawLine (transform.position, downCast.point);
			if (bottomBoundSet == false) {
				bottomBoundSet = true;
				bottomBound = transform.position.y;
			}
			//bottomBound = downCast.point.y + cameraBox.size.y/2;
		} else {
			bottomBoundSet = false;
			bottomBound = player.position.y;
		}
		if (upCast.collider != null) {
			if (topBoundSet == false) {
				topBoundSet = true;
				//Debug.DrawLine (transform.position, upCast.point);
				topBound = transform.position.y;
			}
			//topBound = upCast.point.y - cameraBox.size.y/2;
		} else {
			topBoundSet = false;
			topBound = player.position.y;
		}
		//}
		*/
		//xPosition = 



	}
}
