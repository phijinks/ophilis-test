using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;

public class levelmanager : MonoBehaviour {
	public Transform[] blocks = new Transform[3];

	float tilex = 3.8f;
	float tiley = 3.6f;

	int[,] data;
	int dimx = 0;
	int dimy = 0;

	// default player start
	float px = 5;
	float py = 5;

	void Start () {
		loadFile("./Assets/level.txt");
	}

	void Update () {

	}

	void addBlock(int t, int a, int b) {
		float x = (float)a * tilex;
		float y = (float)b * tiley;
		Instantiate(blocks[t - 1], new Vector3(x, y, 0), Quaternion.identity);
	}

	// adapted (slightly) from http://answers.unity3d.com/questions/279750/loading-data-from-a-txt-file-c.html
	bool loadFile(string filename) {
		try {
			string line;
			StreamReader sr = new StreamReader(filename, Encoding.Default);

			int b = 0;

			using(sr) {
				do {
					line = sr.ReadLine();
					if(line != null && line.Length > 0) {
						//Debug.Log(line + " " + b);
						if(line.StartsWith("# ")) { // level height
							Int32.TryParse(line.Substring(1), out dimy);
							b = dimy-1;
						} else {
							if(b == dimy-1) {
								dimx = line.Length;
								data = new int[dimx, dimy];
							}
							// iterate over every character
							for(int a=0; a<line.Length; a++) {
								string c = line.Substring(a, 1);
								if(c.Equals("$")) {
									px = (float)a * tilex;
									py = (float)b * tiley;
								} else if(!c.Equals("0")) {
									int t = 0;
									Int32.TryParse(c, out t);
									if(data != null) {
										data[a, b] = t;
									}
								}
							}
							b--;
						}
					}
				}
				while(line != null);
				sr.Close();
			}
			
			GameObject player = GameObject.Find("Player");
			player.transform.position = new Vector3(this.px, this.py, 0);

			for(int x=0; x<dimx; x++) {
				for(int y=0; y<dimy; y++) {
					int t = data[x, y];
					if(data != null && t != 0) {
						addBlock(t, x, y);
					}
				}
			}

			Debug.Log("reading level was successful");
			return true;
		} catch(IOException e) {
			Debug.Log(e.Message);
			return false;
		}
	}

	public int getBlock(int a, int b) {
		if(data != null && data.Length > 0) {
			if(a < 0 || b < 0 || a >= dimx || b >= dimy) {return 1;}
			return data[a, b];
		}
		return 0;
	}
}
