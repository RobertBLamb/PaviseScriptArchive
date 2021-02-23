using UnityEngine;
using System.Collections;

public class CrushTruck : MonoBehaviour {
    public float truckSpeed;
    public bool setPos;
	// Use this for initialization
	void Start () {
        setPos = false;
        GetComponent<Rigidbody2D>().velocity = transform.rotation * new Vector2(truckSpeed, GetComponent<Rigidbody2D>().velocity.y);
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y >= -5)
        {
            {
                truckSpeed = 0;

            }

        }

        if (transform.position.y >= -1)
        {
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<Health>().health = 0;
            GameObject.FindGameObjectWithTag("Enemy1").GetComponent<Health>().health = 0;
            GameObject.Find("SlammableBox").GetComponent<PolygonCollider2D>().enabled = false;
            GameObject.Find("ProperCollider").GetComponent<PolygonCollider2D>().enabled = true;
            GameObject.Find("ProperCollider").GetComponent<Rigidbody2D>().isKinematic = false;


            /*
            if (setPos == false)
            transform.Translate(GameObject.FindGameObjectWithTag("Player").transform.position.x + 1, transform.position.y, transform.position.z);
            setPos = true;
            */
        }



    }
}
