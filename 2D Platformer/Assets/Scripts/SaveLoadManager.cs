using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager {
	public static void savePlayer(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream stream = new FileStream (Application.persistentDataPath + "/playerData.sav", FileMode.Create);
		if (File.Exists (Application.persistentDataPath + "/playerData.sav")) {
			Debug.Log ("file");
		} else {
			Debug.Log ("no file");
		}
		bf.Serialize (stream, GlobalObject.Instance.playerData);
		stream.Close();
	}

	public static void LoadPlayer(){
		if (File.Exists (Application.persistentDataPath + "/playerData.sav")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream stream = new FileStream (Application.persistentDataPath + "/playerData.sav", FileMode.Open);
			GlobalObject.Instance.playerData = (PlayerData) bf.Deserialize (stream);
			Debug.Log (GlobalObject.Instance.playerData.playerHealthData[1]);
			stream.Close ();
		} else {
			Debug.Log ("File does not exist");
		}
	}
}
