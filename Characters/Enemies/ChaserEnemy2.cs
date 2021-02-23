using UnityEngine;
using System.Collections;

public class ChaserEnemy2 : MonoBehaviour
{

    //SCRIPT NEEDS WORK: ENEMY JITTERS WHEN SPEED FASTER THAN PLAYER'S, SHOULD RUN TO A MOVING TARGET OBJ INSTEAD.

    private GameObject player;
    public Transform objTouched;
    public GameObject thisEnemy;
    public float msImport;
    public RaycastHit2D hit;
    public float maxSpeed = 15;
    public float minDistance = 5;
    public float maxDistance = 10;
    public float speedAdd;
    public Rigidbody2D objRigidbody;
    public bool isKinematic;
    public Transform visionEnd;
    public float distToPlayer;
    public float resetDist;
    private float jumpCD = .7f ;
    private float jumpReset;
    public float timeWOJump;
    public bool objSpotted;
    private float idealDist = 11;

    public Transform groundpoint;
    public LayerMask layerIsRunnable;
    public LayerMask layerIsPlat;
    public bool onRunnable;
    public bool playerFurthur;
    private float groundRad = 0.1f;
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
        //playerFurthur = checkTransform();
        raycasting();
        behaviors();
        if (Physics2D.OverlapCircle(groundpoint.position, groundRad, layerIsRunnable))
        {
            onRunnable = true;
        }
        else
        {
            onRunnable = false;
        }
        if(onRunnable)
        {
            timeWOJump += Time.deltaTime;
        }
        var xDistanceSigned = player.transform.position.x - transform.position.x;
        var xDistanceAbs = Mathf.Abs(xDistanceSigned);

        //If this enemy is more than maxDistance away from player
        //Positive: enemy is to the right of player
        //Negative: enemy is to the left of player
        msImport = player.GetComponent<PlayerController>().moveSpeed;
        //objRigidbody.velocity = new Vector2(msImport, 0);
        objRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (xDistanceAbs > minDistance - minDistanceMargin && xDistanceAbs < minDistance + minDistanceMargin)
        {
            objRigidbody.velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, objRigidbody.velocity.y);
        }

        else if (xDistanceAbs < minDistance)
        {
            objRigidbody.velocity = new Vector2(-1 * maxSpeed * Mathf.Sign(xDistanceSigned) * transform.localScale.x, objRigidbody.velocity.y);
        }
        else if (xDistanceAbs > maxDistance)
        {
            objRigidbody.velocity = new Vector2(maxSpeed * Mathf.Sign(xDistanceSigned) * transform.localScale.x, objRigidbody.velocity.y);
        }
        //playerFurthur = checkTransform();
        distToPlayer = Mathf.Abs(player.transform.position.x - objRigidbody.transform.position.x);
        if(distToPlayer>resetDist)
        {
            distToPlayer = 0;
            objRigidbody.transform.position = new Vector3(player.transform.position.x-25,player.transform.position.y +5,0);
        }
    }
    //checks if player is farther than plat hit by raycast
    public bool checkTransform()
    {
        if(player.transform.position.x>hit.transform.position.x)
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
        if(hit.collider != null)
        {
            objTouched = hit.collider.transform;
        }
        else
        {
            objTouched = null; 
        }

        if (objTouched != null && onRunnable && checkTransform())
        {

            jumpReset += Time.deltaTime;
            if (jumpReset >= jumpCD||timeWOJump>=jumpCD)
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

    void raycasting()
    {
        Debug.DrawLine(visionStart.position, visionEnd.position, Color.cyan);
        //      objSpotted = Physics2D.Linecast(visionStart.position, visionEnd.position, 1 << LayerMask.NameToLayer("Stuff1")) ||
        //            Physics2D.Linecast(visionStart.position, visionEnd.position, 1 << LayerMask.NameToLayer("Platforms1"));
        hit = Physics2D.Linecast(visionStart.position, visionEnd.position, 1<<LayerMask.NameToLayer("Stuff1"));
        if(hit.collider != null)
        {
            Debug.Log("The name is: " + hit.collider.name);
        }
    }
}
