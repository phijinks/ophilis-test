using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {
	float dx = 0;
	float dy = 0;

	bool grounded = false;
	ArrayList footCollisions;

	void Start () {
		footCollisions = new ArrayList();
	}
	
	void Update () {
		float accel = 0.1f;
		float maxSpeed = 0.5f;

		float friction = 0.6f;

		float jumpSpeed = 0.6f;
		float gravity = 0.03f;
		float terminalVelocity = 1f;

		float mult = 1f;
		if(!grounded) {mult = 0.2f;}

		if(Input.GetKey("a")) {
			dx -= accel * mult;
			if(dx < 0 - maxSpeed) {
				dx = 0 - maxSpeed;
			}
		} else if(Input.GetKey("d")) {
			dx += accel * mult;
			if(dx > maxSpeed) {
				dx = maxSpeed;
			}
		} else if(grounded) {
			dx *= friction;
		}

		GetComponent<Rigidbody2D>().MovePosition(transform.position + new Vector3(dx, dy, 0));

		Debug.Log(grounded);
		if(!grounded) {
			dy -= gravity;
			if(gravity < 0 - terminalVelocity) {gravity = 0 - terminalVelocity;}
		} else {
			dy = 0;
			if(Input.GetKeyDown("w")) {
				dy = jumpSpeed;
				grounded = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D c) {
		if(dy <= 0) {
			footCollisions.Add(c);
			grounded = true;
		}
	}

	void OnTriggerExit2D(Collider2D c) {
		if(dy <= 0) {
			footCollisions.Remove(c);
			if(footCollisions.Count == 0) {
				grounded = false;
			}
		}
	}
}
