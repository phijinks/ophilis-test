﻿using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {
	float dx = 0;
	float dy = 0;

	bool grounded = false;
	int totalCollisions;
	int footCollisions;

	void Start () {

	}
	
	void Update () {
		float accel = 0.05f;
		float maxSpeed = 0.5f;

		float friction = 0.6f;

		float jumpSpeed = 0.9f;
		float gravity = 0.04f;
		float terminalVelocity = 1f;

		float mult = 1f;
		if(!grounded) {mult = 0.6f;}

		if(Input.GetKey("a")) {
			if(dx > 0 && grounded) {mult *= 0.6f;}
			dx -= accel * mult;
			if(dx < 0 - maxSpeed) {
				dx = 0 - maxSpeed;
			}
		} else if(Input.GetKey("d")) {
			if(dx < 0 && grounded) {mult *= 0.6f;}
			dx += accel * mult;
			if(dx > maxSpeed) {
				dx = maxSpeed;
			}
		} else if(grounded) {
			dx *= friction;
		}

		GetComponent<Rigidbody2D>().MovePosition(transform.position + new Vector3(dx, dy, 0));

		//Debug.Log(grounded);
		if(!grounded || totalCollisions == 0) {
			dy -= gravity;
			if(gravity < 0 - terminalVelocity) {gravity = 0 - terminalVelocity;}
		} else if(grounded) {
			dy = 0;
			if(Input.GetKeyDown("w")) {
				dy = jumpSpeed;
				grounded = false;
			}
		}
	}

	public void MoveTo(float x, float y) {
		transform.position = new Vector3(x, y, 0);
	}

	void OnCollisionEnter2D(Collision2D c) {totalCollisions++;}
	void OnCollisionExit2D(Collision2D c) {totalCollisions--; if(totalCollisions < 0) {totalCollisions = 0;}}

	void OnTriggerEnter2D(Collider2D c) {
		if(dy <= 0) {
			footCollisions++;
			grounded = true;
		}
	}

	void OnTriggerExit2D(Collider2D c) {
		footCollisions--;
		if(footCollisions <= 0) {
			footCollisions = 0;
			grounded = false;
		}
	}
}
