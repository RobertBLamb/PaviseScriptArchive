using UnityEngine;
using System.Collections;

public class DeathOnePose : MonoBehaviour
{

    public Sprite pose;
    public bool fallen;

    // Use this for initialization
    void Start()
    {
        fallen = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (fallen)
        //    fall();

    }

    public void fall()
    {
        fallen = true;
                gameObject.GetComponent<SpriteRenderer>().sprite = pose;

    }

}

