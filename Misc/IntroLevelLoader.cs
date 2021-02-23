using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroLevelLoader : MonoBehaviour {

    public GameObject[] preloadAnimObjects;
    public int loadAnimFrames = 40;
    public int loadFramesCounter;
    public GameObject introScreen;
    public float introScreenTime = 1.5f;
    public bool loading;

    //public GameObject loadLevelObject;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i <= preloadAnimObjects.Length; i++ )
        {
            GameObject.Instantiate(preloadAnimObjects[i],transform);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        loadFramesCounter++;
        if (loadFramesCounter >= loadAnimFrames)
        {
            StartCoroutine("StayIntroScreen");
        }
	}

    IEnumerator StayIntroScreen()
    {
        yield return new WaitForSeconds(introScreenTime);
        //loadLevelObject.SetActive(true);
        SceneManager.LoadScene("DemoLvl01");
        yield return new WaitForSeconds(2);
        introScreen.SetActive(false);
    }

}



