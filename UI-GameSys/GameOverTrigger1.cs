using UnityEngine;
using System.Collections;

public class GameOverTrigger1 : MonoBehaviour
{
    public GameObject gameOverUi;
    public GameObject playerSvel;
    public GameObject mainUI;

    public bool memes;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BlockHealth>().currentHealthBlock.index == 0
            && GetComponent<BlockHealth>().currentHealthBlock.health <= 0)
        {
            gameOverUi.SetActive(true);
            mainUI.SetActive(false);
            memes = true;
            playerSvel.SetActive(false);
        }
    }
}
