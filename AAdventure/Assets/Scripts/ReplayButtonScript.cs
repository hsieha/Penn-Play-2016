using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReplayButtonScript : MonoBehaviour {

    public void replayGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
