using UnityEngine;
using System.Collections;

public class ShowWorldRotation : MonoBehaviour {
    public Vector3 currentRotation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        currentRotation = transform.rotation.eulerAngles;
    }
}
