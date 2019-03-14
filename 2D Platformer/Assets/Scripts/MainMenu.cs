using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		GlobalObject.Instance.playerData.playerHealthData [1] = 10;
	}
	public void LoadGame(){
		SaveLoadManager.LoadPlayer ();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
