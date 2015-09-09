using UnityEngine;
using System.Collections;

public class playermovement : MonoBehaviour {
	public Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		float accel = 200f;
		float maxSpeed = 15;
		bool maxLeft = rb.velocity.x < -maxSpeed;
		bool maxRight = rb.velocity.x > maxSpeed;
		if(Input.GetKey("a") && !maxLeft) {
			rb.AddForce(new Vector3(-accel, 0, 0));
		} else if(Input.GetKey("d") && !maxRight) {
			rb.AddForce(new Vector3(accel, 0, 0));
		} else {
			rb.AddForce(new Vector3(10 * rb.velocity.x * -0.8f, 0, 0));
		}

		if(Input.GetKeyDown("w")) {
			rb.AddForce(new Vector3(0, 1000, 0));
		}
		if(Input.GetKey("s")) {
			rb.AddForce(new Vector3(0, -100, 0));
		}
	}
}
