using UnityEngine;
using System.Collections;

public class StopPlatCycleOnDeath : MonoBehaviour {

    public Health healthScript;
    //Parent (moving) object of GroundSpawner trigger
    public GameObject currentPlat;
    public GroundSpawner currentGroundSpawner;
    public bool cycleStopped;

    // Use this for initialization
    void Start()
    {
        healthScript = GetComponent<Health>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<ChaserEnemy>())
        {
            currentPlat = GetComponent<ChaserEnemy>().currentPlat;
        }

        // <1 in case of any fractional health going on
        //if (healthScript.health < 1)
        {
            if (GetComponent<ChaserEnemy>())
            {
                currentPlat = GetComponent<ChaserEnemy>().currentPlat;
            }

            if (currentPlat != null)
            {
                if (currentPlat.GetComponentInChildren<GroundSpawner>())
                {
                    currentGroundSpawner = currentPlat.GetComponentInChildren<GroundSpawner>();

                    //preemptively spawn if newGround of currentPlat hasn't been spawned yet
                    if (healthScript.health < 1 && ! cycleStopped 
                        )
                    {
                        if (!currentGroundSpawner.spawned)
                        {
                            //Debug.Log("hadtospawn");

                            currentGroundSpawner.spawnNewGround();
                        }
                        //Debug.Log("trying");
                        currentGroundSpawner.newGround.GetComponentInChildren<GroundSpawner>().spawnNewGround();
                        currentGroundSpawner.newGround.GetComponentInChildren<GroundSpawner>().enabled = false;
                        currentGroundSpawner.newGround.GetComponentInChildren<GroundSpawner>().newGround.GetComponentInChildren<GroundSpawner>().enabled = false;
                        cycleStopped = true;
                    }
                }
            }
        }
    }
}
