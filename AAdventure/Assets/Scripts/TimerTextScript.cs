using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimerTextScript : MonoBehaviour {
    public Text timerText;
    private float time;

	// Use this for initialization
	void Start () {
        time = 60f;
	}
	
	// Update is called once per frame
	void Update () {
        var minutes = (int)Math.Ceiling(time) / 60;
        var seconds = (int)Math.Ceiling(time) % 60;

        if(time < 10f)
        {
            timerText.color = Color.red;
        }

        timerText.text = string.Format("{0:0} : {1:00}", minutes, seconds);

        time -= Time.deltaTime;
    }
}
