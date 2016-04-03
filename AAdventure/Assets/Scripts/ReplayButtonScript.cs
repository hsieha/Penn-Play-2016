using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReplayButtonScript : MonoBehaviour {

    public void replayGame()
    {
        Debug.Log("Again!");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
