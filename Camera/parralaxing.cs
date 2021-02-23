using UnityEngine;
using System.Collections;

public class parralaxing : MonoBehaviour {

    public Transform[] backgrounds;
    private float[] parallaxScales;
    public float smoothing;
    //public Transform cam;

    private Vector3 previousCamPos;


    void awake()
    {
        //cam = Camera.main.transform;
    }

	// Use this for initialization
	void Start ()
    {
        previousCamPos = transform.position;

        parallaxScales = new float[backgrounds.Length];

        for(int i=0; i<parallaxScales.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        //Debug.Log(cam.position);
	    for(int i=0; i<backgrounds.Length; i++)
        {
            Vector3 parallax = (previousCamPos - transform.position) * (parallaxScales[i] / smoothing);

            backgrounds[i].position = new Vector3(backgrounds[i].position.x + parallax.x, backgrounds[i].position.y + parallax.y, backgrounds[i].position.z);

            //float backgroundTargetX = backgrounds[i].position.x + parallax;

            //Vector3 backgroundTarget = new Vector3(backgroundTargetX, backgrounds[i].position.y, backgrounds[i].position.z);


        }

        previousCamPos = transform.position;
	}
}
