using UnityEngine;
using System.Collections;

public class spawnOnKill : spawnArrayObjs {

	/*
	// Update is called once per frame
	void Update ()
    {
	    if (GetComponent<Health>().health<= 0 
            //&& spawnPoint != null
            )
        {
            if (!doneSpawning)
            {
                spawnAll();
            }

            /*
            if (!yLocked)
            {
                for (int i = 0; i < itemToSpawn.Length; i++)
                    Instantiate(itemToSpawn[i], spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                for (int i = 0; i < itemToSpawn.Length; i++)
                    Instantiate(itemToSpawn[i], new Vector3(spawnPoint.position.x, ySpawnHeight, spawnPoint.position.z), spawnPoint.rotation);
            }
           
        }
    }
    */

    //Currently relies on spawnOnKill being attached to the HITBOX object of an obj
    //The hitbox obj is to be destroyed when the main obj "dies" and destroys its child objs.

    /*
    private void OnDestroy()
    {
        //if (this.enabled)
            if (!doneSpawning)
            {
                spawnAll();
            }
    }
    */

    public void spawnAll()
    {
        if (!doneSpawning)
        {
            for (int i = 0; i < objsToSpawn.Length; i++)
            {
                objSpawnSet currentObjSet = objsToSpawn[i];
                GameObject currentObj = currentObjSet.itemToSpawn;

                if (currentObjSet.spawnPoint == null)
                    currentObjSet.spawnPoint = transform;

                if (!currentObjSet.yLocked)
                    currentObj = (Instantiate(currentObjSet.itemToSpawn, currentObjSet.spawnPoint.position, transform.rotation) as GameObject);
                else
                    currentObj = (Instantiate(currentObjSet.itemToSpawn, new Vector3(currentObjSet.spawnPoint.position.x, currentObjSet.ySpawnHeight, currentObjSet.spawnPoint.position.z), transform.rotation) as GameObject);

                if (currentObjSet.usesParentVelocity)
                {
                    if (GetComponent<Rigidbody2D>())
                        currentObjSet.spawnVelocity = GetComponent<Rigidbody2D>().velocity;
                }
                if (currentObjSet.spawnVelocity.x != 0 || currentObjSet.spawnVelocity.y != 0)
                {
                    if (currentObj.GetComponent<Rigidbody2D>())
                        currentObj.GetComponent<Rigidbody2D>().velocity = currentObjSet.spawnVelocity;
                }
            }
            doneSpawning = true;
        }
    }
}
