using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneratorScript : MonoBehaviour {
	public Dictionary<Tuple, Transform> map;

	public Transform room;
	public Transform player;

	enum Entry{
		Left, Right, Top, Bottom
		};

	// Use this for initialization
	void Start () {
		map = new Dictionary<Tuple, Transform> ();
		generateLevel ();
		Instantiate (player, new Vector3 (0, 0, -1), Quaternion.AngleAxis(90, new Vector3(1,0,0)));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void instantiateRoom(int x, int y, bool leftEntry, bool rightEntry, bool topEntry, bool bottomEntry) {
		Transform r = (Transform)Instantiate (room, new Vector3 (x*7, y*7, 0), Quaternion.identity);
		RoomScript rScript = r.GetComponentInChildren<RoomScript> ();
		rScript.setWallEntries (leftEntry, rightEntry, topEntry, bottomEntry);
		map.Add (new Tuple (x, y), r);

	}

	void generateLevel() {
		Dictionary<Tuple, HashSet<int>> entryMap = new Dictionary<Tuple,HashSet<int>> ();
		Tuple curTuple = new Tuple (0, 0);
		entryMap.Add (curTuple, new HashSet<int> ());
		while (entryMap.Count > 0) {
			foreach (Tuple t in entryMap.Keys) {
				curTuple = t;
				break;
			}
			HashSet<int> newEntries = randomizeRoom (curTuple.x, curTuple.y, entryMap[curTuple], Mathf.Abs(curTuple.x)+Mathf.Abs(curTuple.y));
			foreach (int entry in newEntries) {
				if (entry == (int)Entry.Left) {
					Tuple left = new Tuple (curTuple.x - 1, curTuple.y);
					if (entryMap.ContainsKey (left)) {
						HashSet<int> oldEntries = entryMap [left];
						oldEntries.Add ((int)Entry.Right);
						entryMap.Add (left, oldEntries);
					} else {
						HashSet<int> oldEntries = new HashSet<int> ();
						oldEntries.Add ((int)Entry.Right);
						entryMap.Add (left, oldEntries);
					}
				} else if (entry == (int)Entry.Right) {
					Tuple right = new Tuple (curTuple.x + 1, curTuple.y);
					if (entryMap.ContainsKey (right)) {
						HashSet<int> oldEntries = entryMap [right];
						oldEntries.Add ((int)Entry.Left);
						entryMap.Add (right, oldEntries);
					} else {
						HashSet<int> oldEntries = new HashSet<int> ();
						oldEntries.Add ((int)Entry.Left);
						entryMap.Add (right, oldEntries);
					}
				} else if (entry == (int)Entry.Top) {
					Tuple top = new Tuple (curTuple.x, curTuple.y + 1);
					if (entryMap.ContainsKey (top)) {
						HashSet<int> oldEntries = entryMap [top];
						oldEntries.Add ((int)Entry.Bottom);
						entryMap.Add (top, oldEntries);
					} else {
						HashSet<int> oldEntries = new HashSet<int> ();
						oldEntries.Add ((int)Entry.Bottom);
						entryMap.Add (top, oldEntries);
					}
				} else if (entry == (int)Entry.Bottom) {
					Tuple bottom = new Tuple (curTuple.x, curTuple.y - 1);
					if (entryMap.ContainsKey (bottom)) {
						HashSet<int> oldEntries = entryMap [bottom];
						oldEntries.Add ((int)Entry.Top);
						entryMap.Add (bottom, oldEntries);
					} else {
						HashSet<int> oldEntries = new HashSet<int> ();
						oldEntries.Add ((int)Entry.Top);
						entryMap.Add (bottom, oldEntries);
					}
				}
			}
			entryMap.Remove (curTuple);
		}
	}

		HashSet<int> randomizeRoom(int x, int y, HashSet<int> entry, int layer) {
		HashSet<int> entriesSet = new HashSet<int>();
		foreach (int e in entry) {
			entriesSet.Add (e);
		}
		HashSet<int> newEntries = new HashSet<int> ();
		while (entriesSet.Count < 3 - layer) {
			int rand = Random.Range (0, 3);
			while (entriesSet.Contains (rand)) {
				rand = Random.Range (0, 3);
			}
			entriesSet.Add (rand);
			newEntries.Add (rand);
		}
		instantiateRoom (x, y, !entriesSet.Contains (0), !entriesSet.Contains (1), !entriesSet.Contains (2), !entriesSet.Contains (3));
		return newEntries;
	}
		
}
