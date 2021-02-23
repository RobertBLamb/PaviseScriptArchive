using UnityEngine;
using System.Collections;

public class StalkerDrone : MonoBehaviour
{
    aStar_Manager a_Manager;

    //10-17R2 revisit
    public float movementTimer = 1.5f;
    private float movementTimerReset;
    private bool changingHeight;
    private Rigidbody2D player;
    private Rigidbody2D objRigidbody;
    public bool aStarDisabled;
    public float addSpeed;
    public float maxDifInHeight;
    public float adjHeight;
    public float rangeX;

    public float catchUpSpeed;
    public float catchUpdist;
    //10-17R2
    public Vector3 droneOffset;
    public Transform desiredLocation;
    public bool overlapping;
    public bool fixingPath;
    bool aStarChain;
    bool farOutThere;

    //rework how the pathing bool works
    //rework  random height change
    //known bugs, sometimes flies to player when close

    void Start()
    {
        a_Manager = aStar_Manager.S;

        objRigidbody = GetComponent<Rigidbody2D>();
        movementTimerReset = movementTimer;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //lock on x zone
        Debug.DrawLine(new Vector3(player.transform.position.x - rangeX, player.transform.position.y, 0) + droneOffset,
               new Vector3(player.transform.position.x + rangeX, player.transform.position.y, 0) + droneOffset, Color.black);
        //lock on y zone0
        Debug.DrawLine(new Vector3(player.transform.position.x + droneOffset.x, player.transform.position.y + droneOffset.y + maxDifInHeight, transform.position.z),
            (new Vector3(player.transform.position.x + droneOffset.x, player.transform.position.y + droneOffset.y - maxDifInHeight, transform.position.z)), Color.black);
        desiredLocation.position = droneOffset + player.transform.position;
        //controls x movement when not using unit to get close
        if (aStarDisabled)
        {
            objRigidbody.velocity = new Vector2(player.velocity.x + addSpeed, player.velocity.y + adjHeight);
            AdjustSpeed();
            OutOfBounds();
        }
        //10-17R2 optimize you fool
        //lets the drone know when to switch to A* and back
        if (Vector3.Distance(transform.position, desiredLocation.transform.position) < 5 &&
            GetComponent<Unit>().target != null && !overlapping)
        {
            //likely the issue
            //GetComponent<Unit>().target = null;

            //GetComponent<Unit>().StopAllCoroutines();
            aStarDisabled = true;
        }
        else if (overlapping)
        {
            //aStarDisabled = false;
            //aStarChain = true;
        }
        else if (!overlapping && aStarChain)
        {
            //aStarChain = false;
            //GetComponent<Unit>().target = player.transform;
            //Debug.Log("we got a hit");
        }
    }
    void OutOfBounds()
    {
        if (((transform.position.y - desiredLocation.position.y + maxDifInHeight > maxDifInHeight * 2) && (adjHeight > 0)) || ((transform.position.y - desiredLocation.position.y + maxDifInHeight < 0) && (adjHeight < 0)))
        {
            adjHeight = 0;
            //Destroy current grid and replace it with a new one at Player's location
            //a_Manager.ReplaceGrid(player.transform);
        }
        if (((transform.position.x - desiredLocation.position.x + rangeX > rangeX * 2) && addSpeed > 0) || ((transform.position.x - desiredLocation.position.x + rangeX < 0) && addSpeed < 0))
        {
            addSpeed = 0;
            //a_Manager.ReplaceGrid(player.transform);

        }
        //Debug.Log(transform.position.y - desiredLocation.position.y + maxDifInHeight);
    }
    void AdjustSpeed()
    {
        movementTimer -= Time.deltaTime;
        if (movementTimer <= 0)
        {
            int changeCourse = Random.Range(0, 101);

            if (changeCourse > 57)
            {
                StartCoroutine(RandomHeightChange());
                StartCoroutine(RandomSpeedChange());
            }
            movementTimer = movementTimerReset;
        }
    }

    IEnumerator RandomHeightChange()
    {
        //near top
        if ((transform.position.y >= desiredLocation.position.y + maxDifInHeight - 1))
        {
            adjHeight = Random.Range(-4, 0);
        }
        //near bot
        else if ((transform.position.y <= desiredLocation.position.y - maxDifInHeight + 1))
        {
            adjHeight = Random.Range(0, 4);
        }
        //mid area
        else if ((transform.position.y < desiredLocation.position.y + maxDifInHeight - 1) && (transform.position.y > desiredLocation.position.y - maxDifInHeight + 1))
        {
            adjHeight = Random.Range(-4, 4);
        }
        //escaped top
        else
        {
            adjHeight = 0;
        }
        yield return new WaitForEndOfFrame();
    }
    IEnumerator RandomSpeedChange()
    {
        if (transform.position.x >= desiredLocation.position.x + rangeX - 1)
        {
            addSpeed = Random.Range(-4, 0);
        }
        else if (transform.position.x <= desiredLocation.position.x - rangeX + 1)
        {
            addSpeed = Random.Range(0, 4);
        }
        else
        {
            addSpeed = Random.Range(-4, 4);
        }
        yield return new WaitForEndOfFrame();
    }

    void OnTriggerStay2D(Collider2D plat)
    {
        if (plat.tag == "Ground")
        {
            overlapping = true;
            if (aStarDisabled && !fixingPath)
            {
                StartCoroutine(FixPathing());
            }
        }
    }
    void OnTriggerExit2D(Collider2D plat)
    {
        if (plat.tag == "Ground")
        {
            overlapping = false;
        }
    }
    IEnumerator FixPathing()
    {
        //so i know there will be some type of error if you have DL in a unaccessable zone but i need to test the base case
        fixingPath = true;
        aStarDisabled = false;
        objRigidbody.velocity = new Vector2(0, 0);
        GetComponent<Unit>().target = transform;
        yield return new WaitForSeconds(.5f);
        fixingPath = false;
        GetComponent<Unit>().target = desiredLocation;
    }
}

