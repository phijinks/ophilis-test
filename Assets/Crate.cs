using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {
	public Player player;
	Animator anim;
	int stage = 1;
	float dy = 0;

	void Start () {
		anim = GetComponent<Animator>();
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			Vector3 p = player.transform.position;
			Vector3 my = transform.position;
			if(Mathf.Abs(p.x - my.x) < 5 && stage <= 3) {
				float size = 3;
				float flip = (p.x < my.x ? 1f : -1f);
				transform.localScale = new Vector3(flip*size, size, 1);

				anim.Play("Impact_" + stage);
				if(stage == 3) {
					Destroy(GetComponent<Collider2D>());
				}
				stage++;
			}
		}
	}
}
