using UnityEngine;
using System.Collections;

public class fungus : MonoBehaviour {
	public Sprite fungusSprite;

	void Start () {

	}
	
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name.Equals("Player")) {
			GetComponent<SpriteRenderer>().sprite = fungusSprite;
		}
	}
}
