using UnityEngine;
using System.Collections;

public class RoomScript : MonoBehaviour {
	RoomInfoScript roomInfoScript;

	// Use this for initialization
	void Start () {
		roomInfoScript = transform.GetComponent<RoomInfoScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!roomInfoScript.isActive) {
			foreach (Transform t in transform) {
				if (t.tag == "Item") {
					t.gameObject.SetActive (false);
				}
			}
		} else {
			foreach (Transform t in transform) {
				if (t.tag == "Item") {
					t.gameObject.SetActive (true);
				}
			}
		}
	}
}
