using UnityEngine;
using System.Collections;

public class Blockage : MonoBehaviour {
	Animator anim;
	float progress = 0f;
	float goal = 1f;

	void Start() {
		anim = GetComponent<Animator>();
	}

	// set how much of the level needs to be fungusified for the blockage to break
	void SetGoal(float value) {
		this.goal = value;
	}

	// calculate how close to the goal the player is
	void Calculate(float value) {
		this.progress = value / this.goal;
	}
	
	void Update() {
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
		if(info.IsName("Normal") && progress <= 0.99f) {
			anim.Play("Normal", 0, progress);
			//info.normalizedTime = progress;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.name.Contains("Projectile") && progress >= 1f) {
			anim.Play("C R U M B L I N G"); // maybe i shouldn't have named it that
			Destroy(GetComponent<Collider2D>());
		}
	}
}
