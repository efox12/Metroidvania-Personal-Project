using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chest : MonoBehaviour {
	public GameObject item;
	public LayerMask ChestLayer;
	public LayerMask Playerlayer;
	public bool closed;
	public GameObject player;
	 
	public Tilemap tileMap;
	public TileBase highlightTile1;
	public TileBase highlightTile2;
	public TileBase chestTile1;
	public TileBase chestTile2;
	public TileBase openChestTile1;
	public TileBase openChestTile2;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		closed = true;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnTriggerEnter2D(Collider2D col){
		if (closed) {
			if (col.gameObject.Equals (player)) {
				highlightChest (this.gameObject.transform.position);
				if (Input.GetKey (KeyCode.C)) {
					closed = false;
					openChest (this.gameObject.transform.position);
					//GameObject.FindGameObjectWithTag ("ItemInfo").SetActive (true);
					player.GetComponent<GrappleHook> ().setHasGrapple(true);
					//Instantiate (item, player.transform);
				}
			}
		}
	}
	public void OnTriggerStay2D(Collider2D col){
		if (closed) {
			if (col.gameObject.Equals (player)) {
				highlightChest (this.gameObject.transform.position);
				if (Input.GetKey (KeyCode.C)) {
					closed = false;
					openChest (this.gameObject.transform.position);
					//GameObject.FindGameObjectWithTag ("ItemInfo").SetActive (true);
					player.GetComponent<GrappleHook> ().setHasGrapple(true);
					//Instantiate (item, player.transform);
				}
			}
		}
	}

	public void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.Equals (player)) {
			unHighlightChest (this.gameObject.transform.position);
		}
	}

	public void highlightChest(Vector2 position){
		if (tileMap.HasTile (tileMap.WorldToCell (new Vector2 (position.x + 1, position.y)))) {
			tileMap.SetTile (tileMap.WorldToCell (position), highlightTile1);
			tileMap.SetTile (tileMap.WorldToCell (new Vector2 (position.x + 1, position.y)), highlightTile2);
		} else {
			tileMap.SetTile (tileMap.WorldToCell (position), highlightTile2);
			tileMap.SetTile (tileMap.WorldToCell (new Vector2 (position.x - 1, position.y)), highlightTile1);
		}

	}
	public void unHighlightChest(Vector2 position){
		if (tileMap.HasTile (tileMap.WorldToCell (new Vector2 (position.x + 1, position.y)))) {
			tileMap.SetTile (tileMap.WorldToCell (position), chestTile1);
			tileMap.SetTile (tileMap.WorldToCell (new Vector2 (position.x + 1, position.y)), chestTile2);
		} else {
			tileMap.SetTile (tileMap.WorldToCell (position), chestTile2);
			tileMap.SetTile (tileMap.WorldToCell (new Vector2 (position.x - 1, position.y)), chestTile1);
		}

	}
	public void openChest(Vector2 position){
		if (tileMap.HasTile (tileMap.WorldToCell (new Vector2 (position.x + 1, position.y)))) {
			tileMap.SetTile (tileMap.WorldToCell (position), openChestTile1);
			tileMap.SetTile (tileMap.WorldToCell (new Vector2 (position.x + 1, position.y)), openChestTile2);
		} else {
			tileMap.SetTile (tileMap.WorldToCell (position), openChestTile2);
			tileMap.SetTile (tileMap.WorldToCell (new Vector2 (position.x - 1, position.y)), openChestTile1);
		}

	}
}
