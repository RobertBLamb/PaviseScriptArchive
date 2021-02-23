using UnityEngine;
using System.Collections;

public class Stairlevator : MonoBehaviour
{

    public GameObject player;
    public GameObject platform;
    public float initialPlatY;
    public Collider2D catchPlayerTrigger;
    public Transform lowPoint;
    public Transform highPoint;

    public float playerXDist;
    public float platHeight;
    public float tanOfSlopeAngle;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialPlatY = platform.transform.position.y;
        tanOfSlopeAngle = (highPoint.position.y - lowPoint.position.y) / Mathf.Abs(highPoint.position.x - lowPoint.position.x);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //if (other.collider.gameObject == player)
        if (other.gameObject == player)

            //if (other == player)
        {
            playerXDist = Mathf.Abs(player.transform.position.x - lowPoint.position.x);
            platHeight = playerXDist * tanOfSlopeAngle + initialPlatY;
            platform.transform.position = new Vector3(player.transform.position.x, platHeight, platform.transform.position.z);
        }
    }
}
