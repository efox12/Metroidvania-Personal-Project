using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheifAttack : MonoBehaviour {
	private Animator animator;
	public int attackNumber;
	public float attackCoolDown;
	public float coolDownTime;
	
	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
		this.attackCoolDown = 0;
		this.coolDownTime = .25f;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateCoolDown();
		NormalAttack();
	}

	// updates the cooldown for swinging a sword
	public void UpdateCoolDown(){
		if(attackCoolDown > 0)
			attackCoolDown -= Time.fixedDeltaTime;
	}

	// sets the cooldown time (how quickly the sword can be swung)
	public void setCoolDownTime(float coolDownTime){
		this.coolDownTime = coolDownTime;
	}

	// gets the cooldown time (how quickly the sword can be swung)
	public float getCoolDownTime(){
		return this.coolDownTime;
	}
	public void NormalAttack(){
		// swing sword when the attack button is pressed
		if (Input.GetKeyDown (KeyCode.C) && attackCoolDown <= 0){

			// check to see if a downward strike 
			if (Input.GetKey (KeyCode.C) && Input.GetKey (KeyCode.DownArrow) && GetComponentInChildren<GroundCheck> ().ground == false) {
				attackNumber = 4;
				animator.SetInteger ("attack", attackNumber);

			// otherwise do a normal attack	
			} else{
				attackNumber = 1;
				animator.SetInteger ("attack", attackNumber);
				attackCoolDown = coolDownTime;
			}

		// keep swingins sword when the attack button is held	
		} else if (Input.GetKey (KeyCode.C) && attackCoolDown <= 0) {
			attackCoolDown = .25f;

			// check to see if a downward strike 
			if (Input.GetKey (KeyCode.C) && Input.GetKey (KeyCode.DownArrow) && GetComponentInChildren<GroundCheck> ().ground == false) {
				attackNumber = 4;
				animator.SetInteger ("attack", attackNumber);

			// otherwise do a normal attack		
			} else if (Input.GetKey (KeyCode.C)) {
				attackNumber = Random.Range (1, 4);
				animator.SetInteger ("attack", attackNumber);
			}

		// otherwise dont swing sword
		} else {
				attackNumber = 0;
				animator.SetInteger ("attack", attackNumber);
		} 
	}
}
