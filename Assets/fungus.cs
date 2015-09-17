using UnityEngine;
using System.Collections;

public class fungus : MonoBehaviour {
	Animator anim;
	SpriteRenderer sprite;

	void Start () {
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
	}
	
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.name.Equals("Player")) {
			anim.Play("Fungus");
			sprite.sortingLayerName = "Fungus";
		}
	}
}
