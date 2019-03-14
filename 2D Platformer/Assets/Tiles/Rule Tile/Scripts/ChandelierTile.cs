using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace UnityEngine {
	[Serializable]
	[CreateAssetMenu]

	public class GrapplePointTile : TileBase {
		public Sprite m_DefaultSprite;
		public Tile.ColliderType m_DefaultColliderType = Tile.ColliderType.None;
		//var grapplePoint : GrapplePoint;
		public override bool StartUp (Vector3Int position, ITilemap tilemap, GameObject go)
		{
			//Instantiate (Resources.Load ("GrapplePoint"), position, Quaternion.identity);
			CircleCollider2D col = go.gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
			col.radius = .01f;
			//col.
			return base.StartUp (position, tilemap, go);
		}
		public override void GetTileData(Vector3Int position, ITilemap tileMap, ref TileData tileData){
			tileData.sprite = m_DefaultSprite;
			tileData.colliderType = m_DefaultColliderType;
			tileData.flags = TileFlags.LockTransform;
			tileData.transform = Matrix4x4.identity;
		}

		public override void RefreshTile(Vector3Int location, ITilemap tileMap){
			base.RefreshTile(location, tileMap);
		}
	}
}
