using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    //7-17R added to prevent screen panning bug
    //a better fix will be needed if the screen shaking bug reappears
    public GameObject pause;
    public PlayerController thePlayer;
    public Vector2 focusAreaSize;
    public float zValue;

    public float horiOffSet;
    public float vertOffSet;
    public float lookAheadDstX;
    public float lookSmoothX;
    public float vertSmoothTime;
    public bool lookAheadStopped;
    FocusArea focusArea;

    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothVelocityX;
    float smoothVelocityY;
    //new variables, compared to og cam


    public float pointerActivateTime;
    public float temp;
    public float comboDist;
    public float pointerDist;
    // Use this for initialization
    void Start()
    {
        if (thePlayer == null)
            thePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        focusArea = new FocusArea(thePlayer.GetComponent<Collider2D>().bounds, focusAreaSize);
    }
    //only used for seeing which side of the screen the mouse is
    void Update()
    {

    }
    void LateUpdate()
    {
        focusArea.update(thePlayer.GetComponent<Collider2D>().bounds);

        //10-17R changed to take in the screen shake into account
        Vector2 focusPosition = focusArea.centre + GetComponent<CameraShake>().shakePos + Vector2.right * horiOffSet + Vector2.up * vertOffSet;
        //is the player moving, if yes has value


        //this is the camera swing for when touching a different side of the focus area
        /*
        if (focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            */
            /*
            if (thePlayer.moveSpeed != 0 && Mathf.Sign(thePlayer.moveSpeed) == Mathf.Sign(focusArea.velocity.x))
            {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * lookAheadDstX;
            }
            else
            */
            /*
            {
                if (!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX
                        - lookAheadDirX) / 4f;
                }
            }
        }
        */
     
    
        targetLookAheadX = lookAheadDirX * lookAheadDstX;
        //Pointer on right
        //7-17R part 2 of screen panning bug fix
        if (!pause.GetComponent<PauseReset>().paused)
        {
            //standard look ahead or behind
            currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX,
                 ref smoothLookVelocityX, lookSmoothX);

            focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y,
                ref smoothVelocityY, vertSmoothTime);
            
            comboDist = currentLookAheadX + pointerDist;
            focusPosition += Vector2.right * comboDist;
            transform.position = (Vector3)focusPosition + Vector3.forward * zValue;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(focusArea.centre, focusAreaSize);
    }
    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void update(Bounds targetBounds)
        {
            float shiftX = 0;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
