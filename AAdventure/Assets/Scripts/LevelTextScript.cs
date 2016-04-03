using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelTextScript : MonoBehaviour {
	Text levelText;

	// Use this for initialization
	void Start () {
		levelText = gameObject.GetComponent<Text>();
		levelText.text = "LEVEL: " + PlayerPrefs.GetInt ("LevelNumber", 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
