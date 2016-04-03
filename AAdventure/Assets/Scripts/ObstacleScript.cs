using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {
	enum Direction {
		Horizontal, Vertical
	}

	Direction direction;
	Vector3 curVel;

	Rigidbody rb;

	// Use this for initialization
	void Start () {
		if (Random.value < 0.5) {
			direction = Direction.Horizontal;
		} else {
			direction = Direction.Vertical;
		}
		rb = GetComponent<Rigidbody> ();
		if (direction == Direction.Horizontal) {
			if (Random.value < 0.5) {
				rb.velocity = new Vector3 (3, 0, 0);
			} else {
				rb.velocity = new Vector3 (-3, 0, 0);
			}
		} else {
			if (Random.value < 0.5) {
				rb.velocity = new Vector3 (0, 3, 0);
			} else {
				rb.velocity = new Vector3 (0, -3, 0);
			}
		}
		curVel = rb.velocity;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "WallPiece") {
			curVel *= -1;
			rb.velocity = curVel;
		}
	}
}
