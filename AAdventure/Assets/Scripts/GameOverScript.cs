using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {
    private GameInfoScript gameInfoScript;
    public GameObject gameOverCanvas;
    Animator animator;

    void Awake()
    {
        gameOverCanvas = GameObject.Find("GameOverCanvas");
        gameInfoScript = gameOverCanvas.GetComponent<GameInfoScript>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameInfoScript.isOver)
        {
            animator.SetTrigger("GameOver");
        }
	}
}
