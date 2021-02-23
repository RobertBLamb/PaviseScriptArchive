using UnityEngine;
using System.Collections;

public class ScaleWithSpeed : MonoBehaviour {

    public GameObject movingObject;
    public Component scaleComponentObj;
    public Vector3 componentObjDefaultScale;

    public float speed;
    public float speed2D;
    public float maxSpeed;
    public float speedProportion;

    public float lowSpeedRange;

    // Use this for initialization
    void Start () {

        //PlayerController specific
        maxSpeed = movingObject.GetComponent<PlayerController>().moveSpeedMax;

	}
	
	// Update is called once per frame
	void Update () {
        AutoScale();
	}

    void OnEnable()
    {
        AutoScale();
    }

    void AutoScale()
    {
        //speed = movingObject.GetComponent<PlayerController>().moveSpeed;
        speed2D = movingObject.GetComponent<Rigidbody2D>().velocity.magnitude;

        if (Mathf.Abs(speed2D) > lowSpeedRange && scaleComponentObj.transform.localScale.magnitude <= componentObjDefaultScale.magnitude * 1.5)
        {
            speedProportion = Mathf.Abs(
                //speed
                (speed2D/4)/ maxSpeed);
            scaleComponentObj.transform.localScale = componentObjDefaultScale * (1 + speedProportion*.5f);
        }
        else

        {
            scaleComponentObj.transform.localScale = componentObjDefaultScale;
        }
    }
}
