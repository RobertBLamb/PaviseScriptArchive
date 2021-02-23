using UnityEngine;
using System.Collections;

public class ShowRBVelocity : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(gameObject.name);
        Debug.Log(GetComponent<Rigidbody2D>().velocity);
    }
}
