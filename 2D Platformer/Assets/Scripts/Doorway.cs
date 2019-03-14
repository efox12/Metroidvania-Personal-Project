using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour {
	
	public GameObject player;
	public GameObject connectedDoor;
	public bool inDoorway;
	public bool teleported;
	void Start(){
		//p = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(inDoorway && Input.GetKeyDown(KeyCode.DownArrow)){
			player.transform.position = connectedDoor.transform.position;
			Camera.main.transform.position = new Vector3(connectedDoor.transform.position.x, 
				connectedDoor.transform.position.y, Camera.main.transform.position.z);
			//gameObject.GetComponentInParent<PlayerController> ().teleport = true;
			teleported = true;
		}
	}
		
	void OnTriggerEnter2D(Collider2D col){
		inDoorway = true;
		//p. = true;
	}

	void OnTriggerStay2D(Collider2D col){
		inDoorway = true;
		//p.grounded = true;
	}

	void OnTriggerExit2D(Collider2D col){
		inDoorway = false;
		//p.grounded = false;
	}
}

