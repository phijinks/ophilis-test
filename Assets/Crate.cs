﻿using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {
	private GameObject player;
	Animator anim;
	AudioSource au;
	int stage = 1; // how damaged the crate is
	float size = 2.3f; // size of the crate

	void Start () {
		anim = GetComponent<Animator>();
		au = GetComponent<AudioSource>();
		player = GameObject.Find("Player");
		transform.localScale = new Vector3 (size, size, 1);
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			Vector3 p = player.transform.position;
			Vector3 my = transform.position;

			if(Mathf.Abs(p.x - my.x) < 5 && Mathf.Abs(p.y - my.y) < 3 && stage <= 3) {
				//Break();
			}
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if(other.gameObject.name.Contains("Projectile")) {
			Vector3 pos = other.gameObject.transform.position;
			float dist = (transform.position - pos).magnitude;
			if(dist <= 4) {Break();}
		}
	}

	void Break() {
		//au.Play();

		if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.5) {return;}

		Vector3 p = player.transform.position;
		Vector3 my = transform.position;

		// flip sprite based on what direction it was hit from
		float flip = (p.x < my.x ? 1f : -1f);
		transform.localScale = new Vector3(flip*size, size, 1);
		
		anim.Play("Impact_" + stage); // play corresponding hit animation
		if(stage == 3) { // crate is destroyed
			Destroy(GetComponent<Collider2D>()); // destroy the collider so the player can walk through it
		}
		stage++; // go to next hit animation
	}
}
