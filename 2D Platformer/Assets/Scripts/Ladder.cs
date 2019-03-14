using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {
	public float center;
	public bool climb;
	// Use this for initialization
	void Start () {
		center = transform.position.x;
	}

	void OnTriggerEnter2D (Collider2D col){
		climb = true;
		//col.GetComponent<LadderClimb> ().ladder = true;
		//col.GetComponent<LadderClimb> ().center = center;
	}
	void OnTriggerStay2D (Collider2D col){
		climb = true;
		//col.GetComponent<LadderClimb> ().ladder = true;
		//col.GetComponent<LadderClimb> ().center = center;
	}
	void OnTriggerExit2D(Collider2D col){
		climb = false;
		//col.GetComponent<LadderClimb> ().ladder = false;
	}
}
