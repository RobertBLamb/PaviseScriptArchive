using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SightRadius : MonoBehaviour {

    //Place a prefab here to specify its tag to be targeted
    public GameObject sampleTargetPrefab;
    public string targetTag;
    public GameObject currentSighted;
    public List<GameObject> sightedObjects;
    public int sightedLimit = 5;
    public bool doneSighting;
    public float doneSightingTimeLim = 1f;
    private float originalSightTimeLim;

    // Use this for initialization
    void Start () {

        targetTag = sampleTargetPrefab.tag;
    //sightRadius = gameObject.GetComponent<Collider2D>();
}

// Update is called once per frame
    void Update () {
        //if (sightedObjects.)

	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (currentSighted == null && other.tag == targetTag && other.GetComponent<Projectile>().reflected == true)
        {
            currentSighted = other.gameObject;
        }
    }
    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MedProjectile") {
            if (sightedObjects.Count < sightedLimit)
            {
                StartCoroutine("SightCountdown");

                originalSightTimeLim = doneSightingTimeLim;
                sightedObjects.Add(other.gameObject);
            }
         }
    }
    */
    IEnumerator SightCountdown()
    {

        yield return new WaitForSeconds(doneSightingTimeLim);
        doneSighting = true;

    }

}
