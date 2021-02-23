using UnityEngine;
using System.Collections;

public class doubleTap : MonoBehaviour {
    public float buttonCD = 0.5f;
    public int buttonCount;
    public float timeToDoubleTap;
    private float ogGrav;
    public float hyperGrav;
	// Use this for initialization
	void Start ()
    {
        ogGrav = GetComponent<Rigidbody2D>().gravityScale;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (buttonCount == 0)
            {
                buttonCD = timeToDoubleTap;
                buttonCount++;
            }

            else if (buttonCount == 1)
            {
                if (buttonCD > 0)
                {
                    //function
                    buttonCount = 1000;
                }
                
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (buttonCount == 0)
            {
                buttonCD = timeToDoubleTap;
                buttonCount++;
            }

            else if (buttonCount == 1)
            {
                if (buttonCD > 0)
                {
                    //function
                    if(!GetComponent<PlayerController>().onRunnable())
                    {
                        GetComponent<Rigidbody2D>().gravityScale = hyperGrav;
                    }
                }

            }
        }
        if (buttonCD>0)
        {
            buttonCD -= 1 * Time.deltaTime;
        }
        else
        {
            buttonCD = 0;
        }
        if(buttonCD==0)
        {
            buttonCount = 0;
        }

        if(GetComponent<PlayerController>().onRunnable() && !GetComponent<PlayerSpecialMoves>().dashing)
        {
            GetComponent<Rigidbody2D>().gravityScale = ogGrav;
        }
    }
}
