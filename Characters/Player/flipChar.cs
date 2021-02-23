using UnityEngine;
using System.Collections;

public class flipChar : MonoBehaviour {
    public bool facingRight = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (GetComponent<PlayerController>().control == true)
        {
            if ((Input.GetKey(KeyCode.D)) && !facingRight && GetComponent<PlayerController>().moveSpeed>0)
            {
                //transform.localRotation = new Quaternion(0, 0, 0, 0);
                facingRight = true;
                //transform.rotation = Quaternion.identity;
            }

            if (Input.GetKey(KeyCode.A) && facingRight && GetComponent<PlayerController>().moveSpeed<0)
            {
                //transform.localRotation = new Quaternion(0, 180, 0, 0);
                facingRight = false;
                //transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
