using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionExplosion : MonoBehaviour {

    //Works SPECIFICALLY with an object that will collide in the same space 
    //with a set of phys objs that are parented to one object

    //STILL Need a solution for getting child objs to fling at the same velocity as the parent

    //public GameObject baseCollider;
    public GameObject objsToExplodeParent;
    public GameObject currentObjsParent;

    public bool doneSpawning = false;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(delayedSpawn());
	}

    IEnumerator delayedSpawn()
    {
        //Instantiate(baseCollider, transform.position, transform.rotation);
        currentObjsParent = (Instantiate(objsToExplodeParent, transform.position, transform.rotation) as GameObject);
        currentObjsParent.transform.DetachChildren();
        yield return new WaitForSeconds(.03f);

        Destroy(currentObjsParent);
        //Destroy(baseCollider);
        Destroy(gameObject);
    }
}
