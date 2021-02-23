//Different player scripts for rendering?

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //8-17R2 testing new parenting garbage, experimental build

    //for checking if on ground
    public Transform groundpoint;
    public Transform groundpointTwo;
    public GameObject hittingWall;
    //for checking if on ground
    public LayerMask layerIsRunnable;
    public LayerMask layerIsPlat;
    //rename
    public float airControl;
    public float airControlRev;

    public bool onRunnableDebug;

    /*
    //needs constant attention or moderately broken
    public bool droppable;
    */

    //all pretty stable dont need to view constantly
    //[HideInInspector]
    public bool control;
    //[HideInInspector]
    public float playerLocationY;
    //[HideInInspector]
    public float playerMaxY;
    [HideInInspector]
    public float deceleration = 70;
    [HideInInspector]
    public float acceleration = 70;
    [HideInInspector]
    public float accelReset;
    [HideInInspector]
    public float timeSlowed = 1f;
    [HideInInspector]
    public float fallStunDis = 8;
    //[HideInInspector]
    public float decelFromLimit = 20;
    [HideInInspector]
    public bool slowed;
    //[HideInInspector]

    public GameObject placerSprite;
    public bool showPlacer;

    public GameObject playerRigObject;
    [HideInInspector]
    public SpriteRenderer playerRenderer;
    public Animator playerAnimator;

    //public GameObject[] playerRigObjects;
    //public Animator[] playerAnimators;

    //1-15-18W : These were the old player animation objs. 
    //They're now here as TEMP so I can fill anim gaps to the new system.
    public GameObject TEMP_playerSpriteObject;
    public SpriteRenderer TEMP_playerRenderer;

    [HideInInspector]
    public Rigidbody2D playerRigidBody;

    private bool intFAir;
    private bool intBAir;
    private bool airChecked;
    [SerializeField]
    private float airTimer = 0.1f;
    private float airtimerReset;
    [SerializeField]
    private float jumpTimeCounter;
    private float cCollected;
    private float stopSpeed = 2;
    private bool touchingPlatBot;
    private float groundRad = 0.05f;
    public bool stunned;
    private float stunTime = .5f;
    //[HideInInspector]
    public float jumpForce = 17;
    //[HideInInspector]
    public float addJumpForce = 23;
    [HideInInspector]
    public float jumpTime = .2f;
    private bool jumping;
    //private bool jumped;
    public Sprite defaultSprite;
    private Collider2D myCollider;
    public GameObject arm;
    //8-17R2 the new parenting shit
    public float mSMaxExtend;
    public float mSMinExtended;
    public float platSpeedX = 0;
    public float platSpeedY = 0;
    //Allows non-zero default speed while on moving plat
    public float currentMoveSpeedZero;
    public float moveSpeed;
    [HideInInspector]
    public float moveSpeedMax = 17;
    [HideInInspector]
    public float moveSpeedMin = -17;
    public GameObject currentPlat;
    private Rigidbody2D currentPlatRB;
    public float fallingVel;
    public float percentPlatX;
    public float platDecel;

    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

        currentMoveSpeedZero = 0;

        //1-16-18W : Toggle sprite for animation placing
        if (!showPlacer)
            if (placerSprite != null)
                placerSprite.SetActive(false);

        //1-15-18W : Replacing 2nd ver animation system with 3rd ver

        myCollider = GetComponent<Collider2D>();
        control = true;
        jumpTimeCounter = jumpTime;
        accelReset = acceleration;
        airtimerReset = airTimer;
    }

    /*
    if overlapcircl-ing trigger and pressing S
    disable player contact with trigger's plat

    Do not reenable until ontriggerexit

    rely on plat effector otherwise
    */

    void FixedUpdate()
    {
        //Debug.Log(moveSpeed);
        jumpControls();
        parentingNPlatforms();
        modifiedPlayerMovement();
        movementBasics();
        //08-17R2 rework
        if (control && onRunnable() || control && currentPlatRB == null && !onRunnable())
        {
            playerRigidBody.velocity = new Vector2(moveSpeed + platSpeedX, playerRigidBody.velocity.y);
        }
        else if (control && currentPlatRB != null && !onRunnable())
        {
            playerRigidBody.velocity = new Vector2(moveSpeed + platSpeedX * percentPlatX, playerRigidBody.velocity.y);
        }
        //8-17R2 testin some shit
        //7-17R created for when the player gets stunned on a moving platform
        else if (!control && !GetComponent<PlayerSpecialMoves>().dashing && !GetComponent<PlayerSpecialMoves>().slamming)
        {
            playerRigidBody.velocity = new Vector2(platSpeedX, playerRigidBody.velocity.y);
        }
    }
    public void movementBasics()
    {
        //Debug.Log(playerRigidBody.velocity.y);
        //I should probably switch this to switch statements, code would look less terrifying
        if (control)
        {
            //Move forwards
            if ((Input.GetKey(KeyCode.D)) && (moveSpeed < moveSpeedMax) && onRunnable())
            {
                if (moveSpeed < currentMoveSpeedZero)
                {
                    moveSpeed = currentMoveSpeedZero + 1;
                }
                moveSpeed = moveSpeed + acceleration * Time.deltaTime;
                //transform.rotation = Quaternion.identity;
            }
            //Move backwards
            else if (Input.GetKey(KeyCode.A) && (moveSpeed > moveSpeedMin) && onRunnable())
            {
                if (moveSpeed > currentMoveSpeedZero)
                {
                    moveSpeed = currentMoveSpeedZero - 1;
                }
                moveSpeed = moveSpeed - acceleration * Time.deltaTime;
                //transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            //Move forwards air
            else if ((Input.GetKey(KeyCode.D)) && (moveSpeed < moveSpeedMax) && !onRunnable())
            {
                if (intFAir)
                {
                    moveSpeed = moveSpeed + acceleration * airControl * Time.deltaTime;
                }
                if (intBAir)
                {
                    moveSpeed = moveSpeed + acceleration * airControlRev * Time.deltaTime;
                }
                if (intBAir == false && intFAir == false)
                {
                    moveSpeed = moveSpeed + acceleration * airControl * Time.deltaTime;
                }
            }
            //Move backwards air
            else if (Input.GetKey(KeyCode.A) && (moveSpeed > moveSpeedMin) && !onRunnable())
            {
                if (intFAir)
                {
                    moveSpeed = moveSpeed - acceleration * airControl * Time.deltaTime;
                }
                if (intBAir)
                {
                    moveSpeed = moveSpeed - acceleration * airControlRev * Time.deltaTime;
                }
                if (intBAir == false && intFAir == false)
                {
                    moveSpeed = moveSpeed - acceleration * airControl * Time.deltaTime;
                }
            }
            else
            {
                //keep the if blank so nothing happens if a movement key is held
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                {

                }
                else
                {
                    if (moveSpeed > currentMoveSpeedZero && onRunnable())
                    {
                        if (moveSpeed < stopSpeed)
                        {
                            moveSpeed = currentMoveSpeedZero;
                        }
                        else
                        {
                            moveSpeed = moveSpeed - deceleration * Time.deltaTime;
                        }
                    }

                    else if (moveSpeed < currentMoveSpeedZero && onRunnable())
                    {
                        if (moveSpeed > stopSpeed)
                        {
                            moveSpeed = currentMoveSpeedZero;
                        }
                        else
                        {
                            moveSpeed = moveSpeed + deceleration * Time.deltaTime;
                        }
                    }
                }
            }
        }
    }
    public void modifiedPlayerMovement()
    {
        if (control)
        {
            playerRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        playerLocationY = transform.position.y;
        if (!onRunnable() && playerLocationY > playerMaxY)
        {
            playerMaxY = playerLocationY;
        }
        //10-17R works now

        else if (!onRunnable() && !jumping && playerRigidBody.velocity.y < fallingVel + platSpeedY)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, fallingVel + platSpeedY);
        }
        else if (onRunnable())
        {
            if (playerMaxY != 0 && playerMaxY - playerLocationY > fallStunDis)
            {
                StartCoroutine("slow");
            }
            playerMaxY = 0;
        }

        if (hittingWall && !touchingPlatBot)
        {
            //player to left of wall
            if (playerRigidBody.transform.position.x < hittingWall.transform.position.x && Input.GetKey(KeyCode.D))
            {
                moveSpeed = 0;
                currentMoveSpeedZero = 0;
            }
            else if (playerRigidBody.transform.position.x > hittingWall.transform.position.x && Input.GetKey(KeyCode.A))
            {
                moveSpeed = 0;
                currentMoveSpeedZero = 0;
            }
            if (Mathf.Abs(playerRigidBody.transform.position.x - hittingWall.transform.position.x) > .2)
            {
                hittingWall = null;
            }
        }
        //if(moveSpeed>moveSpeedMax && !dashing)
        if (moveSpeed > moveSpeedMax && control)
        {
            moveSpeed = moveSpeed - decelFromLimit * Time.deltaTime;
        }
    }
    public void parentingNPlatforms()
    {
        //Set Currentplat at OverlapCircle instead of OnTriggerStay
        //Collider to get at OverlapCircle
        Collider2D col = null;
        //Check groundpointTWO first, as a player would more likely depend on being on left groundpoint
        if (Physics2D.OverlapCircle(groundpointTwo.position, groundRad, layerIsRunnable))
            col = Physics2D.OverlapCircle(groundpointTwo.position, groundRad, layerIsRunnable);

        if (Physics2D.OverlapCircle(groundpoint.position, groundRad, layerIsRunnable))
            col = Physics2D.OverlapCircle(groundpoint.position, groundRad, layerIsRunnable);

        if (col != null)
        {
            currentPlat = col.gameObject;

            //If falling
            if (playerRigidBody.velocity.y <= 0f)
            {
                if (col.GetComponent<Rigidbody2D>() != null)
                {
                    currentPlatRB = col.GetComponent<Rigidbody2D>();
                }

                //TEST: Reset Plat Speeds on plats without rigidbodies (ones that don't move)
                else
                {
                    platSpeedX = 0;
                    platSpeedY = 0;

                    currentPlatRB = null;
                }
            }

        }
        //8-17R2 testing the new shit
        if (Physics2D.OverlapCircle(groundpoint.position, groundRad, layerIsPlat) && currentPlatRB != null
            || Physics2D.OverlapCircle(groundpointTwo.position, groundRad, layerIsPlat) && currentPlatRB != null)
        {
            if (currentPlatRB != null)
            {
                platSpeedX = currentPlatRB.velocity.x;
                platSpeedY = currentPlatRB.velocity.y;
            }

            else
            {
                platSpeedX = 0;
                platSpeedY = 0;
            }
            //Debug.Log(currentPlat.GetComponent<MeshRenderer>().bounds.min.x);
            //Debug.Log(currentPlat.GetComponent<MeshRenderer>().bounds.max.x);

            //TEMP DISABLED: THIS WAS STOPPING THE PLAYER SHORT WHEN JUMPING UP THRU A PLAY. 
            // DISABLED TO MAKE SURE YOU CAN JUMP THRU PLATS SMOOTHLY. 
            //WILL NEED TO REMAKE THIS FUNCTIONALITY FOR PLATS THAT HAVE COMPLICATED PATHS
            /*
                //8-17R2 stops the player from bouncing off of plat when dir changes
                if (!jumping && !GetComponent<PlayerSpecialMoves>().dashing)
                {
                   playerRigidBody.velocity = new Vector2(platSpeedX, platSpeedY);
                }
            */
        }
        else if (currentPlat != null && !onRunnable() &&
            (playerRigidBody.transform.position.x < currentPlat.GetComponent<Collider2D>().bounds.min.x
            || playerRigidBody.transform.position.x > currentPlat.GetComponent<Collider2D>().bounds.max.x))
        {
            //8-17R2 currently runs many times
            //Debug.Log("we out of it");

            platSpeedX = platSpeedX * platDecel;
            platSpeedY = platSpeedY * platDecel;
            //10-17R dash now stays at a steady height after going off a plat
            if (GetComponent<PlayerSpecialMoves>().dashing && !GetComponent<PlayerSpecialMoves>().slamBouncing)
            {
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
            }
        }

        //8-17R2 stripped
        if (onRunnable())
        {
            if (playerRigObject.activeSelf == true)
            {
                playerAnimator.SetFloat("MoveSpeed", playerRigidBody.velocity.x);

                playerAnimator.SetBool("Jumping", false);
            }
        }
    }
    public void jumpControls()
    {

        //updated 7-17R
        if (Input.GetKeyDown(KeyCode.Space) && control)
        {
            //sets up standard jumps and boosted jumps
            if (onRunnable())
            {
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce + platSpeedY);
                jumping = true;
            }
        }

        if (!onRunnable())
        {
            airTimer -= Time.deltaTime;
        }
        if (onRunnable())
        {
            if(!jumping)
                airTimer = airtimerReset;
            //resets ability to dash, in Player Special Moves
            GetComponent<PlayerSpecialMoves>().dashed = false;
            //7-17R
            if (!GetComponent<PlayerSpecialMoves>().slamming)
            {
                GetComponent<PlayerSpecialMoves>().slammed = false;
            }
        }
        //allows for the variable jump height
        //7-17R conrtol check added, prevents rising dash
        if (Input.GetKey(KeyCode.Space) && jumping && control)
        {
            if (jumpTimeCounter > 0)
            {
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, addJumpForce + platSpeedY);
                jumpTimeCounter -= Time.deltaTime;
            }
            //prevents holding space for multiple jumps
            else
            {
                jumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTimeCounter = 0;
            jumping = false;
        }
        //resets variables via touching ground
        if (onRunnable() && !Input.GetKey(KeyCode.Space))
        {
            jumpTimeCounter = jumpTime;
            intBAir = false;
            intFAir = false;
            airChecked = false;
                 
        }
        //locks what the initial air momentumn was
        if (!onRunnable() && airChecked == false)
        {
            if (moveSpeed > currentMoveSpeedZero && airChecked == false)
            {
                intFAir = true;
                airChecked = true;
            }
            if (moveSpeed < currentMoveSpeedZero && airChecked == false)
            {
                intBAir = true;
                airChecked = true;
            }
        }

        if (!onRunnable())
        {
            //playerRigObject.SetActive(false);
            playerAnimator.SetBool("Jumping", true);
        }
    }
    public bool onRunnable()
    {
        if (Physics2D.OverlapCircle(groundpoint.position, groundRad, layerIsRunnable)
         || Physics2D.OverlapCircle(groundpointTwo.position, groundRad, layerIsRunnable)
           )
        {
            onRunnableDebug = true;
            return true;
        }
        else
        {
            onRunnableDebug = false;
            return false;
        }
    }

    void OnTriggerStay2D (Collider2D plat)
    {
        if (plat.tag == "Wall")
        {
            hittingWall = plat.gameObject;
        }
     
        //Assumes plat is a DropTrigger the player is in, plat is the child DropTrigger of a platform with a non-trigger collider, and the player is pressing "S" to try phasing thru the plat
        //CURRENTLY DISABLES THE PLATFORM IN QUESTION, WILL NEED TO MOVE THE PLAT TO A SEPERATE LAYERS SO THAT ENEMIES AND PROPS ARE UNEFFECTED
        if (plat.tag == "DropTrigger")
        {
            if (Input.GetKey(KeyCode.S))
            {
                plat.transform.parent.GetComponent<Collider2D>().enabled = false;
            }
        }
    }
    //get collectable(basic) walls and plat parenting
    void OnTriggerEnter2D(Collider2D plat)
    {
        //8-17R
        if (plat.tag == "Void")
        {
            GetComponent<BlockHealth>().InstaKill();
        }
        if (plat.tag == "Wall")
        {
            hittingWall = plat.gameObject;
            moveSpeed = 0;
        }
        //new, still needs more testing, in its current state you just drop off and then go backwards a bit
        /*
        if (plat.tag == "platEdge")
        {
            transform.SetParent(null);
            Physics2D.IgnoreLayerCollision(12, 10, true);
        }
        */
        if (plat.tag == "Collectable")
        {
            cCollected++;
            Destroy(plat.gameObject);
        }

        //Need to move to new scripts!

        if (plat.tag == "ETank")
        {
            Debug.LogError("Need to move collectibles to new scripts!");
            GetComponent<PlayerSpecialMoves>().addMana();
            Destroy(plat.gameObject);
        }
        if (plat.tag == "HpPack")
        {
            Debug.LogError("Need to move collectibles to new scripts!");
            if (GetComponent<Health>().health < GetComponent<Health>().maxHealth)
            {
                GetComponent<Health>().health += 20;
                if (GetComponent<Health>().health > GetComponent<Health>().maxHealth)
                {
                    GetComponent<Health>().health = GetComponent<Health>().maxHealth;
                }
            }
            Destroy(plat.gameObject);
        }
        if (plat.tag == "underSPlat")
        {
            touchingPlatBot = true;
        }

    }
    //resets wall trigger and thin platform parenting
    void OnTriggerExit2D(Collider2D plat)
    {

        //UNDER THIS CURRENT SYSTEM:
        //THIN PLATS (ONES THAT CAN BE DROPPED THRU) *MUST* HAVE A CHILD TAGGED DROPTRIGGER!

        //Reset the trigger's corresponding plat
        if (plat.tag == "DropTrigger")
        {
            plat.transform.parent.GetComponent<Collider2D>().enabled = true;
        }
        
        /*
        if (plat.tag == "platEdge")
        {
            // Physics2D.IgnoreLayerCollision(9, 10, false);
            StartCoroutine("platReset");
        }
        */
        
        if (plat.tag == "Wall")
        {
            hittingWall = null;
            Debug.Log("Exited Wall");

            moveSpeed = 0;
            currentMoveSpeedZero = 0;
        }
        if (plat.tag == "underSPlat")
        {
            touchingPlatBot = false;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        /*
        {
            //Debug.Log("non plat seen");
            platSpeedX = 0;
            platSpeedY = 0;
            currentPlat = null;
        }
        */
    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (onRunnable())
        {
            //Debug.Log(col.gameObject.transform.rotation.y);
        }
  
        //8-17R testing to fix the bug where the player can get stuck on the ball midair
        if (col.gameObject.tag == "Heavy_Object")
        {
            if (!onRunnable())
            {
                //Debug.Log("triggering air 0");
                moveSpeed = 0;
            }
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        //7-17R
        if (col.gameObject.tag == "Platform" && gameObject.GetComponent<PlayerSpecialMoves>().dashing
            || col.gameObject.tag == "thinPlatform" && gameObject.GetComponent<PlayerSpecialMoves>().dashing)
        {
            //8-17R2 you know the drill
            currentPlat = null;
            currentPlatRB = null;
            platSpeedX = 0;
            platSpeedY = 0;
        }

    }
    IEnumerator stun()
    {
        control = false;
        stunned = true;
        playerAnimator.SetBool("Stunned", true);
        TEMP_playerRenderer.color = new Color(.5f, .5f, .5f, 1);
        playerRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(stunTime);
        control = true;
        playerAnimator.SetBool("Stunned", false);
        TEMP_playerRenderer.color = new Color(1, 1, 1, 1);
        stunned = false;
    }
    IEnumerator slow()
    {
        slowed = true;
        moveSpeed = currentMoveSpeedZero;
        acceleration = acceleration / 2;
        //GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1);
        playerRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(timeSlowed);
        //GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        acceleration = accelReset;
        slowed = false;
    }
}


