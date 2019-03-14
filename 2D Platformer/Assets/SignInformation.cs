using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SignInformation : MonoBehaviour {
	public string description;
	public GameObject infoSign;
	// Use this for initialization
	void Start () {
		infoSign = GameObject.FindGameObjectWithTag ("InfoSign");
		infoSign.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		
	}
	void OnTriggerEnter2D (Collider2D col){
		infoSign.SetActive (true);
		infoSign.GetComponentInChildren<Text> ().text = description;
	}
	void OnTriggerStay2D(Collider2D col){
		infoSign.SetActive (true);
		infoSign.GetComponentInChildren<Text> ().text = description;
	}
	void OnTriggerExit2D(Collider2D col){
		infoSign.SetActive (false);
	}
}
