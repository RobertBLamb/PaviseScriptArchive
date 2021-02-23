using UnityEngine;
using System.Collections;

public class LowerGate_BallStuck : MonoBehaviour {
    public GameObject heavyObj;
    public GameObject[] movingObjs;
    public GameObject objToMoveToY;
    public Vector2[] boundsMin;
    public Vector2[] boundsMax;
    public bool arrived;
    public float moveSpeed;
	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update ()
    {       
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Heavy_Object")
        {
            heavyObj = other.gameObject;
            StartCoroutine("LowerGates");
        }
    }

    IEnumerator LowerGates()
    {
        //Debug.Log("Lezz Go");
        while (!arrived)
        {
            for (int i = 0; i < movingObjs.Length; i++)
            {
                boundsMin[i] = movingObjs[i].GetComponent<BoxCollider2D>().bounds.min;
                boundsMax[i] = movingObjs[i].GetComponent<BoxCollider2D>().bounds.max;
            }
            yield return new WaitForEndOfFrame();
            //add check to see if arrived
            arrived = true;
            for(int i= 0; i<movingObjs.Length; i++)
            {
                if(Mathf.Abs(boundsMax[i].y-objToMoveToY.GetComponent<BoxCollider2D>().bounds.max.y)>.13)
                {
                    movingObjs[i].transform.position = Vector3.MoveTowards(movingObjs[i].transform.position,
                        new Vector3(movingObjs[i].transform.position.x, objToMoveToY.GetComponent<BoxCollider2D>().bounds.min.y, movingObjs[i].transform.position.z), Time.deltaTime * moveSpeed);
                    arrived = false;
                }
            }
        }
        //Debug.Log("Donezo");
        yield return new WaitForEndOfFrame();
    }
}
