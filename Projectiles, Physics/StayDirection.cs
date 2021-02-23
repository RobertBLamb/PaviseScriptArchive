using UnityEngine;
using System.Collections;

public class StayDirection : MonoBehaviour
{

    Quaternion startingRotation;
    // Use this for initialization
    void Start()
    {
        startingRotation = transform.rotation;
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        //(Instantiate(Trail, transform.position, transform.rotation) as GameObject).transform.parent = transform;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = startingRotation;
        /*
        transform.position = transform.parent.position + new Vector3(.1F, .1F, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        */
    }
}
