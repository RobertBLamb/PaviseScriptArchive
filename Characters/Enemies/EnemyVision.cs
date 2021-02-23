using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour {

    public bool sighted;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D plat)
    {
        if (plat.tag == "Player")
        {
            sighted = true;
        }
    }
    void OnTriggerExit2D(Collider2D plat)
    {
        if (plat.tag == "Player")
        {
            sighted = false;
        }
    }
}
