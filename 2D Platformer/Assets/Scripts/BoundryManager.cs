using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundryManager : MonoBehaviour {
	private BoxCollider2D managerBox;
	private Transform player;
	public  GameObject boundry;

	// Use this for initialization
	void Start () {
		managerBox = GetComponent<BoxCollider2D> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		ManageBoundry ();
	}

	void ManageBoundry(){
		if (managerBox.bounds.min.x < player.position.x && managerBox.bounds.min.y < player.position.y &&
		    managerBox.bounds.max.x > player.position.x && managerBox.bounds.max.y > player.position.y) {
			boundry.SetActive(true);
		} else {
			boundry.SetActive(false);
		}
	}
}
