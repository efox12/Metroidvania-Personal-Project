using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheifAttack : MonoBehaviour {
	private Animator animator;
	public int attackNumber;
	public float timer;
	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
		//timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.C)) {
			timer += Time.fixedDeltaTime;
			if (timer >= .2) {
				timer = 0;
				if (Input.GetKey (KeyCode.C) && Input.GetKey (KeyCode.DownArrow)
				    && GetComponentInChildren<GroundCheck> ().ground == false) {
					attackNumber = 4;
					animator.SetInteger ("attack", attackNumber);
				} else if (Input.GetKey (KeyCode.C)) {
					attackNumber = Random.Range (1, 4);
					animator.SetInteger ("attack", attackNumber);
				}
			} else {
				attackNumber = 0;
				animator.SetInteger ("attack", attackNumber);
			}
		} else {
			attackNumber = 0;
			animator.SetInteger ("attack", attackNumber);
		}
	}
}
