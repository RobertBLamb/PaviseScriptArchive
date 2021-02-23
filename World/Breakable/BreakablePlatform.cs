using UnityEngine;
using System.Collections;

public class BreakablePlatform : MonoBehaviour {

    public GameObject player;


	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Health>().health <= 0 && player.transform.parent == transform)
        {
            player.transform.parent = null;
        }
	}
}
