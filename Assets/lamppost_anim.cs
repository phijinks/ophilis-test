using UnityEngine;
using System.Collections;
using UnityEditor;

public class lamppost_anim : MonoBehaviour {
	float frame = 0f;
	float speed = 1f;
	bool animating = true;
	Sprite[] frames = new Sprite[41];
	
	void Start () {
		string[] folders = {"lamppost"};
		string[] guids = AssetDatabase.FindAssets("", folders);
		for(int i=0; i<guids.Length; i++) {
			//frames[i] = Resources.
		}
	}
	
	void Update () {
		if(animating) {
			frame += speed;
		}
		if(frame >= frames.Length) {
			frame = 0;
		}
		GetComponent<SpriteRenderer>().sprite = frames[(int)frame];
	}
}
