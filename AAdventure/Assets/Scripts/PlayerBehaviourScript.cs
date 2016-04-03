using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBehaviourScript : MonoBehaviour {
    private GameInfoScript gameInfoScript;
    public GameObject gameOverCanvas;
	public GameObject winCanvas;
    public float offset = 0.2f;
    public Rigidbody rb;
	public Vector3 curRoomPos;
	public GameObject cam;
    public GameObject startPanel;
    public Dictionary<Tuple, Transform> map;
	public bool camMoving;
	public bool hasTreasure;
	public int numKeys;
	public Vector3 targetCameraPosition;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
		cam = GameObject.Find ("Main Camera");
		LevelGeneratorScript levelGenerator = (LevelGeneratorScript) GameObject.Find ("LevelGenerator").GetComponent("LevelGeneratorScript");
		map = levelGenerator.map;
		camMoving = false;
		hasTreasure = false;
		numKeys = 0;
		updateRoomPos ();
        gameOverCanvas = GameObject.Find("GameOverCanvas");
		winCanvas = GameObject.Find ("WinCanvas");
        startPanel = GameObject.Find("StartPanel");
    }
	
	// Update is called once per frame
	void Update () {
		if (!camMoving) {
            if (Input.anyKey && startPanel != null)
            {
                startPanel.SetActive(false);
            }
			if (Input.GetKey (KeyCode.DownArrow)) {
				rb.MovePosition (this.transform.position - new Vector3 (0, 1 * offset, 0));
			}
			if (Input.GetKey (KeyCode.UpArrow)) {
				rb.MovePosition (this.transform.position - new Vector3 (0, -1 * offset, 0));
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				rb.MovePosition (this.transform.position - new Vector3 (offset, 0, 0));
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				rb.MovePosition (this.transform.position - new Vector3 (-1 * offset, 0, 0));
			} 
		}
		if (rb.position.x > curRoomPos.x + 3.5) {
			if (!camMoving) {
				camMoving = true;
				targetCameraPosition = cam.transform.position + new Vector3 (7, 0, 0);
			}
		} else if (rb.position.x < curRoomPos.x - 3.5) {
			if (!camMoving) {
				camMoving = true;
				targetCameraPosition = cam.transform.position + new Vector3 (-7, 0, 0);
			}
		} else if (rb.position.y > curRoomPos.y + 3.5) {
			if (!camMoving) {
				camMoving = true;
				targetCameraPosition = cam.transform.position + new Vector3 (0, 7, 0);
			}
		} else if (rb.position.y < curRoomPos.y - 3.5) {
			if (!camMoving) {
				camMoving = true;
				targetCameraPosition = cam.transform.position + new Vector3 (0, -7, 0);
			}
		}
		if (camMoving) {
			cam.transform.position = Vector3.MoveTowards(cam.transform.position, targetCameraPosition, 20 * Time.deltaTime);
		}
		if (camMoving && cam.transform.position == targetCameraPosition) {
			camMoving = false;
			updateRoomPos ();
		}

    }

	void updateRoomPos() {
		curRoomPos = cam.transform.position + new Vector3 (0, -2, 20);
		Tuple curLocation = new Tuple((int) curRoomPos.x / 7, (int) curRoomPos.y / 7);
		if (map.ContainsKey (curLocation)) {
			Transform r = map [curLocation];
			foreach (Transform rChild in r) {
				if (rChild.name == "RoomObject") {
					r = rChild;
					break;
				}
			}
			RoomObjectScript roomScript = r.GetComponent<RoomObjectScript>();
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
					foreach (Transform rChild in r) {
						if (rChild.name == "RoomObject") {
							r = rChild;
							break;
						}
					}
					RoomObjectScript roomScript = r.GetComponent<RoomObjectScript>();
					roomScript.deactivate ();
				}
			}
		}
        if(!map[curLocation].gameObject.activeSelf)
        {
            gameInfoScript = gameOverCanvas.GetComponent<GameInfoScript>();
            gameInfoScript.isOver = true;
        }
	}

    void OnCollisionEnter(Collision collision)
    {
		if (collision.collider.gameObject.layer == LayerMask.NameToLayer ("Wall")) {
			rb.velocity = new Vector3 (0, 0, 0);
		}
		if (collision.collider.gameObject.tag == "WallPiece") {
			WallScript wScript = collision.collider.GetComponent<WallScript> ();
			if (wScript.isDoor && numKeys > 0) {
				numKeys--;
				wScript.isDoor = false;
				collision.gameObject.SetActive (false);
				WallScript otherWScript = wScript.otherDoor.GetComponent<WallScript> ();
				otherWScript.isDoor = false;
				wScript.otherDoor.gameObject.SetActive (false);
			}
		}
		if (collision.collider.tag == "Item") {
			if (collision.collider.name == "Treasure(Clone)") {
				hasTreasure = true;
			}
			if (collision.collider.name == "Key(Clone)") {
				numKeys++;
			}
			Destroy (collision.gameObject);
		}
		if (collision.collider.tag == "Finish") {
			Debug.Log ("YOU WIN");
			gameInfoScript = winCanvas.GetComponent<GameInfoScript> ();
			gameInfoScript.isOver = true;
			gameInfoScript.isWon = true;
			Destroy (this);
		}
		if (collision.collider.tag == "Death") {
			Debug.Log ("YOU LOSE");
            gameInfoScript = gameOverCanvas.GetComponent<GameInfoScript>();
            gameInfoScript.isOver = true;
            Destroy (this);
		}
    }

}
