using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkCollect : MonoBehaviour {

    public ScoreManager scoreManager;
    //TODO: TEMP! NEEDS TO BE SETTABLE IN A PREFAB FOR THE COLLECTIBLE SOMEHOW, IT MAY NEED ITS OWN SCRIPT
    public int scoreValue = 1;

    public Transform targetTrans;
    public float shrinkRadius;
    public bool shrinking;
    public float collectRadius = 3f;
    public float shrinkFactor = .5f;
    //public Vector3 defaultScale;


	// Use this for initialization
	void Start () {
        //defaultScale = transform.localScale;
        //ASSUMES THAT COLLECTION DEPENDS ON DISTANCE, AND NOT ON COLLIDERS!
        GetComponent<Collider2D>().enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector2.Distance(transform.position, targetTrans.position) <= shrinkRadius)
        {
            if (transform.localScale.magnitude > .01f)
            {
                transform.localScale *= (shrinkFactor * Time.deltaTime);
            }
        }

        if (Vector2.Distance(transform.position, targetTrans.position) <= collectRadius)
        {
            Collect();
        }
    }

    void Collect()
    {
        scoreManager.UpdateScore(scoreValue);
        Destroy(gameObject);
    }
}
