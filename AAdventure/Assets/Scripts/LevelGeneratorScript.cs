using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneratorScript : MonoBehaviour {
	public class Tuple {
		public int x;
		public int y;
		public Tuple(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}
	public IDictionary map = new Dictionary<Tuple, Transform> ();

	public Transform room;

	// Use this for initialization
	void Start () {
		for (int i = -1; i < 2; i++) {
			for (int j = -1; j < 2; j++) {
				Debug.Log(i + " " + j);
				Transform r = (Transform) Instantiate (room, new Vector3 (i * 7, j * 7, 0), Quaternion.identity);
				map.Add (new Tuple (i, j), r);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
