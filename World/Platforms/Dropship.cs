using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropship : MonoBehaviour {

    public Rigidbody2D rb;
    public SquadManager squadManager;
    public GameObject skiff;
    public GameObject door;
    public GameObject[] thrusters;

    //Flags for movement stages. Consider making them an enum.
    [SerializeField]
    protected enum DropShipState
    {
        stopped,
        flyingIn,
        flyingOut,
        landing
    }
    [SerializeField]
    protected DropShipState state;

    [SerializeField]
    protected bool moving;

    public Coroutine currentCoroutine;

    //Transform of where to stop
    [SerializeField]
    protected Transform dropPoint;
    [SerializeField]
    protected Transform exitPoint;
    [SerializeField]
    protected float dropPointVertOffset = 2;
    [SerializeField]
    public float moveVel;
    public float maxMoveVel = 15;
    public float minMoveVel = 5;

    public Vector2 moveDir;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        state = DropShipState.stopped;

        FlyToPoint(dropPoint);
        //flyIn =  StartCoroutine(FlyIn());
	}
	
	void FixedUpdate ()
    {
        if (moving)
        {
            rb.velocity = moveVel * moveDir;
        }
        if (state == DropShipState.flyingIn)
        {
            if (Mathf.Abs(transform.position.x - dropPoint.position.x) <  .5f)
            {
                Land();
            }
        }
        if (state == DropShipState.landing)
        {
            if (Mathf.Abs(transform.position.y - (dropPoint.position.y + dropPointVertOffset)) < .5f)
            {
                Stop();
                if (squadManager != null)
                {
                    currentCoroutine = StartCoroutine(DropSquad());
                }
                /*
                if (skiffController != null)
                {
                  currentCoroutine = StartCoroutine(DropSkiff());
                }
                */
                //FlyToPoint(exitPoint);
            }
        }
    }

    void Stop()
    {
        state = DropShipState.stopped;
        moving = false;
        moveVel = 0;
        moveDir = Vector2.zero;
        rb.velocity = moveVel * moveDir;
    }

    //Set params to get to drop zone
    void FlyToPoint(Transform targetPoint)
    {
        moving = true;
        state = DropShipState.flyingIn;

        moveVel = maxMoveVel;

        if (transform.position.x - targetPoint.position.x < 0)
        {
            moveDir = Vector2.right;
        }
        else
        {
            moveDir = Vector2.left;
        }
    }

    //Take some time to land down
    //Start branch
    void Land()
    {
        moving = true;
        state = DropShipState.landing;

        moveVel = minMoveVel;

        if (transform.position.y - dropPoint.position.y > 0)
        {
            moveDir = Vector2.down;
        }
        else
        {
            moveDir = Vector2.up;
        }
    }

    void TakeOff(Transform targetPoint)
    {
        moving = true;
        state = DropShipState.flyingOut;

        moveVel = maxMoveVel;

        if (transform.position.y - targetPoint.position.y > 0)
        {
            moveDir = Vector2.down;
        }
        else
        {
            moveDir = Vector2.up;
        }
    }

    void UpdateCurve()
    {
        //Would need to update velocity with Bezier motion curve every FixedUpdate
    }

    IEnumerator DropSquad()
    {
        squadManager.ActivateMembers();
        yield return new WaitForSeconds(1);
        TakeOff(exitPoint);
    }

    IEnumerator DropSkiff()
    {
        yield return new WaitForSeconds(1);

    }
}
