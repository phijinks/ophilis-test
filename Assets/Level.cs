using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;

public class Level : MonoBehaviour {
	public Transform[] blocks = new Transform[4]; // list of block types

	float tilex = 3.8f; // width of tile grid space
	float tiley = 3.6f; // height of tile grid space

	int[,] data; // 2d array of block ints
	int dimx = 0; // width of the level in # of blocks
	int dimy = 0; // height of the level in # of blocks

	// default player starting point
	float px = 5;
	float py = 5;

	void Start () {
		loadFile("./Assets/level.txt");
	}

	void Update () {

	}

	void addBlock(int t, int a, int b) { // adds a block to the level grid
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
						if(line.StartsWith("# ")) { // level height
							Int32.TryParse(line.Substring(1), out dimy);
							b = dimy-1; // starts at the top of the level
						} else {
							if(b == dimy-1) {
								dimx = line.Length;
								data = new int[dimx, dimy]; // instantiate the 2d array of data
							}

							// iterate over every character in the line
							for(int a=0; a<line.Length; a++) {
								string c = line.Substring(a, 1);
								if(c.Equals("$")) { // player starting point
									px = (float)a * tilex;
									py = (float)b * tiley;
								} else if(!c.Equals("0")) { // place a block
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

			// iterate over data array and place blocks
			for(int x=0; x<dimx; x++) {
				for(int y=0; y<dimy; y++) {
					int t = data[x, y];
					if(data != null && t != 0) {
						addBlock(t, x, y);
					}
				}
			}

			//Debug.Log("reading level was successful");
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
