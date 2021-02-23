using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class PauseReset : MonoBehaviour {

    public bool paused;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Time.timeScale = 0;
                paused = true;
            }
            else if (paused)
            {
                Time.timeScale = 1.0f;
                paused = false;
            }
        }
	}
}
