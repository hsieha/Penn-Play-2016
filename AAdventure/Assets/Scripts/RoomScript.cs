using UnityEngine;
using System.Collections;

public class RoomScript : MonoBehaviour {
	RoomInfoScript roomInfoScript;
	PlayerBehaviourScript playerScript;

	// Use this for initialization
	void Start () {
		roomInfoScript = transform.GetComponent<RoomInfoScript> ();
		playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerBehaviourScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!roomInfoScript.isActive) {
			foreach (Transform t in transform) {
				if (t.tag == "Item" || t.tag == "Finish") {
					t.gameObject.SetActive (false);
				}
			}
		} else {
			foreach (Transform t in transform) {
				if (t.tag == "Item") {
					t.gameObject.SetActive (true);
				}
				if (t.tag == "Finish"){
					if (playerScript.hasTreasure) {
						t.gameObject.SetActive (true);
					} else {
						t.gameObject.SetActive (false);
					}
				}
			}
		}
	}
}
