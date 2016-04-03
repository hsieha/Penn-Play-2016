using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomScript : MonoBehaviour {
	public bool hasSeen;
	public Material active;
	public Material inactive;
	public Material hidden;
	public Renderer rendereŕ;
	public HashSet<Transform> walls;
	public RoomInfoScript roomInfoScript;

	// Use this for initialization
	void Start () {
		rendereŕ = GetComponent<Renderer> ();
		walls = new HashSet<Transform> ();
		foreach(Transform children in transform) {
			foreach (Transform wallpiece in children) {
				if (wallpiece.tag == "WallPiece") {
					walls.Add (wallpiece);
				}
			}
		}
		hasSeen = false;
		roomInfoScript = transform.GetComponent<RoomInfoScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void deactivate() {
		if (hasSeen) {
			rendereŕ.material = inactive;
			foreach (Transform wallpiece in walls) {
				WallScript wallScript = (WallScript) wallpiece.GetComponent ("WallScript");
				wallScript.deactivate ();
			}
		} else {
			rendereŕ.material = hidden;
			foreach (Transform wallpiece in walls) {
				WallScript wallScript = (WallScript) wallpiece.GetComponent ("WallScript");
				wallScript.hide ();
			}
		}
		roomInfoScript.isActive = false;
	}

	public void activate() {
		hasSeen = true;
		rendereŕ.material = active;
		foreach (Transform wallpiece in walls) {
			WallScript wallScript = (WallScript) wallpiece.GetComponent ("WallScript");
			wallScript.activate ();
		}
		roomInfoScript.isActive = true;
	}
}
