using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBehaviourScript : MonoBehaviour {
    public float offset = 0.2f;
    public Rigidbody rb;
	public Vector3 curRoomPos;
	public GameObject cam;
	public Dictionary<Tuple, Transform> map;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
		cam = GameObject.Find ("Main Camera");
		LevelGeneratorScript levelGenerator = (LevelGeneratorScript) GameObject.Find ("LevelGenerator").GetComponent("LevelGeneratorScript");
		map = levelGenerator.map;
		updateRoomPos ();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.MovePosition(this.transform.position - new Vector3(0, 1 * offset, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.MovePosition(this.transform.position - new Vector3(0, -1 * offset, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.MovePosition(this.transform.position - new Vector3(offset, 0, 0));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.MovePosition(this.transform.position - new Vector3(-1 * offset, 0, 0));
		} 
		if (rb.position.x > curRoomPos.x + 3.5) {
			cam.transform.position += new Vector3 (7, 0, 0);
			updateRoomPos ();
		} else if (rb.position.x < curRoomPos.x - 3.5) {
			cam.transform.position += new Vector3 (-7, 0, 0);
			updateRoomPos ();
		} else if (rb.position.y > curRoomPos.y + 3.5) {
			cam.transform.position += new Vector3 (0, 7, 0);
			updateRoomPos ();
		} else if (rb.position.y < curRoomPos.y - 3.5) {
			cam.transform.position += new Vector3 (0, -7, 0);
			updateRoomPos ();
		}
    }

	void updateRoomPos() {
		curRoomPos = cam.transform.position + new Vector3 (0, -2, 20);
		Tuple curLocation = new Tuple((int) curRoomPos.x / 7, (int) curRoomPos.y / 7);
		if (map.ContainsKey (curLocation)) {
			Transform r = map [curLocation];
			RoomScript roomScript = (RoomScript)r.GetComponent ("RoomScript");
			roomScript.activate ();
		}
		for (int i = curLocation.x - 1; i < curLocation.x + 2; i++) {
			for (int j = curLocation.y - 1; j < curLocation.y + 2; j++) {
				if (i == curLocation.x && j == curLocation.y) {
					continue;
				}
				Tuple t = new Tuple (i, j);
				if (map.ContainsKey (t)) {
					Transform r = map [t];
					RoomScript roomScript = (RoomScript) r.GetComponent ("RoomScript");
					roomScript.deactivate ();
				}
			}
		}
	}

    void OnCollisionEnter(Collision collision)
    {
		if (collision.collider.gameObject.layer == LayerMask.NameToLayer ("Wall")) {
			rb.velocity = new Vector3 (0, 0, 0);
		}
    }

}
