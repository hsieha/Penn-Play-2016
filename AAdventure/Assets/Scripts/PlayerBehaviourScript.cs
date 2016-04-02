using UnityEngine;
using System.Collections;

public class PlayerBehaviourScript : MonoBehaviour {
    public float offset = 0.2f;
    public Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
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
    }

    void OnCollisionEnter(Collision collision)
    {
		if (collision.collider.gameObject.layer == LayerMask.NameToLayer ("Wall")) {
			rb.velocity = new Vector3 (0, 0, 0);
		}
    }

}
