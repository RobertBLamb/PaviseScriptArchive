using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class DebugValueDisplay : MonoBehaviour {

    public Text healthText;
    public Text manaText;
    public float value;

    public GameObject player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {

        if (healthText != null)
        {
            value = player.GetComponent<BlockHealth>().currentHealthBlock.health;
            healthText.text = "" + value.ToString();
        }
        else if (manaText != null)
        {
            manaText.text = "" + value.ToString();
            value = player.GetComponent<PlayerSpecialMoves>().mana;
        }
    }
}
