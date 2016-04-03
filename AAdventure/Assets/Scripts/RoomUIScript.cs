using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class RoomUIScript : MonoBehaviour {
    private RoomInfoScript roomInfoScript;
    private GameInfoScript gameInfoScript;
	PlayerBehaviourScript playerScript;
    public Canvas canvas;
	public GameObject roomTimer;
    public GameObject gameOverCanvas;
    private Text timeText;
    private Text mainTimeText;

    private Color baseText = new Color(0.49f, 0.46f, 0.36f, 0.5f);
    private Color pulseRed = new Color(1f, 0.67f, 0.15f, 0.38f);
    //private float pulseTime = 1f;

    // Use this for initialization
    void Start () {
        roomInfoScript = transform.parent.GetComponent<RoomInfoScript>();
        roomTimer = Instantiate(roomTimer) as GameObject;
        roomTimer.transform.SetParent(canvas.transform, false);
        gameOverCanvas = GameObject.Find("GameOverCanvas");
		playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerBehaviourScript> ();
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        roomTimer.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);

        //roomTimer.SetActive(!roomInfoScript.isActive);

        Text activeTimerText = roomTimer.GetComponentInChildren<Text>();

        if (roomInfoScript.isActive || playerScript.hasTreasure)
        {
            activeTimerText.color = baseText;

            if (roomInfoScript.time < 0f)
            {
                activeTimerText.color = Color.red;
                if (roomInfoScript.isActive)
                {
                    gameInfoScript = gameOverCanvas.GetComponent<GameInfoScript>();
                    gameInfoScript.isOver = true;
                    return;
                }
                transform.parent.gameObject.SetActive(false);
            }

            //activeTimerText = mainRoomTimer.GetComponentInChildren<Text>();
            roomInfoScript.time -= Time.deltaTime;

            if (roomInfoScript.time < 10f)
            {
                activeTimerText.color = Color.Lerp(Color.red, pulseRed, Mathf.PingPong(Time.time, 0.5f));

                if ((int)Math.Ceiling(roomInfoScript.time) % 2 == 0)
                {
                    activeTimerText.fontSize = 34;
                }
                else
                {
                    activeTimerText.fontSize = 30;
                }
            }
        }

        activeTimerText.text = getTimeText(roomInfoScript.time);
    }

    String getTimeText(float time)
    {
        //var minutes = (int)Math.Ceiling(time) / 60;
        var seconds = (int)Math.Ceiling(time) % 60;

        String timerText = string.Format("{0:0}", seconds);

        return timerText;
    }
}
