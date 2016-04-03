using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneratorScript : MonoBehaviour {
	public Dictionary<Tuple, Transform> map;

	public Tuple treasureLocation;
	public Tuple exitLocation;

	public Transform room;
	public Transform player;
	public Transform treasure;
	public Transform exit;
	public Transform obstacle;
	public Transform key;

	enum Entry{
		Left, Right, Top, Bottom
		};

	// Use this for initialization
	void Start () {
		map = new Dictionary<Tuple, Transform> ();
        generateLevel ();
		generateTreasure ();
		generateExit (4);
		generateObstacles (2);
		generateKeyAndDoor ();
        Instantiate(player, new Vector3(0, 0, -1), Quaternion.AngleAxis(90, new Vector3(1, 0, 0)));
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
		treasureLocation = new Tuple(0,0);
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
			Instantiate (treasure, new Vector3 (treasureLocation.x*7+(Random.value-0.5f)*4, 
				treasureLocation.y*7+(Random.value-0.5f)*4, -1), Quaternion.AngleAxis(180,new Vector3(0,0,1)));
		treasureTransform.parent = map [treasureLocation];

	}

	public void generateExit(int minDist) {
		HashSet<Tuple> possible = new HashSet<Tuple> ();
		foreach (Tuple t in map.Keys) {
			if (distanceBetweenRooms (treasureLocation, t) >= minDist) {
				possible.Add (t);
			}
		}
		int rand = Random.Range (0, possible.Count);
		int count = 0;
		exitLocation = treasureLocation;
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

	public void generateObstacles(int difficulty) {
		int count = 0;
		while (count < difficulty) {
			foreach (Tuple t in map.Keys) {
				if (t.Equals (new Tuple (0, 0)))
					continue;
				if (Random.value < 0.25) {
					Transform obstacleTransform = (Transform) Instantiate 
						(obstacle, new Vector3 (t.x * 7 + (Random.value - 0.5f) * 4, t.y * 7 + (Random.value - 0.5f) * 4, -1), Quaternion.identity);
					obstacleTransform.parent = map [t];
				}
			}
			count++;
		}
	}

	public void generateKeyAndDoor() {
		int rand = Random.Range (0, map.Keys.Count);
		int count = 0;
		Tuple doorLocation = new Tuple (0, 0);
		foreach (Tuple t in map.Keys) {
			if (count == rand) {
				doorLocation = t;
				break;
			}
			count++;
		}
		RoomObjectScript rScript = map [doorLocation].GetComponentInChildren<RoomObjectScript> ();
		count = 0;
		bool isValid = false;
		while (!isValid && count < 1000) {
			rand = Random.Range (0, 4);
			if(rand == (int) Entry.Left && !rScript.leftWallEntry.gameObject.activeSelf) {
				isValid = true;
			}
			if(rand == (int) Entry.Right && !rScript.rightWallEntry.gameObject.activeSelf) {
				isValid = true;
			}
			if(rand == (int) Entry.Top && !rScript.topWallEntry.gameObject.activeSelf) {
				isValid = true;
			}
			if(rand == (int) Entry.Bottom && !rScript.bottomWallEntry.gameObject.activeSelf) {
				isValid = true;
			}
			count++;
		}
		if (isValid) {
			Transform door1 = rScript.createDoor (rand);
			RoomObjectScript otherRoomScript = map [nextRoomTuple (doorLocation, rand)].GetComponentInChildren<RoomObjectScript> ();
			Transform door2 = otherRoomScript.createDoor (reverseEntry (rand));
			WallScript door1Script = door1.GetComponent<WallScript> ();
			WallScript door2Script = door2.GetComponent<WallScript> ();
			door1Script.otherDoor = door2;
			door2Script.otherDoor = door1;
		}

		isValid = false;
		count = 0;
		Tuple keyLocation = new Tuple (0, 0);
		while (!isValid && count < 1000) {
			rand = Random.Range (0, map.Keys.Count);
			int randcount = 0;
			foreach (Tuple t in map.Keys) {
				if (randcount == rand) {
					keyLocation = t;
					break;
				}
				randcount++;
			}
			if (distanceBetweenRooms (new Tuple (0, 0), keyLocation) > 0) {
				isValid = true;
			}
			count++;
		}
		if (isValid) {
			Transform keyTransform = (Transform) Instantiate 
				(key, new Vector3 (keyLocation.x * 7 + (Random.value - 0.5f) * 4, keyLocation.y * 7 + (Random.value - 0.5f) * 4, -1), Quaternion.identity);
			keyTransform.parent = map [keyLocation];
		}
	}

	public int reverseEntry(int entry) {
		if (entry == (int) Entry.Left) {
			return (int)Entry.Right;
		}
		if (entry == (int) Entry.Right) {
			return (int)Entry.Left;
		}
		if (entry == (int) Entry.Top) {
			return (int)Entry.Bottom;
		}
		if (entry == (int) Entry.Bottom) {
			return (int)Entry.Top;
		}
		return -1;
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
