using UnityEngine;
using System.Collections;

public class DashLauncher : MonoBehaviour
{
    //8-17R notes not done yet
    //make angle and speed consistent regardless of where you enter
    public Transform lowPoint;
    public Transform highPoint;
    public GameObject player;
    public float slopeDashSpeed;
    public float slopeAngle;
    public float slopeDashTime;
    //filler time value to dash player until trigger exit
    public float slopeRideDashTime = 1000;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //8-17R changed to player check for more consistency
        if (other.CompareTag("Player"))
        {
            if (player.GetComponent<PlayerSpecialMoves>().dashing)
            {
                reDash(slopeRideDashTime);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        //8-17R changed to player check for more consistency
        if (other.CompareTag("Player"))
        {
            if (player.GetComponent<PlayerSpecialMoves>().dashing)
            {
                reDash(slopeRideDashTime);
            }
        }
    }

    void reDash(float thisDashTime)
    {
        slopeAngle = Mathf.Rad2Deg * Mathf.Atan2((highPoint.position.y - lowPoint.position.y), Mathf.Abs(highPoint.position.x - lowPoint.position.x));
        //7-17R testing
        //StopCoroutine(player.GetComponent<PlayerSpecialMoves>().shieldDash(0,0,0));
        //8-17R definite fix to to make sure only one dash is active
        player.GetComponent<PlayerSpecialMoves>().StopAllCoroutines();
        player.GetComponent<PlayerSpecialMoves>().dashing = true;
        StartCoroutine(player.GetComponent<PlayerSpecialMoves>().shieldDash(slopeDashSpeed, slopeAngle, slopeDashTime));
        Debug.Log("dashing throught tilt");
    }
}