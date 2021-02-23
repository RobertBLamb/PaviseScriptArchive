using UnityEngine;
using System.Collections;

public class CollideToStart : MonoBehaviour {
    public GameObject scriptHolder;
    public MonoBehaviour scriptToEnable;
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            //Debug.Log("stepin on the beach");
            //TEMP AS FUCK
            scriptHolder.GetComponent<movingPlat>().enabled = true;
        }        
    }
}
