using UnityEngine;
using System.Collections;

// wrote my own basic animation script because it's honestly easier than wrangling with unity

public class fungus : MonoBehaviour {
	float frame = 0f;
	float speed = 0.7f;
	bool animating = false;
	public Sprite[] frames = new Sprite[15];

	void Start () {

	}
	
	void Update () {
		if(animating) {
			frame += speed;
		}
		if(frame >= 15) {
			frame = 14;
			animating = false;
		}
		GetComponent<SpriteRenderer>().sprite = frames[(int)frame];
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name.Equals("Player")) {
			animating = true;
		}
	}
}
