using UnityEngine;
using System.Collections;

// wrote my own basic animation script because it's honestly easier than wrangling with unity

public class fungus : MonoBehaviour {
	int frame = 0;
	bool animating = false;
	public Sprite[] frames = new Sprite[15];

	void Start () {
		for(int i=0; i<15; i++) {
			//frames[i] = Resources.Load("block_anim/" + i, typeof(Sprite)) as Sprite;
			//Debug.Log(frames[i]);
		}
	}
	
	void Update () {
		if(animating) {
			frame++;
		}
		if(frame >= 15) {
			frame = 14;
			animating = false;
		}
		GetComponent<SpriteRenderer>().sprite = frames[frame];
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name.Equals("Player")) {
			animating = true;
		}
	}
}
