using UnityEngine;
using System.Collections;

public class ButtonTriggerDoor : MonoBehaviour {
    public GameObject gateTop;
    public GameObject gateBottom;
    public float gateDestinationY;
    public float gateSpeedY;
    public bool arrived;
    public bool opening;

    private float ogYTop;
    private float ogYBottom;
    private float modYTop;
    private float modYBottom;

	// Use this for initialization
	void Start ()
    {
        ogYTop = gateTop.transform.position.y;
        ogYBottom = gateBottom.transform.position.y;
        modYTop = gateTop.transform.position.y + gateDestinationY;
        modYBottom = gateBottom.transform.position.y - gateDestinationY;

    }
	
	// Update is called once per frame
	void Update ()
    {
        //platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * movespeed)

        if (opening && !arrived)
        {
            //Debug.Log("opening");
            gateTop.transform.position = Vector3.MoveTowards(gateTop.transform.position, new Vector3(gateTop.transform.position.x, modYTop, gateTop.transform.position.z), Time.deltaTime * gateSpeedY);
            gateBottom.transform.position = Vector3.MoveTowards(gateBottom.transform.position, new Vector3(gateBottom.transform.position.x, modYBottom, gateBottom.transform.position.z), Time.deltaTime * gateSpeedY);
        }
        else if(!opening && !arrived)
        {
            //Debug.Log("closing");
            gateTop.transform.position = Vector3.MoveTowards(gateTop.transform.position, new Vector3(gateTop.transform.position.x, ogYTop, gateTop.transform.position.z), Time.deltaTime * gateSpeedY);
            gateBottom.transform.position = Vector3.MoveTowards(gateBottom.transform.position, new Vector3(gateBottom.transform.position.x, ogYBottom, gateBottom.transform.position.z), Time.deltaTime * gateSpeedY);
        }
        if(gateTop.transform.position.y == modYTop)
        {
            arrived = true;
        }
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Heavy_Object")
        {
            //Debug.Log("Enter");
            opening = true;
            arrived = false;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Heavy_Object")
        {
            //Debug.Log("Exit");
            opening = false;
            arrived = false;
        }
    }
}
