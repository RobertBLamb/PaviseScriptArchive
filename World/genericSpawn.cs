using UnityEngine;
using System.Collections;

public class genericSpawn : MonoBehaviour {
    public GameObject[] objectsToSpawn;
    public bool spawned;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (tag == "Spawner" && spawned == false)
            {
                spawned = true;
                for (int i = 0; i < objectsToSpawn.Length; i++)
                {
                    objectsToSpawn[i].SetActive(true);
                }
            }
        }
    }
}
