using UnityEngine;
using System.Collections;

public class FollowVelocity : MonoBehaviour {

    //SCRIPT IS NOT REAL YET!!!!!!!!!!!!!!!!!!!!!!!!!

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.rotation = Quaternion.Euler(0, 0, 20);

    }
}
