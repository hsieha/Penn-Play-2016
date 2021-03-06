﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomObjectScript : MonoBehaviour {
	public bool hasSeen;
	public Material active;
	public Material inactive;
	public Material hidden;
	public Renderer rendereŕ;
	public HashSet<Transform> walls;
	public RoomInfoScript roomInfoScript;
	public Transform leftWallEntry;
	public Transform rightWallEntry;
	public Transform topWallEntry;
	public Transform bottomWallEntry;

	enum Entry{
		Left, Right, Top, Bottom
	};

	// Use this for initialization
	void Start () {
		rendereŕ = GetComponent<Renderer> ();
		walls = new HashSet<Transform> ();
		foreach(Transform children in transform) {
			
			foreach (Transform wallpiece in children) {
				if (wallpiece.name == "WallEntry") {
					if (children.name == "LeftWall") {
						leftWallEntry = wallpiece;
					} else if (children.name == "RightWall") {
						rightWallEntry = wallpiece;
					} else if (children.name == "TopWall") {
						topWallEntry = wallpiece;
					} else if (children.name == "BottomWall") {
						bottomWallEntry = wallpiece;
					}
				}
				if (wallpiece.tag == "WallPiece") {
					walls.Add (wallpiece);
				}
			}
		}
		hasSeen = false;
		roomInfoScript = transform.parent.GetComponent<RoomInfoScript> ();
		deactivate ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setWallEntries(bool leftEntry, bool rightEntry, bool topEntry, bool bottomEntry) {
		leftWallEntry.gameObject.SetActive (leftEntry);
		rightWallEntry.gameObject.SetActive (rightEntry);
		topWallEntry.gameObject.SetActive (topEntry);
		bottomWallEntry.gameObject.SetActive (bottomEntry);
	}

	public int numOfEntries() {
		return (leftWallEntry.gameObject.activeSelf ? 0 : 1) + (rightWallEntry.gameObject.activeSelf ? 0 : 1) +
		(topWallEntry.gameObject.activeSelf ? 0 : 1) + (bottomWallEntry.gameObject.activeSelf ? 0 : 1);
	}

	public Transform createDoor(int entry) {
		if (entry == (int)Entry.Left) {
			leftWallEntry.gameObject.SetActive (true);
			WallScript wScript = leftWallEntry.GetComponent<WallScript> ();
			wScript.isDoor = true;
			return leftWallEntry;
		}
		if (entry == (int)Entry.Right) {
			rightWallEntry.gameObject.SetActive (true);
			WallScript wScript = rightWallEntry.GetComponent<WallScript> ();
			wScript.isDoor = true;
			return rightWallEntry;
		}
		if (entry == (int)Entry.Top) {
			topWallEntry.gameObject.SetActive (true);
			WallScript wScript = topWallEntry.GetComponent<WallScript> ();
			wScript.isDoor = true;
			return topWallEntry;
		}
		if (entry == (int)Entry.Bottom) {
			bottomWallEntry.gameObject.SetActive (true);
			WallScript wScript = bottomWallEntry.GetComponent<WallScript> ();
			wScript.isDoor = true;
			return bottomWallEntry;
		}
		return null;
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
