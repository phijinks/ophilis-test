using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	Animator anim;
	SpriteRenderer sprite;

	void Start () {
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
	}
	
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		// if the block touches the player's foot collider, play the fungus growing animation
		if(other.gameObject.name.Equals("Player") && anim != null) {
			anim.Play("Fungus");
			sprite.sortingLayerName = "Fungus"; // put the sprite in front of the non-fungus-covered blocks
		}
	}
}
