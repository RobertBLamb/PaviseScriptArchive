using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnLandOnPlat : spawnArrayObjs {

    public EndPlatSegAndSpawn platSegEnder;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.gameObject.GetComponent<PlayerController>().currentPlat == gameObject)
            {
                spawnAll();

                //TODO: Make this a little less hardcoded?
                if (platSegEnder != null)
                    platSegEnder.DestroyPlatTriggers();

                this.enabled = false;
                //Destroy(this);
            }
        }
	}
}
