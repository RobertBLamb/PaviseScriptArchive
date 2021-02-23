using UnityEngine;
using System.Collections;

//New Script 09-12-17W

public class GroundSpawner : MonoBehaviour
{
    public GameObject ground;
    public GameObject newGround;
    public GameObject peg;

    private GameObject player;

    //Collider object that parents itself to player and "neuters" next plat spawn's collider
    public GameObject spawnBreaker;
    public string spawnBreakerTag = "";

    public int copyNumber = 1;
    //Set this to limit copies!!
    public int copyLimit = 0;

    public bool stackable;

    public bool spawned = false;
    public float offset;
    public int sortOrder;

    // Use this for initialization
    void Start()
    {
        peg = transform.parent.gameObject;

        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().sortingOrder = sortOrder;

        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && spawned == false && ground != null)
        {
            if(GetComponent<BoxCollider2D>().enabled)
                spawnNewGround();

        }
        /*
        if (other.CompareTag("GroundSpawnStop"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Debug.Log("Stopped?");
        }
        */
    }

    public void spawnNewGround()
    {
        //Attempt to avoid overflow
        if (copyNumber <= 0)
            copyNumber = 1;

        if (peg != null)
        {
            
            if ((copyLimit > 0 && copyNumber < copyLimit) || copyLimit <= 0)
            {
                   
                newGround = (Instantiate(ground, (new Vector3((peg.transform.position.x + offset), peg.transform.position.y, peg.transform.position.z)), new Quaternion(0, 0, 0, 0)) as GameObject);

                if (copyLimit > 0 && copyNumber < copyLimit)
                    newGround.GetComponentInChildren<GroundSpawner>().copyNumber++;

                if (stackable)
                {
                    newGround.transform.GetChild(0).GetComponent<GroundSpawner>().sortOrder = sortOrder + 1;
                }

                spawned = true;
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
