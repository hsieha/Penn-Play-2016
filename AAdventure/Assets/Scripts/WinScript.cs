using UnityEngine;
using System.Collections;

public class WinScript : MonoBehaviour {
	private GameInfoScript gameInfoScript;
	public GameObject winCanvas;
	Animator animator;

	void Awake()
	{
		winCanvas = GameObject.Find("WinCanvas");
		gameInfoScript = winCanvas.GetComponent<GameInfoScript>();
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		if (gameInfoScript.isOver && gameInfoScript.isWon)
		{
			Debug.Log ("won");
			animator.SetTrigger("GameOver");
		}
	}
}
