using UnityEngine;
using System.Collections;

public enum ShieldColor
{
    //Default shield reflecting states
    BrightGreen,
    PaleTransGreen,
    PaleTransYellow,
    PaleTransWhite,
    //Special moves
    BrightCyan,
    BrightOrange,
    BrightPurple

}

public class ArmPoses : MonoBehaviour {

    public SpriteRenderer shieldFlash;
    public SpriteRenderer shieldHandle;

    Color flashInactiveColor = new Color(1, 1, 1, 40/255f);
    Color flashActiveColor = new Color(100/255f, 240/255f, 0, 100/255f);
    Color flashBlockColor = new Color(230/255f, 1, 0, 150/255f);
    Color flashReflectColor = new Color(100/255f, 240/255f, 0, 240/255f);

    Color handleInactiveColor = new Color(150/255f, 150/255f, 150/255f, 1);
    Color handleBlockColor = new Color(230/255f, 255/255f, 0, 1);
    Color handleReflectColor = new Color(100/255f, 240/255f, 0, 1);

    public Sprite[] poses;
    public bool reflectTrigger;
    public bool reflecting;
    public bool Trigger;


    // Use this for initialization
    void Start () {

        //poses = Resources.LoadAll<Sprite>("SvelArms");
        shieldFlash = GameObject.FindGameObjectWithTag("ShieldFlash").GetComponent<SpriteRenderer>();
        shieldHandle = GameObject.FindGameObjectWithTag("ShieldHandle").GetComponent<SpriteRenderer>();

        //flashActiveColor = new Color(100 / 255, 240 / 255, 0, 200 / 255);

        /*
        flashInactiveColor = shieldFlash.color;
        handleInactiveColor = shieldHandle.color;
        */
        //gameObject.GetComponent<SpriteRenderer>().sprite = poses[0];
        reflectTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
            //Wow Unity, thanks for canceling my coroutines when called from a object that gets destroyed
            //gameObject.GetComponent<SpriteRenderer>().sprite = poses[2];

        /*
        else if (Input.GetMouseButtonDown(0) && reflecting)

        {
            gameObject.GetComponent<SpriteRenderer>().sprite = poses[2];
        }
        */

        if (Input.GetMouseButton(0) && !reflecting)
        {
            shieldFlash.color = flashActiveColor;
            shieldHandle.color = handleReflectColor;
            //gameObject.GetComponent<SpriteRenderer>().sprite = poses[1];
        }

        else if (!reflecting)
        {
            shieldFlash.color = flashInactiveColor;
            shieldHandle.color = handleInactiveColor;
            //gameObject.GetComponent<SpriteRenderer>().sprite = poses[0];
        }
    }

    public void flashShield(int colorChoice)
    {
        Color chosenColor;
        switch (colorChoice)
        {
            case 1:
                //Opaque green reflect
                chosenColor = flashReflectColor;
                break;
            case 2:
                //Pale trans green
                chosenColor = flashActiveColor;
                break;
            case 3:
                //Pale trans yellow block
                chosenColor = flashBlockColor;
                break;
            default:
                //Pale trans white inactive
                chosenColor = flashInactiveColor;
                break;
        }

        StartCoroutine(flashShieldCoroutine(chosenColor));
    }

    public IEnumerator flashShieldCoroutine(Color flashColor = default (Color))
    {
        reflectTrigger = false;
        reflecting = true;
        //gameObject.GetComponent<SpriteRenderer>().sprite = poses[2];
        shieldFlash.color = flashColor;
        shieldHandle.color = handleReflectColor;

        yield return new WaitForSeconds(.1F);

        //gameObject.GetComponent<SpriteRenderer>().sprite = poses[0];
        shieldFlash.color = flashInactiveColor;
        shieldHandle.color = handleInactiveColor;

        reflecting = false;
    }

}
