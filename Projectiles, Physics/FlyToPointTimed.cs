using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyToPointTimed : MonoBehaviour {

    public Transform destinationPoint;
    public float flightTime;
    public float flightStep;

	// Use this for initialization
	void Start () {
        StartCoroutine(SkipToDestination());
        flightStep = (Time.deltaTime / flightTime) * Vector2.Distance(transform.position, destinationPoint.position);

    }

    // Update is called once per frame
    void Update () {

        float newFlightStep = (Time.deltaTime * flightStep);

        transform.position = Vector3.Lerp(transform.position, destinationPoint.position, flightStep); 
	}

    IEnumerator SkipToDestination()
    {
        yield return new WaitForSeconds(flightTime);
        transform.position = destinationPoint.transform.position;
    }
}
