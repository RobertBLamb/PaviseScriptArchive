using UnityEngine;
using System.Collections;

public class ChaserEnemy : MonoBehaviour
{
    //SCRIPT NEEDS WORK: ENEMY JITTERS WHEN SPEED FASTER THAN PLAYER'S, SHOULD RUN TO A MOVING TARGET OBJ INSTEAD.
    //7-17R cleaning up this mess increasing readablity in inspector
    private GameObject player;
    public Rigidbody2D objRigidbody;
    public Transform objTouched;
    public Transform groundpoint;
    public Transform visionStart;
    public Transform visionEnd;
    public Transform visionEndJump;
    public LayerMask layerIsRunnable;
    public LayerMask layerIsPlat;
    public RaycastHit2D hit;
    public float jumpForce;
    public float resetDistX;
    public float resetDistY;

    //Should prevent stuttering from not being exactly at minDistance
    private float minDistanceMargin = .5f;
    //time between adding jumpforce in wall courtines
    private float jumpInterval = 0.1f;
    private float jumpCD = .7f;
    private float jumpReset;
    [SerializeField]
    private float minDistance = 7;
    [SerializeField]
    private float maxDistance = 15;
    private float groundRad = 0.1f;
    private float controlResetTimer;
    [SerializeField]
    private float idealDist = 11;
    private float retreatJump;
    private float EnemyPosY;
    private bool controlReset;
    private bool onWall;
    private bool objSpotted;

    [HideInInspector]
    public bool control = true;
    //[HideInInspector]
    public float maxSpeed = 20;
    //[HideInInspector]
    public float speed;
    [HideInInspector]
    public float distToPlayerX;
    [HideInInspector]
    public float distToPlayerY;
    [HideInInspector]
    public float timeWOJump;
    [HideInInspector]
    public bool onRunnable;
    [HideInInspector]
    public float defaultMaxSpeed;
    //7-17R new gameobject for when touching walls helps with wall jump code
    [HideInInspector]
    public GameObject wallTouched;

    //different from my last version that worked
    public RaycastHit2D jumpGap;
    public RaycastHit2D groundCheck;
    //7-17R is it needed?
    public float vPlayer;
    public RaycastHit2D hitObstacle;
    public GameObject currentPlat;
    public RaycastHit2D[] hitPlats;
    public GameObject[] hitPlatObjs;

    public bool playerFurther;

    public float enviroLength;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        objRigidbody = GetComponent<Rigidbody2D>();
        defaultMaxSpeed = maxSpeed;

