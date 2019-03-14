using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBackpack : MonoBehaviour {
	Rigidbody2D rb;
	// Use this for initialization
	float timeHeld = 0;
	bool isRocketing = false;
	public float cooldownTime = 1;
	float timePassed = 0;
	void Start () {
		rb = GetComponent <Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.GetComponent<CameraControl> ().SetPosition(rb.position);
		if (Input.GetKeyDown(KeyCode.V) && isRocketing == false && timePassed > cooldownTime){
			timePassed = 0;
			rb.velocity = new Vector2(transform.localScale.x*30,5);
			isRocketing = true;
			timeHeld = 0;
		} else {
			timePassed += Time.fixedDeltaTime;
		}
		
		rocketing();
	}

	public void rocketing(){
		if(isRocketing){
			if(timeHeld < .1f){
				GetComponent<PlayerController> ().velocityControlled = false;
				rb.velocity = new Vector2(transform.localScale.x*30,rb.velocity.y);
				timeHeld += Time.fixedDeltaTime;
			} else{
				isRocketing = false;
				GetComponent<PlayerController> ().velocityControlled = true;
			}
		}
	}
}
