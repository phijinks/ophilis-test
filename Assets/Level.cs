using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;

public class Level : MonoBehaviour {
	public GameObject[] blocks = new GameObject[3]; // list of block types
	public GameObject[] props = new GameObject[8];
	public Transform[] walls = new Transform[4];

	float tilex = 3.8f; // width of tile grid space
	float tiley = 3.6f; // height of tile grid space

	int[,] data; // 2d array of block ints
	int dimx = 0; // width of the level in # of blocks
	int dimy = 0; // height of the level in # of blocks

	// default player starting point
	float px = 5;
	float py = 5;

	void Start () {
		loadLevel("01");
	}

	void Update () {

	}

	void addBlock(int t, int a, int b) { // adds a block to the level grid
		if (t > blocks.Length) {return;}
		float x = (float)a * tilex;
		float y = (float)b * tiley;
		GameObject block = Instantiate (blocks [t - 1], new Vector3 (x, y, 0), Quaternion.identity) as GameObject;
		if (block != null && GetBlock(a, b+1) == 0) {
			block.BroadcastMessage("Start");
			block.BroadcastMessage("SetFloor");
		}
	}

	void loadLevel(string name) {
		loadBlocks("./Assets/Levels/" + name + "/blocks.txt");
		loadProps("./Assets/Levels/" + name + "/props.txt");

		// tell the fungus counter to place blockages based on name_blockages.txt
		GameObject fungus = GameObject.Find("FungusCounter");
		fungus.BroadcastMessage("PlaceBlockages", name);

		float dist = 4;
		for(float y = 0; y<=(dimy*tiley); y += walls[0].GetComponent<Renderer>().bounds.size.y) {
			Instantiate(walls[0], new Vector3(-dist, y, -1), Quaternion.identity);
			Instantiate(walls[1], new Vector3(dist + (float)dimx * tilex, y, -1), Quaternion.identity);
		}
	}

	// adapted (slightly) from http://answers.unity3d.com/questions/279750/loading-data-from-a-txt-file-c.html
	bool loadBlocks(string filename) {
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
			player.BroadcastMessage("SetSpawn");

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

	bool loadProps(string filename) {
		try {
			string line;
			StreamReader sr = new StreamReader(filename, Encoding.Default);
			
			using(sr) {
				do {
					line = sr.ReadLine();
					if(line == null) {break;}
					char[] delimiter = {' '};
					string[] data = line.Split(delimiter);

					float[] offset_y = {0, -0.6f, 0.8f};

					int type = int.Parse(data[0]);
					float x = float.Parse(data[1]) * tilex;
					float y = (dimy - float.Parse(data[2]) - (type < offset_y.Length ? offset_y[type] : 0)) * tiley;
					float z = float.Parse(data[3]);

					Instantiate(props[type], new Vector3(x, y, z), Quaternion.identity);
				} while(line != null);
				sr.Close();
			}

			return true;
		} catch(IOException e) {
			Debug.Log(e.Message);
			return false;
		}
	}

	public int GetBlock(int a, int b) {
		if(data != null && data.Length > 0) {
			if(a < 0 || b < 0 || a >= dimx || b >= dimy) {return 1;}
			return data[a, b];
		}
		return 1;
	}
}