        transform.parent = null;
    }

    // Update is called once per frame
    //7-17R making heavy edits to readabilty, might cut some functionality you were trying to add
    //lots o bugs gotta start somewhere
    void FixedUpdate()
    {
        speed = objRigidbody.velocity.x;
        raycasting();
        behaviors();

        if (Physics2D.OverlapCircle(groundpoint.position, groundRad, layerIsRunnable))
        {
            onRunnable = true;
            timeWOJump += Time.deltaTime;
            if (controlResetTimer > .01f)
            {
                controlResetTimer = 0;
                controlReset = false;
                control = true;
            }
        }
        else
        {
            onRunnable = false;
            controlResetTimer += Time.deltaTime;
        }
        if (control)
        {
            //distance from player
            var xDistanceSigned = player.transform.position.x - transform.position.x;
            var xDistanceAbs = Mathf.Abs(xDistanceSigned);
            objRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            //shouldnt make a difference with jumping bug
            vPlayer = player.GetComponent<Rigidbody2D>().velocity.x;
            //Simply adds the player's current platform boost. The enemy would otherwise have to be smart enough to jump on faster plats to catch up
            maxSpeed = defaultMaxSpeed + player.GetComponent<PlayerController>().platSpeedX;

            if (xDistanceAbs > minDistance - minDistanceMargin && xDistanceAbs < minDistance + minDistanceMargin)
            {
                objRigidbody.velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x,
                    objRigidbody.velocity.y);
            }
            else if (xDistanceAbs < minDistance)
            {
                objRigidbody.velocity = new Vector2(-1 * maxSpeed * Mathf.Sign(xDistanceSigned) * Mathf.Sign(transform.localScale.x),
                    objRigidbody.velocity.y);
            }
            else if (xDistanceAbs > maxDistance)
            {
                objRigidbody.velocity = new Vector2(maxSpeed * Mathf.Sign(xDistanceSigned) * Mathf.Sign(transform.localScale.x),
                    objRigidbody.velocity.y);
            }
        }
        //if enemy is too far from player transfrom gets set to this
        distToPlayerX = Mathf.Abs(player.transform.position.x - objRigidbody.transform.position.x);
        distToPlayerY = Mathf.Abs(player.transform.position.y - objRigidbody.transform.position.y);
        if (distToPlayerX > resetDistX || distToPlayerY > resetDistY)
        {
            distToPlayerX = 0;
            distToPlayerY = 0;
            objRigidbody.transform.position = new Vector3(player.transform.position.x - 25, player.transform.position.y + 5, 0);
        }
    }
    //checks if player is farther than plat hit by raycast
    //7-17R testing new way to calc where player is, find more effective way to make call later
    public bool checkTransform()
    {
        //7-17R add variable for the 2 later, all it does is make sure that the stalker doesnt jump right away
        if (player.transform.position.x > hitObstacle.transform.GetComponent<Collider2D>().bounds.min.x + 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //7-17R all before verified works
    void behaviors()
    {
        //finds first gameobject in linecast
        if (hitObstacle.collider != null)
        {
            objTouched = hitObstacle.collider.transform;
        }
        else
        {
            objTouched = null;
        }
        //if player is to the right of stuff1 obj
        if (objTouched != null && onRunnable && checkTransform())
        {
            jumpReset += Time.deltaTime;
            //if on ground
            if (!controlReset && jumpReset >= jumpCD || !controlReset && timeWOJump >= jumpCD)
            {
                if (!onWall)
                {
                    standardJump();
                }
                else if (onWall)
                {
                    StartCoroutine(wallJumpRight());
                }
            }
        }
        //runs one of these if the player is to the left of the attached enemy
        else if (player.transform.position.x < objRigidbody.transform.position.x)
        {
            retreatJump += Time.deltaTime;
            if (retreatJump > 3f && !onWall)
            {
                retreatJump = 0;
                StartCoroutine("retreat");
            }
            else if (retreatJump > 3f && onWall)
            {
                retreatJump = 0;
                StartCoroutine("wallJumpLeft");
            }
        }
        //7-17R this is the line that enables the jump bug
        if (jumpGap.collider == null && onRunnable)
        {
            standardJump();
        }

    }
    void raycasting()
    {
        Debug.DrawLine(visionStart.position, visionEnd.position, Color.cyan);
        Debug.DrawLine(visionStart.position, visionEndJump.position, Color.red);
        hitObstacle = Physics2D.Linecast(visionStart.position, visionEnd.position, layerIsRunnable);
        jumpGap = Physics2D.Linecast(visionStart.position, visionEndJump.position, layerIsRunnable);
    }
    //7-17R temp out to help with readablity during bug fixing
    /*void raycastObstacles()
    {
        Debug.DrawLine(visionStart.position, visionEnd.position, Color.cyan);

        //      objSpotted = Physics2D.Linecast(visionStart.position, visionEnd.position, 1 << LayerMask.NameToLayer("Stuff1")) ||
        //            Physics2D.Linecast(visionStart.position, visionEnd.position, 1 << LayerMask.NameToLayer("Platforms1"));
        hitObstacle = Physics2D.Linecast(visionStart.position, visionEnd.position, 1 << LayerMask.NameToLayer("Stuff1"));
        
        if (hitObstacle.collider != null)
        {
            Debug.Log("The name is: " + hitObstacle.collider.name);
        }
        
    }*/
    /*void raycastPlats()
    
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
          
           if (currentPlat != null)
           {
               //Debug.Log("CurrentPlat: " + currentPlat.name);
           }
       }       */
    void standardJump()
    {
        objRigidbody.velocity = new Vector2(player.GetComponent<PlayerController>().moveSpeedMax, jumpForce);
        control = false;
        jumpReset = 0;
        timeWOJump = 0;
        controlReset = true;
    }

    void OnTriggerStay2D(Collider2D plat)
    {
        if (plat.tag == "Wall")
        {
            onWall = true;
            //7-17R new to make left wall jump work correctly
            wallTouched = plat.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D plat)
    {
        if (plat.tag == "Wall")
        {
            onWall = false;
        }
    }
    IEnumerator retreat()
    {
        control = false;
        objRigidbody.velocity = new Vector2(player.GetComponent<PlayerController>().moveSpeedMin, jumpForce);
        yield return new WaitForSeconds(1f);
        control = true;
        yield return new WaitForEndOfFrame();
    }
    IEnumerator wallJumpRight()
    {
        control = false;
        jumpReset = 0;
        timeWOJump = 0;
        controlReset = true;
        //7-17R modified to use the height of the wall and not the raycast
        EnemyPosY = wallTouched.transform.position.y + 4;
        //Debug.Log("active");
        while (transform.position.y < EnemyPosY)
        {
            objRigidbody.velocity = new Vector2(player.GetComponent<PlayerController>().moveSpeedMax, jumpForce);
            yield return new WaitForSeconds(jumpInterval);
        }
        //yield return new WaitForSeconds(1f);
        objRigidbody.velocity = new Vector2(player.GetComponent<PlayerController>().moveSpeedMax / 2, -15);
        yield return new WaitForSeconds(1f);

        yield return new WaitForEndOfFrame();
    }
    IEnumerator wallJumpLeft()
    {
        //Debug.Log("Memes");
        control = false;
        jumpReset = 0;
        timeWOJump = 0;
        controlReset = true;
        //7-17R see walljump right
        EnemyPosY = wallTouched.transform.position.y + 4;
        while (transform.position.y < EnemyPosY)
        {
            objRigidbody.velocity = new Vector2(player.GetComponent<PlayerController>().moveSpeedMin, jumpForce);
            yield return new WaitForSeconds(jumpInterval);
        }
        //yield return new WaitForSeconds(1f);
        objRigidbody.velocity = new Vector2(player.GetComponent<PlayerController>().moveSpeedMin / 2, -15);
        yield return new WaitForSeconds(1f);

        yield return new WaitForEndOfFrame();
    }
}