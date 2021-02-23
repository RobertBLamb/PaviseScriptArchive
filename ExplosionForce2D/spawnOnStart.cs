using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnOnStart : spawnArrayObjs {

    // Use this for initialization
    void Start()
    {
        if (!doneSpawning)
        {
            spawnAll();
        }

        //Attempt to make default spawnPoint this tranform
        /*
        if (spawnPoint == null)
            spawnPoint = transform;
	*/
    }
}
