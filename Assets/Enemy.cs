using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	Animator anim;
	int health = 2; // how damaged the enemy is

	void Start () {
		anim = GetComponent<Animator>();
	}
	
	void Update () {
		AnimatorStateInfo info =  anim.GetCurrentAnimatorStateInfo(0);
		if(info.normalizedTime >= 1 && info.IsName("Hit")) {
			anim.Play("Normal");
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.name.Contains("Projectile")) {
			Break();
		}
	}

	void Break() {
		anim.Play("Hit"); // play corresponding hit animation
		if(health <= 0) { // the grim reaper comes for us all
			anim.Play("Die");
			Destroy(this.gameObject, 2f);
			Destroy(GetComponent<Collider2D>()); // destroy the collider
		}
		health--; // take some damage
	}
}
