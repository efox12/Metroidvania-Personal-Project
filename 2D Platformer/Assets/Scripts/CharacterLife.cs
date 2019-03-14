using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterLife : MonoBehaviour {
	public int totalHearts;
	public int currentHealth;
	public bool invincibilityFrames;

	public GameObject heartPrefab;
	public GameObject healthUI;

	public Image damageImage;
	public Sprite fullHeart;
	public Sprite halfHeart;
	public Sprite emptyHeart;
	public bool hasKnockback;

	private List<GameObject> heartList = new List<GameObject> ();
	private int direction;
	private float knockback;
	public LayerMask playerMask;
	public LayerMask enemyMask;


	void Start () {
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
		invincibilityFrames = false;
		int pos = 0;
		for (int i = 0; i < totalHearts; i++) {
			GameObject heart = Instantiate (heartPrefab, healthUI.transform) as GameObject;
			heart.GetComponent<Image> ().rectTransform.position = new Vector3 (heart.transform.position.x + pos, heart.transform.position.y, 0);
			pos += 22;
			heartList.Add(heart);
		}
		/*if (PlayerData.Instance.playerHealthData [1] == 0) {
			PlayerData.Instance.playerHealthData[1] = 10;
			setHealth (PlayerData.Instance.playerHealthData[1]);
		} else*/
		setHealth (GlobalObject.Instance.playerData.playerHealthData[1]);
	}

	/*public void Save(){
		int[] savedStats = new int[2];
		savedStats [0] = totalHearts;
		savedStats [1] = currentHealth;
		PlayerData.playerHealthData = savedStats;
	}

	public void Load(){
		int[] loadedStats = PlayerData.playerHealthData;
		totalHearts = loadedStats [0];
		currentHealth = loadedStats [1];
	}*/

	void FixedUpdate(){
		if (hasKnockback) {
			hasKnockback = false;
			GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction * knockback, 6), ForceMode2D.Impulse);
		}
	}

	public void takeDamage(int hp, Vector2 damagePoint, float knockback){
		GlobalObject.Instance.playerData.playerHealthData [1] -= hp;
		//currentHealth -= hp;
		currentHealth = GlobalObject.Instance.playerData.playerHealthData [1];

		if (this.transform.position.x.CompareTo (damagePoint.x) >= 0)
			this.direction = 1;
		else
			this.direction = -1;
		this.knockback = knockback;
		this.hasKnockback = true;

		setHealth (currentHealth);
		StartCoroutine (FlashDamage (GetComponent<SpriteRenderer> (), 8, .1f));
	}

	public void setHealth(int currentHealth){
		for (int i = 0; i < heartList.Count; i++) {
			if (currentHealth >= 2) {
				currentHealth -= 2;
				heartList [i].GetComponent<Image>().sprite = fullHeart;
			} else if (currentHealth >= 1) {
				currentHealth -= 1;
				heartList [i].GetComponent<Image>().sprite = halfHeart;
			} else {
				heartList [i].GetComponent<Image>().sprite = emptyHeart;
			}
		}
	}
	IEnumerator FlashDamage (SpriteRenderer sprite, int numberOfTimes, float delay){
		invincibilityFrames = true;
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
		for (int i = 0; i < numberOfTimes; i++) {
			sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, .5f);
			yield return new WaitForSeconds(delay);
			sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, 1f);
			yield return new WaitForSeconds(delay);
		}
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
		invincibilityFrames = false;
	}
}