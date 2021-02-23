using UnityEngine;
using System.Collections;

public class HitPoses : MonoBehaviour {

    public Sprite defaultSprite;
    public Sprite[] hitPoses;
    public bool poseChanged = false;
    public bool poseDefaulted = false;
    public GameObject[] childrenWSprites;
    //Transform[] allChildren;

    public int counter = 0;

    // Use this for initialization
    void Start ()
    {
        defaultSprite = GetComponent<SpriteRenderer>().sprite;
        /*
        if (transform.childCount > 0)
            allChildren = GetComponentsInChildren<Transform>();
        */
	}
	
	void FixedUpdate () {
        if (counter > hitPoses.Length)
            counter = 0;

        //Currently based on when flickering starts, should be called every time a projectile hits
        if (GetComponent<Health>().flickering && !poseChanged)
        {
            GetComponent<Animator>().enabled = false;
            /*
            if (allChildren.Length > 0)
            {
                foreach (Transform childSpriteRend in allChildren)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            */
            if (childrenWSprites.Length > 0)
            {
                for (int i = 0; i < childrenWSprites.Length; i++)
                    if (childrenWSprites[i] != null)
                    childrenWSprites[i].GetComponent<SpriteRenderer>().enabled = false;
            }
            //For now cycles straight thru poses, not random
            if (counter >= hitPoses.Length)
                counter = 0;
            GetComponent<SpriteRenderer>().sprite = hitPoses[counter];
            counter++;
            poseChanged = true;
        }

        if (!GetComponent<Health>().flickering && poseChanged
            //&& !poseDefaulted
            )
        {
            GetComponent<Animator>().enabled = true;
            /*
            if (allChildren.Length > 0)
            {
                foreach (Transform childSpriteRend in allChildren)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                }
            }
            */
            if (childrenWSprites.Length > 0)
            {
                for (int i = 0; i < childrenWSprites.Length; i++)
                    if(childrenWSprites[i] != null)
                    childrenWSprites[i].GetComponent<SpriteRenderer>().enabled = true;
            }
            poseChanged = false;
            //poseDefaulted = true;
        }
    }
}
