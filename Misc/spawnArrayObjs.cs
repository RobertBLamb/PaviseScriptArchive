using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnArrayObjs : MonoBehaviour {

    //Now takes array of structs, with GameObjects, spawnPoints, spawnHeights, spawnVelocities
    //so that all items and gibs can be spawned with their own parameters.
    [System.Serializable]
    public struct objSpawnSet
    {
        public GameObject itemToSpawn;
        public Transform spawnPoint;
        public bool yLocked;
        //Spawns at a specific Y for spawning level progress based on death of moving enemies, so that hitting jumping enemies does not mean the level spawns at a greater Y
        public float ySpawnHeight;
        public bool usesParentVelocity;
        public Vector2 spawnVelocity;
    }

    public bool spawnsOnStart;
    public objSpawnSet[] objsToSpawn;
    public bool doneSpawning;

    public float spawnDelay;

    protected virtual void Start()
    {
        if (spawnsOnStart)
            spawnAll();
    }

    protected void spawnAll()
    {
        if (spawnDelay == 0)
            spawnArray();
        else
            StartCoroutine(delayedSpawn());
    }


    public IEnumerator delayedSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        spawnArray();
    }

    protected void spawnArray()
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
