using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlatSegAndSpawn : MovingPlatSegment {

    //
    public SpawnOnLandOnPlat[] platTriggerScripts;

	// Use this for initialization
	protected override void Start () {

        PopulatePlatQueue();

        //Get an array of SpawnOnLandOnPlats from platQueue gameObjects
        GameObject[] platQueueTemp = platQueue.ToArray();
        platTriggerScripts = new SpawnOnLandOnPlat[platQueue.Count];

        for (int i = 0; i < platQueueTemp.Length; i++)
        {
            platTriggerScripts[i] = platQueueTemp[i].GetComponent<SpawnOnLandOnPlat>();
            platTriggerScripts[i].platSegEnder = this;
        }

        activationCoroutine = StartCoroutine(ActivateDelayed());
    }
	
    public void DestroyPlatTriggers ()
    {
        foreach (SpawnOnLandOnPlat script in platTriggerScripts)
        {
            Destroy(script);
        }
    }
}
