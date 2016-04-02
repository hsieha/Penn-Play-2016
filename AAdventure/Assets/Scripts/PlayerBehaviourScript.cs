using UnityEngine;
using System.Collections;

public class PlayerBehaviourScript : MonoBehaviour {
    public float offset = 0.2f;
    public Rigidbody rb;
	public Vector3 curRoomPos;
	public GameObject cam;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
		cam = GameObject.Find ("Main Camera");
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
	}

    void OnCollisionEnter(Collision collision)
    {
		if (collision.collider.gameObject.layer == LayerMask.NameToLayer ("Wall")) {
			rb.velocity = new Vector3 (0, 0, 0);
		}
    }

}
