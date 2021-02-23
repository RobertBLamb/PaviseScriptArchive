using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeParent : MonoBehaviour {

    //Make sure to turn this on if this parent object has no other purpose
    public bool emptyParent;

	// Use this for initialization
	void Start () {
        transform.DetachChildren();

        if (emptyParent)
            Destroy(gameObject);
	}

}
