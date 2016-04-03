using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class RoomUIScript : MonoBehaviour {
    private RoomInfoScript roomInfoScript;
    public Canvas canvas;
	public GameObject roomTimer;
    public GameObject mainRoomTimer;
    private Text timeText;
    private Text mainTimeText;

    // Use this for initialization
    void Start () {
        roomInfoScript = GetComponent<RoomInfoScript>();
        roomTimer = Instantiate(roomTimer) as GameObject;
        roomTimer.transform.SetParent(canvas.transform, false);
		mainRoomTimer = GameObject.Find ("MainTimer");
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        roomTimer.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);

        roomTimer.SetActive(!roomInfoScript.isActive);

        Text activeTimerText = roomTimer.GetComponentInChildren<Text>();

        if (roomInfoScript.isActive)
        {
            if (roomInfoScript.time < 0f)
            {
                return;
            }

            activeTimerText = mainRoomTimer.GetComponentInChildren<Text>();
            roomInfoScript.time -= Time.deltaTime;

            if (roomInfoScript.time < 10f)
            {
                activeTimerText.color = Color.red;
            }
            else
            {
                activeTimerText.color = Color.white;
            }
        }

        activeTimerText.text = getTimeText(roomInfoScript.time);
    }

    String getTimeText(float time)
    {
        var minutes = (int)Math.Ceiling(time) / 60;
        var seconds = (int)Math.Ceiling(time) % 60;

        String timerText = string.Format("{0:0} : {1:00}", minutes, seconds);

        return timerText;
    }
}
