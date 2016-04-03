using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneratorScript : MonoBehaviour {
	public Dictionary<Tuple, Transform> map;

	public Transform room;
	public Transform player;
	public Transform treasure;
	public Transform exit;

	enum Entry{
		Left, Right, Top, Bottom
		};

	// Use this for initialization
	void Start () {
		map = new Dictionary<Tuple, Transform> ();
		generateLevel ();
		generateTreasure ();
		Instantiate (player, new Vector3 (0, 0, -1), Quaternion.AngleAxis(90, new Vector3(1,0,0)));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void instantiateRoom(int x, int y, bool leftEntry, bool rightEntry, bool topEntry, bool bottomEntry) {
		Transform r = (Transform)Instantiate (room, new Vector3 (x*7, y*7, 0), Quaternion.identity);
		RoomObjectScript rScript = r.GetComponentInChildren<RoomObjectScript> ();
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
			HashSet<int> newEntries = randomizeRoom (curTuple.x, curTuple.y, entryMap[curTuple], (int) ((Mathf.Abs(curTuple.x)+Mathf.Abs(curTuple.y))/1));
			foreach (int entry in newEntries) {
				if (entry == (int)Entry.Left) {
					Tuple left = new Tuple (curTuple.x - 1, curTuple.y);
					if (entryMap.ContainsKey (left)) {
						HashSet<int> oldEntries = entryMap [left];
						oldEntries.Add ((int)Entry.Right);
						entryMap[left] = oldEntries;
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
						entryMap[right] = oldEntries;
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
						entryMap[top] = oldEntries;
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
						entryMap[bottom] = oldEntries;
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

		while (entriesSet.Count < 4 - layer) {
			int rand = Random.Range (0, 4);
			int count = 0;
			while ((entriesSet.Contains (rand) || map.ContainsKey(nextRoomTuple(new Tuple(x,y),rand))) && count < 1000) {
				rand = Random.Range (0, 4);
				count++;
			}
			if (count >= 1000) {
				break;
			}
			entriesSet.Add (rand);
			newEntries.Add (rand);

		}
		instantiateRoom (x, y, !entriesSet.Contains (0), !entriesSet.Contains (1), !entriesSet.Contains (2), !entriesSet.Contains (3));
		return newEntries;
	}

	
	public Tuple nextRoomTuple(Tuple curTuple, int entry) {
		if (entry == (int)Entry.Left) {
			return new Tuple (curTuple.x - 1, curTuple.y);
		} else if (entry == (int)Entry.Right) {
			return new Tuple (curTuple.x + 1, curTuple.y);
		} else if (entry == (int)Entry.Top) {
			return new Tuple (curTuple.x, curTuple.y + 1);
		} else if (entry == (int)Entry.Bottom) {
			return new Tuple (curTuple.x, curTuple.y - 1);
		}
		return null;
	}

	public void generateTreasure() {
		HashSet<Tuple> ends = new HashSet<Tuple> ();
		foreach (Tuple t in map.Keys) {
			RoomObjectScript rScript = map [t].GetComponentInChildren<RoomObjectScript> ();
			if (rScript.numOfEntries () == 1) {
				ends.Add (t);
			}
		}
		Tuple treasureLocation = new Tuple(0,0);
		int rand = Random.Range (0, ends.Count);
		int count = 0;
		foreach (Tuple t in ends) {
			if (count == rand) {
				treasureLocation = t;
				break;
			}
			count++;
		}
		Transform treasureTransform = (Transform) 
			Instantiate (treasure, new Vector3 (treasureLocation.x*7+(Random.value-0.5f)*4, treasureLocation.y*7+(Random.value-0.5f)*4, -1), Quaternion.identity);
		treasureTransform.parent = map [treasureLocation];
		generateExit (treasureLocation, 4);
	}

	public void generateExit(Tuple treasureLocation, int minDist) {
		HashSet<Tuple> possible = new HashSet<Tuple> ();
		foreach (Tuple t in map.Keys) {
			if (distanceBetweenRooms (treasureLocation, t) >= minDist) {
				possible.Add (t);
			}
		}
		int rand = Random.Range (0, possible.Count);
		int count = 0;
		Tuple exitLocation = treasureLocation;
		foreach (Tuple t in possible) {
			if (count == rand) {
				exitLocation = t;
				break;
			}
			count++;
		}
		Transform exitTransform = (Transform) 
			Instantiate (exit, new Vector3 (exitLocation.x*7, exitLocation.y*7, -1), Quaternion.identity);
		exitTransform.parent = map [exitLocation];
	}

	public struct QueuePair {
		public Tuple tuple;
		public int dist;
		public QueuePair(Tuple t, int d) {
			tuple = t;
			dist = d;
		}
	}

	public int distanceBetweenRooms(Tuple start, Tuple dest) {
		Queue<QueuePair> tuples = new Queue<QueuePair> ();
		HashSet<Tuple> seen = new HashSet<Tuple> ();
		seen.Add (start);
		tuples.Enqueue (new QueuePair(start,0));
		while (tuples.Count > 0) {
			QueuePair p = tuples.Dequeue ();
			if (p.tuple.Equals(dest)) {
				return p.dist;
			}
			HashSet<Tuple> neighbors = getNeighbors (p.tuple);
			foreach (Tuple neighbor in neighbors) {
				
				if (!seen.Contains (neighbor)) {
					seen.Add (neighbor);
					tuples.Enqueue (new QueuePair (neighbor, p.dist + 1));
				}
			}
		}
		return -1;
	}

	public HashSet<Tuple> getNeighbors(Tuple t) {
		HashSet<Tuple> neighbors = new HashSet<Tuple> ();
		if (map.ContainsKey (t)) {
			RoomObjectScript roomObjectScript = map [t].GetComponentInChildren<RoomObjectScript> ();
			if (!roomObjectScript.leftWallEntry.gameObject.activeSelf) {
				neighbors.Add (nextRoomTuple (t, (int)Entry.Left));
			}
			if (!roomObjectScript.rightWallEntry.gameObject.activeSelf) {
				neighbors.Add (nextRoomTuple (t, (int)Entry.Right));
			}
			if (!roomObjectScript.topWallEntry.gameObject.activeSelf) {
				neighbors.Add (nextRoomTuple (t, (int)Entry.Top));
			}
			if (!roomObjectScript.bottomWallEntry.gameObject.activeSelf) {
				neighbors.Add (nextRoomTuple (t, (int)Entry.Bottom));
			}
		}
		return neighbors;
	}
}
