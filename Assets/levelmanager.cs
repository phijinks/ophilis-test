using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;

public class levelmanager : MonoBehaviour {
	public Transform block;

	float tilex = 3.8f;
	float tiley = 3.6f;

	void Start () {
		loadFile("./Assets/lvl.txt");
	}

	void Update () {

	}

	void addBlock(int t, int a, int b) {
		Transform[] tiles = {block};
		if(t > tiles.Length || t <= 0) {return;}

		float x = (float)a * tilex;
		float y = (float)b * tiley;
		Instantiate(tiles[t-1], new Vector3(x, y, 0), Quaternion.identity);
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
							Int32.TryParse(line.Substring(1), out b);
						}

						// iterate over every character
						for(int a=0; a<line.Length; a++) {
							string c = line.Substring(a, 1);
							if(!c.Equals("0")) {
								int t = 0;
								Int32.TryParse(c, out t);
								addBlock(t, a, b);
							}
						}
						b--;
					}
				}
				while(line != null);
				sr.Close();
			}

			//Debug.Log("reading level was successful");
			return true;
		} catch(IOException e) {
			Debug.Log(e.Message);
			return false;
		}
	}
}
