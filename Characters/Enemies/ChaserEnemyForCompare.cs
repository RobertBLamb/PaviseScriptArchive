using UnityEngine;
using System.Collections;

public class ChaserEnemyForCompare : MonoBehaviour
{
    /*
    //SCRIPT NEEDS WORK: ENEMY JITTERS WHEN SPEED FASTER THAN PLAYER'S, SHOULD RUN TO A MOVING TARGET OBJ INSTEAD.

    private GameObject player;
    public Transform objTouched;
    public float jumpInterval;
    public float speed;
    public RaycastHit2D hit;

    public GameObject thisEnemy;
    public float vPlayer;
    public RaycastHit2D hitObstacle;
    public GameObject currentPlat;
    public RaycastHit2D[] hitPlats;
    public GameObject[] hitPlatObjs;

    public float defaultMaxSpeed = 15;
    public float maxSpeed = 15;

    public float speedAdd;
    public Rigidbody2D objRigidbody;
    public bool isKinematic;
    public Transform visionEnd;
    public float distToPlayerX;
    public float resetDistX;
    public float distToPlayerY;
    public float resetDistY;
    public float timeWOJump;
    private bool objSpotted;
    public Transform visionStart;
    public float retreatJump;
    public bool control = true;

    public float xDistanceSigned;
    float xDistanceAbs;

    public float distToPlayer;
    public float resetDist;


    public Transform groundpoint;
    public LayerMask layerIsRunnable;
    public LayerMask layerIsPlat;
    public bool onRunnable;
    public bool playerFurther;

    private float jumpCD = .7f;
    private float jumpReset;
    public float minDistance = 5;
    public float maxDistance = 10;
    private float groundRad = 0.1f;

    public float timeWOJump;
    public bool objSpotted;
    private float idealDist = 11;

    public Transform visionStart;

    //Should prevent stuttering from not being exactly at minDistance
    public float minDistanceMargin = .5f;
    public float jumpForce;
    public float enviroLength;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        objRigidbody = GetComponent<Rigidbody2D>();
        //isKinematic = objRigidbody.isKinematic;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = objRigidbody.velocity.x;
        //playerFurthur = checkTransform();
        raycastObstacles();
        raycastPlats();
        behaviors();

        if (groundpoint != null)
        {
            if (Physics2D.OverlapCircle(groundpoint.position, groundRad, layerIsRunnable))
            {
                onRunnable = true;
            }
            else
            {
                onRunnable = false;
            }
            if (onRunnable)
            {
                timeWOJump += Time.deltaTime;
            }
        }

        //If this enemy is more than maxDistance away from player
        //Positive: enemy is to the right of player
        //Negative: enemy is to the left of player
        vPlayer = player.GetComponent<Rigidbody2D>().velocity.x;

        //Simply adds the player's current platform boost. The enemy would otherwise have to be smart enough to jump on faster plats to catch up
        maxSpeed = defaultMaxSpeed + player.GetComponent<PlayerController>().moveSpeedMod;
        //objRigidbody.velocity = new Vector2(msImport, 0);
        objRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;


        if (xDistanceAbs > maxDistance)
        {
            objRigidbody.velocity = new Vector2(maxSpeed * Mathf.Sign(xDistanceSigned) * Mathf.Sign(transform.localScale.x), objRigidbody.velocity.y);
        }

        else if (xDistanceAbs < minDistance)
        {
            objRigidbody.velocity = new Vector2(-1 * maxSpeed * Mathf.Sign(xDistanceSigned) * Mathf.Sign(transform.localScale.x), objRigidbody.velocity.y);
        }

        else if (xDistanceAbs > minDistance - minDistanceMargin && xDistanceAbs < minDistance + minDistanceMargin)
        {
            objRigidbody.velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, objRigidbody.velocity.y);
        }


        //playerFurther = checkTransform();
        //distToPlayer = Mathf.Abs(player.transform.position.x - objRigidbody.transform.position.x);
        if (xDistanceAbs > resetDist)
        {
            //xDistanceAbs = 0;
            objRigidbody.transform.position = new Vector3(player.transform.position.x - 25, player.transform.position.y + 5, 0);
        }
    }

    //checks if player is farther than plat hit by raycast
    public bool checkTransform()
    {
        if (player.transform.position.x > hitObstacle.transform.position.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void behaviors()
    {
        if (hitObstacle.collider != null)
        {
            objTouched = hitObstacle.collider.transform;
        }
        else
        {
            objTouched = null;
        }

        if (objTouched != null && onRunnable && checkTransform())
        {

            jumpReset += Time.deltaTime;
            if (jumpReset >= jumpCD || timeWOJump >= jumpCD)
            {
                objRigidbody.velocity = new Vector2(objRigidbody.velocity.x, jumpForce);
                jumpReset = 0;
                timeWOJump = 0;
            }
        }
        else
        {

        }
    }

    void raycastObstacles()
    {
        Debug.DrawLine(visionStart.position, visionEnd.position, Color.cyan);
        //      objSpotted = Physics2D.Linecast(visionStart.position, visionEnd.position, 1 << LayerMask.NameToLayer("Stuff1")) ||
        //            Physics2D.Linecast(visionStart.position, visionEnd.position, 1 << LayerMask.NameToLayer("Platforms1"));
        hitObstacle = Physics2D.Linecast(visionStart.position, visionEnd.position, 1 << LayerMask.NameToLayer("Stuff1"));
        /*
        if (hitObstacle.collider != null)
        {
            Debug.Log("The name is: " + hitObstacle.collider.name);
        }
        */
     /*
    }
    void raycastPlats()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - 2, transform.position.z), Color.yellow);
        //      objSpotted = Physics2D.Linecast(visionStart.position, visionEnd.position, 1 << LayerMask.NameToLayer("Stuff1")) ||
        //            Physics2D.Linecast(visionStart.position, visionEnd.position, 1 << LayerMask.NameToLayer("Platforms1"));
        hitPlats = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y), Vector2.down);

        for (int i = 0; i < hitPlats.Length; i++)
        {
            //hitPlatObjs[i] = hitPlats[i].collider.gameObject;

            if (hitPlats[i].collider.tag == ("Platform"))
            //|| hitPlats[i].collider.tag == ("thinPlatform"))
            {
                //Parent is actual moving object
                currentPlat = hitPlats[i].collider.transform.parent.gameObject;
                break;
            }
        }
        /*
        if (currentPlat != null)
        {
            Debug.Log("CurrentPlat: " + currentPlat.name);
        }
    }
*/
}
