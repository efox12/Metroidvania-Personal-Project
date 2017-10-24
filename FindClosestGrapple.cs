using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestGrapple : MonoBehaviour {
	public float distance;
	public Collider2D[] grappleOverlap = new Collider2D[5];
	public LayerMask mask;
	public LayerMask mask1;
	public List<float> lengthList = new List<float> ();
	public Dictionary<float, RaycastHit2D> positionList = new Dictionary<float, RaycastHit2D> ();
	public Vector3 grapplePoint;

	// Use this for initialization
	public bool validPoint;
	void Start () {
		validPoint = false;
	}
	
	// Update is called once per frame
	void Update () {
		CheckForGrapplePoints();
	}

	void CheckForGrapplePoints(){
		// create a list of five empty grapple points
		for (int i = 0; i < 5; i++) {
			grappleOverlap [i] = null;
		}

		validPoint = false;

		// clear the lists
		positionList.Clear ();
		lengthList.Clear ();

		GetComponent<GrappleHook> ().grappled = false;
		// checks to see if there are grapples in range
		if (Physics2D.OverlapCircleNonAlloc (transform.position, distance, grappleOverlap, mask) > 0) {
			// gets the number of grapple points within the given distance
			int times = Physics2D.OverlapCircleNonAlloc (transform.position, distance, grappleOverlap, mask);
		
			for (int i = 0; i < times; i++) {
				Vector3 position = grappleOverlap [i].attachedRigidbody.transform.position;
				var hit = Physics2D.Linecast (transform.position, position, mask1);
				// find the position and the distance to the grapple point
				if (hit.collider == grappleOverlap [i]) {
					position = hit.point;
					float length = Vector2.Distance (transform.position, position);
					
					// adds the grapple points to the lists
					if ((transform.position.x - position.x) * transform.localScale.x < 0) {
						validPoint = true;
						lengthList.Add (length);
						positionList.Add (length, hit);
					}
				}
			}

			// sort the list of lengths
			lengthList.Sort ();
			Vector3 point;
			RaycastHit2D grapple;

			if (validPoint == true) {
				// get the value in the position list using the shortest length in the lenght list
				if (positionList.TryGetValue (lengthList [0], out grapple)) {
					grapplePoint = grapple.point;
					GetComponent<GrappleHook> ().grapplePoint = grapplePoint;
					Debug.DrawLine (transform.position, grapple.point);
					GetComponent<GrappleHook> ().grappled = true;
				} 
			}
		}
	}
}
