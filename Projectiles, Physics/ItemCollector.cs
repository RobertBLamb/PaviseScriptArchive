using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour {

    public ScoreManager scoreManager;
    public Transform destinationTransform;
    public float collectionRadius;
    public float flightTime;
    public bool shrinkCollects;

	// Use this for initialization
	void Start () {
        //Could rewrite with bounds to not depend on CircleCollider2D
        collectionRadius = GetComponent<CircleCollider2D>().radius;
	}
	
	void OnTriggerEnter2D (Collider2D col)
    {
        if (col.CompareTag("Collectible"))
        {
            //Add a fly script to the collectible and let it do the rest
            FlyToPointTimed fly = col.gameObject.AddComponent<FlyToPointTimed>();
            fly.destinationPoint = transform;
            fly.flightTime = flightTime;

            if (shrinkCollects)
            {
                ShrinkCollect shrinkCollect = col.gameObject.AddComponent<ShrinkCollect>();
                shrinkCollect.targetTrans = transform;
                shrinkCollect.shrinkRadius = collectionRadius;
                //Could make a singleton in the future, but allowing multiple instances of ScoreManager might allow for ghost players, limited multiplayer, or other scoring modes
                shrinkCollect.scoreManager = scoreManager;

            }

            //Reduce flight time if close by, by multiplying it to ratio of (distance to collected item to the radius)
            //fly.flightTime = flightTime * (Vector2.Distance(transform.position, col.transform.position)/collectionRadius);
            //null the tag so that the collectible does not get checked for again
            col.tag = "Untagged";
        }
	}
}
