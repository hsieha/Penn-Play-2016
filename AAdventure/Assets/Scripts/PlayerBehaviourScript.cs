using UnityEngine;
using System.Collections;

public class PlayerBehaviourScript : MonoBehaviour {
    public float offset = 0.2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position = this.transform.position - new Vector3(0, offset, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position = this.transform.position - new Vector3(0, -1*offset, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position = this.transform.position - new Vector3(offset, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position = this.transform.position - new Vector3(-1*offset, 0, 0);
        }
    }
}
