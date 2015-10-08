using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	Animator anim;
	SpriteRenderer sprite;
	private GameObject counter;
	bool canGrowFungus = false;
	bool hasFungus = false;

	void Start () {
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
		counter = GameObject.Find("FungusCounter");
	}
	
	void Update () {

	}

	void SetFloor() {
		canGrowFungus = true;
		if (counter != null) {counter.BroadcastMessage("IncrementTotal");}
	}

	void OnTriggerStay2D(Collider2D other) {
		// if the block touches the player's foot collider, play the fungus growing animation
		string c = other.gameObject.name;
		if(canGrowFungus && (c.Equals("Player") || c.Contains("Projectile")) && anim != null) {
			if(!hasFungus) {
				hasFungus = true;
				anim.Play("Fungus");
				sprite.sortingLayerName = "Fungus"; // put the sprite in front of the non-fungus-covered blocks
				counter.BroadcastMessage("Increment");
			}
		}
	}
}
