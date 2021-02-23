using UnityEngine;
using System.Collections;

public class specialColCheck : MonoBehaviour {
    public bool slamCheck;
    public bool dashCheck;
    public GameObject thisGameObject;
	// Use this for initialization
	void Start () {
        thisGameObject = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("REEEEEEEEEEEE");
            //Debug.Log(col.gameObject.GetComponent<PlayerSpecialMoves>().dashing);
            if(dashCheck && col.gameObject.GetComponent<PlayerSpecialMoves>().dashing)
            {
                thisGameObject.GetComponent<Health>().health = 0;
            }
            else if (slamCheck && col.gameObject.GetComponent<PlayerSpecialMoves>().slamming)
            {
                thisGameObject.GetComponent<Health>().health = 0;
            }
        }
    }
}
