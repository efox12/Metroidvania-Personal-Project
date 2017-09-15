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
		for (int i = 0; i < 5; i++) {
			grappleOverlap [i] = null;
		}

		validPoint = false;
		positionList.Clear ();
		lengthList.Clear ();
		GetComponent<GrappleHook> ().grappled = false;

		if (Physics2D.OverlapCircleNonAlloc (transform.position, distance, grappleOverlap, mask) > 0) {
			
			int times = Physics2D.OverlapCircleNonAlloc (transform.position, distance, grappleOverlap, mask);

			for (int i = 0; i < times; i++) {
				Vector3 position = grappleOverlap [i].attachedRigidbody.transform.position;

				var hit = Physics2D.Linecast (transform.position, position, mask1);
				if (hit.collider == grappleOverlap [i]) {
					position = hit.point;
					float length = Vector2.Distance (transform.position, position);
					if ((transform.position.x - position.x) * transform.localScale.x < 0) {
						validPoint = true;
						lengthList.Add (length);
						positionList.Add (length, hit);
					}
				}
			}

			lengthList.Sort ();
			Vector3 point;
			RaycastHit2D grapple;
			if (validPoint == true) { 
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
