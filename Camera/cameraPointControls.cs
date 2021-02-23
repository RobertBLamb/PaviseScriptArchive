using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPointControls : MonoBehaviour {
    public PlayerController thePlayer;
    public CameraController cam;
    public Vector2 mouse;
    public int screenW;
    public int screenH;
    public float ratioLimit;
    public float screenCursorRatioX;
    //Max distances the player can look either way
    public float maxRight = 12;
    public float maxLeft = -10;
    public float pointerToSideTime;
    public float scenario;
    public float pointerDist;
    public float temp;
    public float lookSmoothX;

    float smoothLookVelocityX;

    // Use this for initialization
    void Start () {

        if (thePlayer == null)
            thePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (cam == null)
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        //gets screen size for ratio
        screenW = Screen.width;
        screenH = Screen.height;

    }
	void Update()
    {
        //tldr says where on screen mouse is
        mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        //Goes from .5 (cursor at left edge of screen) to 0 (cursor at midscreen) to -.5 (cursor at right edge)
        screenCursorRatioX = -1 * ((screenW / 2 - mouse.x) / screenW);
        //Good ratio to move at is ~ .3-.32
        if (screenCursorRatioX > .5f)
            screenCursorRatioX = .5f;
        else if (screenCursorRatioX < -.5f)
            screenCursorRatioX = -.5f;
    }
	// Update is called once per frame
	void LateUpdate () {
        temp = thePlayer.moveSpeed;
        cam.pointerDist = pointerDist;
        //if mouse is far enough do x
        if (Mathf.Abs(screenCursorRatioX) > ratioLimit)
        {
            pointerToSideTime += Time.deltaTime;
        }
        //case 0 looking to the right and moving right
        if (screenCursorRatioX > ratioLimit &&  temp > 0)
        {
            if (scenario != 0)
            {
                scenario = 0;
                //pointerToSideTime = 0;
            }
            else
            {
                //if (pointerToSideTime <= lookSmoothX)
                    pointerDist = Mathf.Lerp(pointerDist, maxRight, 2 * Time.deltaTime);
                        //pointerDist = Mathf.SmoothDamp(pointerDist, 20, ref smoothLookVelocityX, lookSmoothX);
            }
        }
        //case 1 looking to the right and moving left
        else if (screenCursorRatioX > ratioLimit &&  temp < 0)
        {
            if (scenario != 1)
            {
                scenario = 1;
                pointerToSideTime = 0;
            }
            else
            {
                pointerDist = Mathf.Lerp(pointerDist, maxRight, 2 * Time.deltaTime);
                //if(pointerToSideTime <= lookSmoothX)
                //pointerDist = Mathf.SmoothDamp(pointerDist, 20, ref smoothLookVelocityX, lookSmoothX);
            }
        }
        //case 2 looking left moving left
        else if (screenCursorRatioX < -ratioLimit && temp < 0)
        {
            if (scenario != 2)
            {
                scenario = 2;
                pointerToSideTime = 0;
            }
            else
            {
                pointerDist = Mathf.Lerp(pointerDist, maxLeft, 2 * Time.deltaTime);
                //if (pointerToSideTime <= lookSmoothX)
                //  pointerDist = Mathf.SmoothDamp(pointerDist, -20, ref smoothLookVelocityX, lookSmoothX);
            }
        }
        //case 3 looking left moving right
        else if (screenCursorRatioX < -ratioLimit && temp > 0)
        {
            if (scenario != 3)
            {
                scenario = 3;
                pointerToSideTime = 0;
            }
            else
            {
                pointerDist = Mathf.Lerp(pointerDist, maxLeft, 2 * Time.deltaTime);
                //if (pointerToSideTime <= lookSmoothX)
                //  pointerDist = Mathf.SmoothDamp(pointerDist, -20, ref smoothLookVelocityX, lookSmoothX);
            }
        }
        //case 4 at standstill
        else if (Mathf.Abs(screenCursorRatioX) < ratioLimit)
        {
            if (scenario != 4)
            {
                scenario = 4;
                pointerToSideTime = 0;
            }
            else
            {
                pointerDist = Mathf.Lerp(pointerDist, 0, 2 * Time.deltaTime);
                //pointerDist = Mathf.SmoothDamp(pointerDist, 0, ref smoothLookVelocityX, lookSmoothX);
            }
        }    
        //case 5 at standstill looking left
        else if (screenCursorRatioX < -ratioLimit && temp == 0)
        {
            if (scenario != 5)
            {
                scenario = 5;
                pointerToSideTime = 0;
            }
            else
            {
                pointerDist = Mathf.Lerp(pointerDist, maxLeft, 2 * Time.deltaTime);
                //if (pointerToSideTime <= lookSmoothX)
                //  pointerDist = Mathf.SmoothDamp(pointerDist, -15, ref smoothLookVelocityX, lookSmoothX);
            }
        }
        //case 6 at standstill looking right
        else if (screenCursorRatioX > ratioLimit && temp == 0)
        {
            if (scenario != 6)
            {
                scenario = 6;
                pointerToSideTime = 0;
            }
            else
            {
                pointerDist = Mathf.Lerp(pointerDist, maxRight, 2 * Time.deltaTime);
                //if (pointerToSideTime <= lookSmoothX)
                //  pointerDist = Mathf.SmoothDamp(pointerDist, 15, ref smoothLookVelocityX, lookSmoothX);
            }
        }
    }
}
