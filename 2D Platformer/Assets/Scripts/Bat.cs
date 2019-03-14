using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy {
	public enum enemyState{Default,Guard,Attack,SeeSomething};
	public int currentState;
	// Use this for initialization
	void Start () {
		health = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentState == (int)enemyState.Attack) {
			AttackState ();
		} else if (currentState == (int)enemyState.SeeSomething) {
			SeeSomething ();
		} else if (currentState == (int)enemyState.Default) {
			DefaultState ();
		} else {
			DefaultState ();
		}
	}
	void DefaultState ()
	{
	}

	void SeeSomething ()
	{
	}

	void AttackState ()
	{
	}
}
