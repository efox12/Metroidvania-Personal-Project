using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
	private string saveGameName = "current";

	public void QuitGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void SaveGame(){
		SaveLoadManager.savePlayer ();
	}
}
