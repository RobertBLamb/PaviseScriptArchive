using UnityEngine;
using System.Collections;

public class HeloControl : MonoBehaviour {

	public float heloSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x + heloSpeed, transform.position.y, transform.position.z);
	}
}
