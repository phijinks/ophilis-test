using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	// delta x and y
	float dx = 0;
	float dy = 0;

	bool grounded = false; // whether the player is on the ground
	int totalCollisions; // number of collisions
	int footCollisions; // number of collisions with the bottom of the collider

	void Start () {

	}
	
	void Update () {
		// constants for platforming code
		const float accel = 0.08f;
		const float maxSpeed = 1.0f;

		const float friction = 0.6f;

		const float jumpSpeed = 0.9f;
		const float gravity = 0.04f;
		const float terminalVelocity = 1f;

		float mult = 1f; // acceleration multiplier
		if(!grounded) {mult = 0.6f;} // player has less control in the air

		if(Input.GetKey("a")) {
			if(dx > 0 && grounded) {mult *= 0.6f;} // skid
			dx -= accel * mult;
			if(dx < 0 - maxSpeed) {
				dx = 0 - maxSpeed;
			}
		} else if(Input.GetKey("d")) {
			if(dx < 0 && grounded) {mult *= 0.6f;} // skid
			dx += accel * mult;
			if(dx > maxSpeed) {
				dx = maxSpeed;
			}
		} else if(grounded) {
			dx *= friction;
		}

		// flip sprite based on x speed
		if(dx < 0) {
			transform.localScale = new Vector3(-1, 1, 1);
		} else {
			transform.localScale = new Vector3(1, 1, 1);
		}

		GetComponent<Rigidbody2D>().MovePosition(transform.position + new Vector3(dx, dy, 0));

		if(!grounded || totalCollisions == 0) {
			dy -= gravity;
			if(dy < 0 - terminalVelocity) {dy = 0 - terminalVelocity;}
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
