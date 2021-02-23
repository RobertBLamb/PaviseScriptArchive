using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnOnPass : spawnArrayObjs {

    private Transform passingObj;

	// Use this for initialization
	void Start () {
        if (passingObj == null)
        {
            passingObj = GameObject.FindGameObjectWithTag("Player").transform;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (passingObj.position.x >= transform.position.x
            && !doneSpawning)

        {
            spawnAll();
        }
	}
}
