using UnityEngine;
using System.Collections;

//WIP: Supposed to disable spawned once player reaches certain point

public class DisableObjectTrigger : MonoBehaviour {

    public GameObject objectToDisable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void onTriggerEnter2D(Collider2D other ) {
        if (other.CompareTag("Player"))
        {
            objectToDisable.SetActive(false);
        }
    }
}
