using UnityEngine;
using System.Collections;

public class FungusCounter : MonoBehaviour {
	int total = 0;
	int count = 0;

	void Start () {
		
	}
	
	void Update () {
		
	}

	void Reset() {
		count = 0;
	}

	void IncrementTotal() {
		total++;
	}

	void Increment() {
		count++;
		Debug.Log(Percentage() * 100);
	}

	void Decrement() {
		count--;
	}

	public float Percentage() {
		return ((float)count / (float)total);
	}
}
