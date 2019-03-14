using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GlobalObject : MonoBehaviour {
	public static GlobalObject Instance;
	public PlayerData playerData;
	public int[] playerPositionData;
	public int[] playerHealthData;
	public bool hasGrapple;
	// Use this for initialization
	void Start () {
		//player = GameObject.FindGameObjectWithTag("Player");
		playerData = new PlayerData();
	}
	
	void Awake(){
		if (Instance == null)
		{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy (gameObject);
		}
	}
		

	/*public void Save(){
		SaveLoadManager.savePlayer ();
	}

	public void Load(){
		SaveLoadManager.LoadPlayer ();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}*/
	//public void setHealth(){
		//player.GetComponent<CharacterLife> ().currentHealth = playerHealthData [1];
	//}
}

[System.Serializable]
public class PlayerData{
	public int[] playerPositionData;
	public int[] playerHealthData;
	public bool hasGrapple;
	public PlayerData(){
		playerHealthData = new int[2];
		playerPositionData = new int[3];
		//hasGrapple = new bool();
	}
	public bool GetHasGrapple(){
		return hasGrapple;
	}
	public void SetHasGrapple(bool hasGrapple){
		this.hasGrapple = hasGrapple;
	}
}
