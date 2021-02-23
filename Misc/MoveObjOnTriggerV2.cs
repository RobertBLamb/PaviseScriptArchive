using UnityEngine;
using System.Collections;

public class MoveObjOnTriggerV2 : MonoBehaviour
{
    public GameObject heavyObj;
    public GameObject[] movingObjs;
    public GameObject[] objToEnable;
    public GameObject[] objToDisable;
    public Transform[] destination;
    public bool arrived;
    public float moveSpeed;
    public float approvedRange;
    // Use this for initialization
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Heavy_Object")//|| other.tag=="Player")
        {
            heavyObj = other.gameObject;
            StartCoroutine("LowerGates");
        }
    }

    IEnumerator LowerGates()
    {
        //Debug.Log("Lezz Go");
        for(int i = 0; i<objToDisable.Length; i++)
        {
            objToDisable[i].SetActive(false);
        }
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < objToEnable.Length; i++)
        {
            objToEnable[i].SetActive(true);
        }
        yield return new WaitForEndOfFrame();

        while (!arrived)
        {
            arrived = true;
            for (int i = 0; i < movingObjs.Length; i++)
            {
                float dist = Vector3.Distance(movingObjs[i].transform.position, destination[i].transform.position);
                Debug.Log(dist);
                if (dist>approvedRange)
                {
                    movingObjs[i].transform.position = Vector3.MoveTowards(movingObjs[i].transform.position, 
                        destination[i].transform.position, Time.deltaTime * moveSpeed);
                    arrived = false;
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        //Debug.Log("Donezo");
        yield return new WaitForEndOfFrame();
    }
}
