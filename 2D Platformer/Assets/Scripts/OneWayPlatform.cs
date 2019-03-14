using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour {
	public bool oneWay;
	BoxCollider2D bc2D;
	// Use this for initialization
	void Start () {
		bc2D =  gameObject.GetComponentsInParent<BoxCollider2D>()[1];;
	}
	
	void OnTriggerEnter2D(Collider2D col){
		oneWay = true;
		bc2D.enabled = false;
	}

	void OnTriggerStay2D(Collider2D col){
		oneWay = true;
		bc2D.enabled = false;
	}

	void OnTriggerExit2D(Collider2D col){
		oneWay = false;
		bc2D.enabled = true;
	}
}
