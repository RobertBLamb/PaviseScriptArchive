using UnityEngine;
using System.Collections;

public class VendingObjSpawn : MonoBehaviour {

    public GameObject vendingFrame;
    public GameObject vendingGlass;
    public GameObject sodaCan;

    public Sprite destroyed;
    public SpriteRenderer myRenderer;

    private bool startedSpawning;
    private bool doneSpawning;

    /*
    public float launchSpeedX = 20;
    public float xPlus = 10;

    public float launchSpeedY = 5;
    public float yPlus = -1;

    public float startingLaunchAngle = 20;
    public float angleChange = 5;
    */

    public int amtCans = 8;
    public float flingMultiplier = 3F;

    // Use this for initialization
    void Start () {

        myRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > .5F && !startedSpawning)
            {
            myRenderer.sprite = destroyed;
            StartCoroutine ("spawnAll");
        }

    }

    IEnumerator spawnAll()
    {

        vendingFrame = (Instantiate(vendingFrame, transform.position + new Vector3(0, .33F, -.32F), new Quaternion(0, 0, 0, 0))) as GameObject;
        vendingGlass = (Instantiate(vendingGlass, transform.position + new Vector3(0, .33F, -.32F), new Quaternion(0, 0, 0, 0))) as GameObject;


        GameObject[] cans = new GameObject[amtCans];
        startedSpawning = true;
        yield return new WaitForSeconds(.1F);

        vendingFrame.layer = 18;
        vendingGlass.layer = 18;

        for (int i = 0; i < amtCans; i++)
        {
            cans[i] = (Instantiate(sodaCan, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject);

            yield return new WaitForSeconds(.05F);

            cans[i].GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * flingMultiplier;
            
            /*
                //Quaternion.Euler(0, 0, startingLaunchAngle) * 
                new Vector2(launchSpeedX, launchSpeedY);
                
            //GetComponent<Rigidbody2D>().velocity * flingMultiplier;
            startingLaunchAngle = startingLaunchAngle + angleChange;
            launchSpeedX = launchSpeedX + xPlus;
            launchSpeedY = launchSpeedY + yPlus;
            */
    }
        doneSpawning = true;
    }
}
