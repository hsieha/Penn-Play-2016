using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
	public Material door;
	public Material active;
	public Material inactive;
	public Material hidden;
	public Renderer rend;

	public bool isDoor;
	public Transform otherDoor;

	void Awake() {
		isDoor = false;
	}

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		hide ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void activate() {
		if (!isDoor) {
			rend.material = active;
		} else {
			rend.material = door;
		}
	}

	public void deactivate() {
		if (!isDoor) {
			rend.material = inactive;
		} else {
			rend.material = door;
		}
	}

	public void hide() {
		rend.material = hidden;
	}

}
