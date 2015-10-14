using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class FungusCounter : MonoBehaviour {
	int total = 0;
	int count = 0;
	public GameObject blockagePrefab;
	private List<GameObject> blockages = new List<GameObject>();

	void Start () {

	}
	
	void Update () {
		
	}

	void PlaceBlockages(string levelname) {
		try {
			string line;
			StreamReader sr = new StreamReader("./Assets/Levels/" + levelname + "/blockages.txt", Encoding.Default);
			
			using(sr) {
				do {
					line = sr.ReadLine();
					if(line != null && line.Length > 0) {
						char[] delimiter = {' '};
						string[] data = line.Split(delimiter);

						float x = float.Parse(data[0]);
						float y = float.Parse(data[1]);
						float goal = float.Parse(data[2]);

						GameObject b = Instantiate(blockagePrefab, new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
						b.BroadcastMessage("Start");
						b.BroadcastMessage("SetGoal", goal);
						blockages.Add(b);
					}
				}
				while(line != null);
				sr.Close();
			}
		} catch(IOException e) {
			Debug.Log(e.Message);
		}
	}

	void Reset() {
		count = 0;
		total = 0;
	}

	void IncrementTotal() {
		total++;
	}

	void Increment() {
		count++;
		if (blockages != null && blockages.Count > 0) {
			foreach (GameObject b in blockages) {
				if(b == null) {continue;}
				b.BroadcastMessage ("Calculate", Percentage ());
			}
		}
		//Debug.Log(count + " / " + total);
		//Debug.Log(Percentage() * 100);
	}

	void Decrement() {
		count--;
	}

	public float Percentage() {
		return ((float)count / (float)total);
	}
}
