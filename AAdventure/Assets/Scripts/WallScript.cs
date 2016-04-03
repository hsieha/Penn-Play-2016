using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
	public Material active;
	public Material inactive;
	public Material hidden;
	public Renderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		hide ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void activate() {
		rend.material = active;
	}

	public void deactivate() {
		rend.material = inactive;
	}

	public void hide() {
		rend.material = hidden;
	}

}
