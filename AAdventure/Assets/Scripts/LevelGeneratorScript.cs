using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneratorScript : MonoBehaviour {
	public Dictionary<Tuple, Transform> map = new Dictionary<Tuple, Transform> ();

	public Transform room;
	public Transform player;

	// Use this for initialization
	void Start () {
		for (int i = -1; i < 2; i++) {
			for (int j = -1; j < 2; j++) {
				Transform r = (Transform) Instantiate (room, new Vector3 (i * 7, j * 7, 0), Quaternion.identity);
				map.Add (new Tuple (i, j), r);
			}
		}

		Instantiate (player, new Vector3 (0, 0, -1), Quaternion.AngleAxis(90, new Vector3(1,0,0)));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
