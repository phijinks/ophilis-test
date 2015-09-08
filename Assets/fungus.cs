using UnityEngine;
using System.Collections;

public class fungus : MonoBehaviour {
	public Sprite fungusSprite;

	void Start () {

	}
	
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.name.Equals("Player")) {
			GetComponent<SpriteRenderer> ().sprite = fungusSprite;
		}
	}
}
