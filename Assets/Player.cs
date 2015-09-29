using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public Transform projectile;

	// delta x and y
	float dx = 0;
	float dy = 0;

	float grace = 0; // grace period during which you can jump after leaving a platform

	bool grounded = false; // whether the player is on the ground
	int jumpsLeft = 0; // number of jumps the player has left
	int numJumps = 2; // number of jumps the player can make (2 = double jump, etc.)
	int totalCollisions; // number of collisions
	int footCollisions; // number of collisions with the bottom of the collider

	Animator anim;
	bool attacked = false;

	void Start () {
		anim = GetComponent<Animator>();
	}
	
	void Update () {
		// constants for platforming code
		const float accel = 0.08f;
		const float maxSpeed = 1.0f;

		const float friction = 0.6f;

		const float jumpSpeed = 1.2f;
		const float gravity = 0.05f;
		const float terminalVelocity = 1f;

		float mult = 1f; // acceleration multiplier
		if (!grounded) {
			mult = 0.6f;
		} // player has less control in the air

		if (Input.GetKey ("a")) {
			if (dx > 0 && grounded) {
				mult *= 0.6f;
			} // skid
			dx -= accel * mult;
			if (dx < 0 - maxSpeed) {
				dx = 0 - maxSpeed;
			}
		} else if (Input.GetKey ("d")) {
			if (dx < 0 && grounded) {
				mult *= 0.6f;
			} // skid
			dx += accel * mult;
			if (dx > maxSpeed) {
				dx = maxSpeed;
			}
		} else if (grounded) {
			dx *= friction;
		}

		bool running = (Mathf.Abs(dx) > 0.4);

		// flip sprite based on x speed
		transform.localScale = new Vector3((dx < 0 ? -1 : 1), 1, 1);

		// thanks to http://answers.unity3d.com/questions/362629/how-can-i-check-if-an-animation-is-being-played-or.html
		float time = anim.GetCurrentAnimatorStateInfo (0).normalizedTime;
		if (anim.GetCurrentAnimatorStateInfo (0).IsName("Attack") && time < 1) {
			if(time > 0.2f && !attacked) {
				Instantiate(projectile, transform.position, Quaternion.identity);
				attacked = true;
			}
		} else {
			attacked = false;
			if (!grounded) {
				anim.Play("Jump");
				anim.speed = 0.5f;
			} else if (running) {
				anim.Play ("Run");
				anim.speed = Mathf.Abs (dx) * 3;
			} else {
				anim.Play("Idle");
				anim.speed = 1;
			}
		}
		if(Input.GetKey(KeyCode.Space)) {
			anim.Play("Attack");
		}

		GetComponent<Rigidbody2D> ().MovePosition (transform.position + new Vector3 (dx, dy, 0));

		if (!grounded || totalCollisions == 0) {
			dy -= gravity * ((!Input.GetKey("w") && dy > 0) ? 4 : 1);
			if (grace > 0) {
				grace -= 1;
			}
			if (dy < 0 - terminalVelocity) {
				dy = 0 - terminalVelocity;
			}
		} else if (grounded) {
			dy = 0;
			grace = 12;
		}
		if(Input.GetKeyDown("w") && (grounded || grace > 0 || jumpsLeft > 0)) {
			dy = jumpSpeed;
			grounded = false;
			jumpsLeft--;
			grace = 0;
		}
		Debug.Log(jumpsLeft);
	}

	public void MoveTo(float x, float y) {
		transform.position = new Vector3(x, y, 0);
	}

	void OnCollisionStay2D(Collision2D c) {
		float y_dist = c.transform.position.y - transform.position.y;
		float height = GetComponent<BoxCollider2D>().bounds.extents.y;
		if(Mathf.Abs(y_dist) < height) {
			dx = 0;
		}
		if (y_dist > height && dy > 0) {dy *= -0.5f;}
	}

	void OnCollisionEnter2D(Collision2D c) {totalCollisions++;}
	void OnCollisionExit2D(Collision2D c) {totalCollisions--; if(totalCollisions < 0) {totalCollisions = 0;}}

	void OnTriggerEnter2D(Collider2D c) {
		if(dy <= 0) {
			footCollisions++;
			grounded = true;
			jumpsLeft = numJumps; // reset jumps
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
