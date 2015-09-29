using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	Animator anim;
	SpriteRenderer sprite;
	public bool canGrowFungus = true;

	void Start () {
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
	}
	
	void Update () {

	}

	void OnTriggerStay2D(Collider2D other) {
		// if the block touches the player's foot collider, play the fungus growing animation
		string c = other.gameObject.name;
		if((c.Equals("Player") || c.Contains("Projectile")) && anim != null) {
			anim.Play("Fungus");
			sprite.sortingLayerName = "Fungus"; // put the sprite in front of the non-fungus-covered blocks
		}
	}
}
