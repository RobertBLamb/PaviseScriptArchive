using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarDisplay : MonoBehaviour {

    public float value;
    public float maxValue;
    public GameObject barFill;
    public GameObject barBorder;
    public float minPosX;
    public float maxPosX;

    public bool healthBar;
    public bool manaBar;
    public bool healthBlock;

    public GameObject player;
    //Mostly TEMP, for use when testing block health system
    public GameObject blockObj;
	// Use this for initialization
	public void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        if (blockObj == null)
        {
            blockObj = player;
        }
        minPosX = barFill.transform.position.x - 140;
        maxPosX = barFill.transform.position.x;
        if (healthBar)
            maxValue = player.GetComponent<BlockHealth>().healthPerBlock;
        else if (manaBar)
            maxValue = player.GetComponent<PlayerSpecialMoves>().maxMana;

        //New 05-02-18 for HealthBlock testing
        else if (healthBlock)
        {
            maxValue = blockObj.GetComponent<BlockHealth>().healthPerBlock;
        }
    }

    // Update is called once per frame

    void Update()
	{
        if (healthBar)
            value = player.GetComponent<Health>().health;
        else if (manaBar)
            value = player.GetComponent<PlayerSpecialMoves>().mana;
        else if (healthBlock)
            value = blockObj.GetComponent<BlockHealth>().currentHealthBlock.health;

        /*
        barFill.transform.position = new Vector3(minPosX + (value / maxValue)* (maxPosX - minPosX), barFill.transform.position.y, barFill.transform.position.z);
        if (barFill.transform.position.x < minPosX)
            barFill.transform.position = new Vector3(minPosX, barFill.transform.position.y, barFill.transform.position.z);
        if (barFill.transform.position.x > maxPosX)
            barFill.transform.position = new Vector3(maxPosX, barFill.transform.position.y, barFill.transform.position.z);
        */

        barFill.GetComponent<Image>().fillAmount = value / maxValue;
    }
    /*
       void OnGUI()
       {
           int w = Screen.width, h = Screen.height;

           GUIStyle style = new GUIStyle();

           Rect rect = new Rect(0, 0, w, h * 2 / 100);
           style.alignment = TextAnchor.UpperLeft;
           style.fontSize = h * 2 / 100;
           style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);

           string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
           GUI.Label(rect, text, style);
       }
       */
}

