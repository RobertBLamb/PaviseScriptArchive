using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustCameraOnDeath : MonoBehaviour
{

    private GameObject mainCamObj;
    private CameraController mainCamController;

    public GameObject target;

    public Vector2 focusAreaSize;
    public float zValue;

    public float vertOffSet;
    public bool adjustTrigger;
    public bool adjusted;

    // Use this for initialization
    void Start()
    {
        mainCamObj = gameObject;
        //GameObject.FindGameObjectWithTag("MainCamera");
        mainCamController =
            //mainCamObj.
            GetComponent<CameraController>();
    }

    // Update is called once per frame
    //TEMP AS HELL, MADE JUST FOR RECORDING
    void FixedUpdate()
    {
            if (target != null)
            {
                if (target.GetComponent<Health>().health <= 0)
                {
                    adjustTrigger = true;
                }
            }
        if (adjustTrigger)
        {
            if (mainCamObj.transform.position.z >= zValue)
            {
                mainCamController.zValue = zValue;
                mainCamObj.transform.position = new Vector3(mainCamObj.transform.position.x, mainCamObj.transform.position.y, mainCamObj.transform.position.z - .5f);
            }
            if (mainCamController.vertOffSet >= vertOffSet)
                mainCamController.vertOffSet -= .1f;
        }
    }
}

