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

	float spawn_x = 0;
	float spawn_y = 0;

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
		float flip = (dx < 0 ? -1f : 1f);
		float scale = 1;
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
		if (info.IsName ("Run")) {
			flip *= -1; // run animation is backwards :/
			scale = 2; // also very small
		}
		transform.localScale = new Vector3(flip, 1, 1);

		// thanks to http://answers.unity3d.com/questions/362629/how-can-i-check-if-an-animation-is-being-played-or.html
		float time = info.normalizedTime;
		if (info.IsName("Attack") && time < 1) {
			if(time > 0.2f && !attacked) {
				Instantiate(projectile, transform.position + (new Vector3(0, -1.5f, 0))*transform.localScale.y, Quaternion.identity);
				attacked = true;
			}
		} else {
			attacked = false;
			if (!grounded) {
				anim.Play("Jump");
			} else if (running) {
				anim.Play ("Run");
				anim.speed = Mathf.Abs (dx) * 2;
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
				if(grace == 0) {jumpsLeft--;}
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
	}

	public void SetSpawn() {
		spawn_x = transform.position.x;
		spawn_y = transform.position.y;
	}

	public void MoveTo(float x, float y, bool spawn) {
		transform.position = new Vector3(x, y, 0);
		SetSpawn ();
	}

	public void MoveTo(float x, float y) {
		MoveTo(x, y, false);
	}

	public void Die() {
		dx = 0;
		dy = 0;
		MoveTo(spawn_x, spawn_y);
		//anim.Play("Spawn");
	}

	void OnCollisionStay2D(Collision2D c) {
		float y_dist = c.transform.position.y - transform.position.y;
		float height = GetComponent<BoxCollider2D>().bounds.extents.y;
		if(Mathf.Abs(y_dist) < height) {
			dx = 0;
		}
		if (y_dist > height && dy > 0) {dy *= -0.5f;}
	}

	void OnCollisionEnter2D(Collision2D c) {
		string objName = c.gameObject.name;
		if(objName.Contains ("Block")) {totalCollisions++;}
		if(objName.Contains ("Enemy")) {
			Die();
		}
	}
	void OnCollisionExit2D(Collision2D c) {
		string objName = c.gameObject.name;
		if (objName.Contains ("Block")) {
			totalCollisions--;
			if (totalCollisions < 0) {
				totalCollisions = 0;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D c) {
		string objName = c.gameObject.name;
		if(dy <= 0 && objName.Contains("Block")) {
			footCollisions++;
			grounded = true;
			jumpsLeft = numJumps; // reset jumps
		}
		if (objName.Contains ("Lamppost")) {
			SetSpawn();
		}
	}

	void OnTriggerExit2D(Collider2D c) {
		if(c.gameObject.name.Contains("Block")) {
			footCollisions--;
			if(footCollisions <= 0) {
				footCollisions = 0;
				grounded = false;
			}
		}
	}
}
