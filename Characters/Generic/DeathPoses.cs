using UnityEngine;
using System.Collections;

public class DeathPoses : MonoBehaviour {


    public Sprite[] poses;
    public bool fallen = false;
    public bool multipleSprites;
    public float randLimit;
    private Rigidbody2D myRigidBody;
    //Fall angle in Degrees
    public float fallangle = 330;
    //Velocity to fall at
    public float vFall = 50;

    // Use this for initialization
    void Start()
    {
        //if (poses[0] == null)
        //needs to be updated to either not load sprites or
        //poses = Resources.LoadAll<Sprite>("Enemy Deads");

        if (myRigidBody == null)
            myRigidBody = GetComponent<Rigidbody2D>();

        fall();
        StartCoroutine(delete());

    }

    public void fall()
    {
        /*
        if (gameObject.GetComponent<Animator>() != null)
        {
            gameObject.GetComponent<Animator>().enabled = false;
        }
        */

        if (poses.Length == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        else if (poses.Length != 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = poses[
                Mathf.RoundToInt(Random.Range(0, poses.Length))];
        }

        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = poses[0];

        }

        if (myRigidBody != null)
        {
            myRigidBody.isKinematic = false;
            //GetComponent<Rigidbody2D>().transform.Rotate(0, 0, -90);
            //transform.rotation *= Quaternion.Euler(0, 0, 90 - (Random.value * 30));
            fallangle = fallangle + Random.Range(-5, 5);
            myRigidBody.velocity = new Vector2(vFall * Mathf.Cos(Mathf.Deg2Rad * fallangle), vFall * Mathf.Sin(Mathf.Deg2Rad * fallangle));
        }

    }

    IEnumerator delete()
    {
        yield return new WaitForSeconds(2f);
        GameObject.Destroy(this);
    }

}

