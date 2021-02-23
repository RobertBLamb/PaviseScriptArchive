using UnityEngine;
using System.Collections;

public class PlayerSpecialMoves : MonoBehaviour
{
    public GameObject playerMain;
    Rigidbody2D playerRigidBody;
    public GameObject forceBall;
    public Transform projectileSpawn;
    public Transform dashForceSpawn;
    public GameObject playerRig;
    public GameObject playerTorso;
    //SpriteRenderer playerRenderer;
    public Animator playerAnimator;

    [SerializeField]
    private GameObject playerArmRig;

    public GameObject arm;

    [Header("For toggling the ability to use any of the 3 Beta special moves")]
    public bool canSuperDeflect;
    public bool canShieldDash;
    public bool canGroundSlam;

    //use to tag what ShieldDash is being used for
    public bool dashing;
    public bool enhanceDash;
    public bool charging;
    public bool slamming;
    public bool slamBouncing;

    public bool superDeflecting;
    public bool superDeflected;
    public float superDeflectCharge;

    public float dashSpeed;
    public float dashTime;
    public float slamSpeed;
    public float groundSlamTime;
    public float bounceSpeed;
    public float groundBounceTime;
    //7-17R
    public float boostSpeed;

    public float enhancedDashTime;
    public float enhancedDashSpeed;

    public GameObject dashBox;
    public GameObject superDeflectBox;

    public Sprite defaultSprite;
    public Sprite dashSprite;
    public Sprite slamSprite;
    public Sprite enhancedDashSprite;
    public Sprite superDeflect1;
    public Sprite superDeflect2;
    //taken from 2-13 version

    //used in playercontroller to allow for dashing to reset
    [HideInInspector]
    public bool dashed;
    //7-17R
    //[HideInInspector]
    public bool slammed;
    public bool onBoosting;
    [HideInInspector]
    public bool dashForcing;

    //Make text object take from tis script instead

    //Allow specials to be used without mana costs, for debugging
    [SerializeField]
    private bool ignoreManaCosts = false;

    public float mana;
    public float maxMana = 100;
    private float minMana = 0;

    [Header("Set how much mana each ability costs")]
    public int superDeflectBaseCost;
    public int shieldDashCost;
    public int groundSlamCost;

    public Coroutine regenDelay;
    public Coroutine regen;

    [Header("Special ability params")]
    public float CountDownMax;
    //lets smallcrushplat know if a special is happening
    public bool genericSpecial;
    //Code unique to groundSlam
    //public HingeJoint2D hinge;
    public GameObject playerArm;
    public GameObject shield;
    public Rigidbody2D playerRB;
    public float armAngle;
    public float superDeflectAngle;
    public float armLock;
    public float minSlamAngle;
    public float maxSlamAngle;
    public float slamMax;
    public float slamMin;
    public float force;
    private bool shieldColliding;
    public float defaultGravScale;

    // Use this for initialization
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        if (playerArm == null)
            playerArm = GameObject.FindGameObjectWithTag("PlayerArm");
        //Still stands in for a proper arm rig. Make arm a new anim layer?
        if (playerArmRig == null)
            playerArmRig = GameObject.FindGameObjectWithTag("PlayerArmRig");
        //hinge = playerArm.GetComponent<HingeJoint2D>();
        playerRigidBody = playerMain.GetComponent<Rigidbody2D>();
        if (playerAnimator == null)
            playerAnimator = playerRig.GetComponent<Animator>();
        if (playerTorso == null)
        {
            playerTorso = GameObject.FindGameObjectWithTag("PlayerTorso");
        }

        mana = maxMana;
        //shieldColliding = GameObject.Find("BlockBox").GetComponent<GroundSlamCheck>().shieldTouch;
        defaultGravScale = playerRigidBody.gravityScale;

    }
    void Update()
    {
        if (playerMain.GetComponent<PlayerController>().control)
        {
            /*
            if (mana < maxMana)
            {
                mana += Time.deltaTime * 15;
            }
            if (mana < minMana)
            {
                mana = minMana;
            }
            */
        }

        //Note: check this out, along with spriteReset, and see if this messes anything up
        if (!genericSpecial)
        {
            superDeflecting = false;
            spriteReset();
        }
        //genericSpecial = true;
    }
    void FixedUpdate()
    {
        //ShieldDash
        //7-17R add way to prevent dash slams?
        if (canShieldDash)
        {
            if (!charging && Input.GetKeyDown(KeyCode.LeftShift) && playerMain.GetComponent<PlayerController>().control
                && !dashed && ManaCostCheck(shieldDashCost))
            {
                SpendMana(shieldDashCost);
                //7-17R testing
                StopCoroutine(shieldDash(0, 0, 0));
                //StopAllCoroutines();
                dashed = true;
                dashing = true;
                StartCoroutine(shieldDash(dashSpeed, 0, dashTime));
            }

        }
        if (canSuperDeflect)
        {

            //SuperDeflect
            if (!charging && !superDeflecting && (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1)) && playerMain.GetComponent<PlayerController>().control
            && ManaCostCheck(superDeflectBaseCost))
            {
                SpendMana(superDeflectBaseCost);
                chargeSuperDeflect();
                //arm.gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonUp(1))
            {
                if (charging)
                    StartCoroutine(superDeflect());
                charging = false;

            }
        }

        if (canGroundSlam)
        {
            //7-17R modified
            if (Input.GetKeyDown(KeyCode.LeftControl) && !slammed && playerMain.GetComponent<PlayerController>().control
            && ManaCostCheck(groundSlamCost))
            {
                SpendMana(groundSlamCost);
                StopAllCoroutines();
                slamming = true;
                slammed = true;
                StartCoroutine(shieldSlam());
            }
        }

        //Conflict checks (Is already using a special?)
        if (charging || superDeflecting)
        {
            genericSpecial = true;
            //playerAnimator.enabled = false;
            //playerArmSpriteContainer.SetActive(false);
            //playerArm.GetComponent<SpriteRenderer>().enabled = false;


            //arm.GetComponent<BoxCollider2D>().enabled = false;
            // arm.gameObject.SetActive(false);
            //playerMain.GetComponent<Health>().damageABLE = false;
        }
        if (dashing
            //&& !enhanceDash
            )
        {
            //| RigidbodyConstraints2D.FreezeRotationZ;
            //playerRigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            playerMain.GetComponent<PlayerController>().control = false;
            playerMain.GetComponent<PlayerController>().moveSpeed = dashSpeed + playerMain.GetComponent<PlayerController>().platSpeedX;
            playerMain.GetComponent<Health>().damageABLE = false;
            CountDownMax += Time.deltaTime;
            //arm.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        }
        if (!dashing)
        {
            CountDownMax = dashTime + 1f;
        }
        //7-17R, temp for slam testing
        if (GetComponent<PlayerController>().onRunnable() && slamming && !slamBouncing)
        {
            //StartCoroutine(shieldSlamBounce());
        }
       
    }

    //Check if player has the required mana to use the special
    bool ManaCostCheck(int manaCost)
    {
        if (ignoreManaCosts)
            return true;
        if (mana >= manaCost)
            return true;
        else
            return false;
    }

    void SpendMana(float manaCost)
    {
        mana -= manaCost;

        //Reset regen
        if (regenDelay != null)
        {
            StopCoroutine(regenDelay);
        }
        if (regen != null)
        {
            StopCoroutine(regen);
        }

        regenDelay = StartCoroutine(DelayRegen());
    }

    IEnumerator DelayRegen()
    {
        float stableMana = mana;
        yield return new WaitForSeconds(1.5f);
        //If no mana was spent during this time
        //if (mana == stableMana)
        if (Mathf.Abs(stableMana - mana) < 1f)
            regen = StartCoroutine(Regen());
    }

    //Regen the health block after a certain time without damage
    //Need to cap max health and cancel if damaged
    IEnumerator Regen()
    {
        while (mana < maxMana)
        {
            //Need to base wait time on either the system time or a calculated interval
            yield return new WaitForSeconds(.03f);
            //Might also need to tweak this to make the regen time reasonable
            mana+= .5f;
        }

        if (mana >= maxMana)
            mana = maxMana;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("BoostPad"))
        {
            onBoosting = true;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        //7-17R no longer needed, onrunnable check is more consistent

        if (dashing)
        {
            //playerMain.GetComponent<PlayerController>().wall = true;
            if (col.gameObject.CompareTag("Wall"))
            {
                //7-17R testing
                StopCoroutine(shieldDash(0,0,0));
                GetComponent<PlayerController>().StartCoroutine("stun");
                dashing = false;
                playerArm.gameObject.SetActive(true);
                playerAnimator.enabled = true;
                GetComponent<Health>().damageABLE = true;
                genericSpecial = false;
            }
            //playerRigidBody.velocity = new Vector2(0,0);
        }
        //7-17R testing
        if (col.gameObject.CompareTag("Ground")|| col.gameObject.CompareTag("Platform")|| col.gameObject.CompareTag("thinPlatform"))
        {
          if (slamming && !slamBouncing)
            {
                StartCoroutine(shieldSlamBounce());
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("BoostPad"))
        {
            //7-17R
            //onBoosting = false;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //7-17R not sure if new, check when integrating
        if (col.tag == "Wall")
        {
            //8-17R updated to work with the new breakplat types
            if ((dashing && col.transform.parent.tag != "breakPlat") ||
                    (dashing && !col.gameObject.GetComponentInParent<specialColCheck>().dashCheck))
            {
                if (col.transform.parent.tag == "breakPlat")
                    StopCoroutine("shieldDash");
                GetComponent<PlayerController>().StartCoroutine("stun");
                dashing = false;
                enhanceDash = false;
                arm.gameObject.SetActive(true);
                playerAnimator.enabled = true;
                GetComponent<Health>().damageABLE = true;
                genericSpecial = false;
            }
            //playerRigidBody.velocity = new Vector2(0,0);
        }
        if (col.tag=="BoostPad")
        {
            //7-17R
            onBoosting = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //7-17R not sure if new, check when integrating
        if (col.tag == "BoostPad")
        {
            //7-17R
            onBoosting = false;
        }
    }

    public void addMana()
    {
        mana += 20;
        if (mana >= maxMana)
        {
            mana = maxMana;
        }
    }

    void Shoot()
    {
        if (dashing)
        {
            Instantiate(forceBall, dashForceSpawn.position, transform.rotation);
        }

        else if (superDeflecting)
        {
            Instantiate(forceBall, projectileSpawn.position, Quaternion.Euler(0, 0, superDeflectAngle));
        }

    else
        {
            Instantiate(forceBall, projectileSpawn.position, transform.rotation);
        }
    }

    IEnumerator shieldSlam()
    {
        slamming = true;
        armAngle = playerArm.transform.rotation.eulerAngles.z;
        //designed with the idea that you can only use slam with the shield below you
        //converts eulerAngles.z to value shown in inspector
        //armAngle -= 360;
        yield return new WaitForSeconds(0);
        if (armAngle > minSlamAngle && armAngle < maxSlamAngle)
        {
            //7-17 its running bounce before slam
            //7-17R doesnt seem to have an affect on the slam speed bug
            //StopAllCoroutines();
            StartCoroutine(shieldDash(slamSpeed, armAngle, groundSlamTime));
        }
    }

    //7-17R calculations different
    public IEnumerator shieldSlamBounce()
    {
        var bounceAngle = armAngle;
        dashing = false;
        slamBouncing = true;
        //7-17R resolves the height issue however if on ground when you bounce it acts as a dash
        //and even triggering it in air is inconsistent
        StopAllCoroutines();
        //Debug.Log("bounce coroutine thinks youre boosting" + onBoosting);

        StartCoroutine(shieldDash(bounceSpeed, bounceAngle, groundBounceTime));
        // Dummy until bounceback is set
        yield return new WaitForSeconds(0);
    }

    //7-17R removed numbers testing
    public IEnumerator shieldDash(float thisDashSpeed, float angleMod, float thisDashTime)
    {
        playerRigidBody.gravityScale = 0;
        GetComponent<PlayerController>().playerMaxY = GetComponent<PlayerController>().playerLocationY;
        if (!slamming && !slamBouncing)
        {
            dashing = true;
        }
        genericSpecial = true;
        playerMain.GetComponent<Health>().damageABLE = false;
        //7-17R removed to prevent forceball bug
        //if (!dashForcing)
        //{

            StartCoroutine("shieldDashForce");
        //}
        playerMain.GetComponent<PlayerController>().control = false;
        if (dashing)
        {
            playerAnimator.SetBool("Dashing", true);
        }
        else
        {
            //TEMP: NEEDS A SEPERATE ANIM AND BOOL FOR SLAMS
            playerAnimator.SetBool("Dashing", true);
        }

        //setup end
        //7-17R testing
        //Debug.Log("onrimable" + GetComponent<PlayerController>().onRunnable());

        if (!slamBouncing)
        {
            transform.eulerAngles = new Vector3(0, 0, angleMod);
        }
        else if (slamBouncing)
        {
            //slam speed is being used instead
            transform.eulerAngles = new Vector3(0, 0, Mathf.Abs(360-angleMod));
        }

        if(!onBoosting)
        {
            //Debug.Log("notboosting");

            playerRigidBody.velocity = transform.rotation * new Vector2(thisDashSpeed, 0);
            //7-17R when commented no apparent change
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x + GetComponent<PlayerController>().platSpeedX, playerRigidBody.velocity.y);
        }
        else if(onBoosting)
        {
            //Debug.Log("boosting");
            playerRigidBody.velocity = transform.rotation * new Vector2(thisDashSpeed + boostSpeed, 0);
            //7-17R when commented no apparent change
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x + GetComponent<PlayerController>().platSpeedX, playerRigidBody.velocity.y);
        }

        Debug.Log(playerRigidBody.velocity.x);
        //flipChar dependent
        /*if (GetComponent<flipChar>() != null)
        {
            GetComponent<flipChar>().facingRight = true;
        }*/
        Instantiate(dashBox, transform.position, transform.rotation);

        //playerArmRig.SetActive(false);
        //playerArm.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(thisDashTime);

        //reseting to standard player settings

        GetComponent<Health>().damageABLE = true;

        spriteReset();

        playerRigidBody.gravityScale = defaultGravScale;
        transform.eulerAngles = new Vector3(0, 0, 0);
        if (dashing)
        {
            playerMain.GetComponent<PlayerController>().moveSpeed = playerMain.GetComponent<PlayerController>().moveSpeedMax;
        }
        //7-17R stops the bug where the player goes way higher if bouncing straight up
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
              playerMain.GetComponent<PlayerController>().control = true;

        genericSpecial = false;
        GetComponent<Health>().damageABLE = true;
        dashing = false;
        slamming = false;
        //7-17R
        if(slamBouncing)
        {
            slammed = false;
            slamBouncing = false;
        }
    }

    //7-17R edited to prevent force balls from not spawning
    public IEnumerator shieldDashForce()
    {
        //dashForcing = true;
        CountDownMax = 0;
        while (CountDownMax <= dashTime)
        {
            //Instantiate(superDeflectBox, transform.position, Quaternion.Euler(0, 0, -30));
            Shoot();
            yield return new WaitForSeconds(.1f);
        }
        CountDownMax = 0;
        //dashForcing = false;
    }

    void chargeSuperDeflect()
    {
        //08-30-17 added superDeflectAngle and optional angle variable for Shoot()
        float tempAngle = playerArm.transform.localRotation.eulerAngles.z;
        charging = true;
        //TEMP as all hell, need to play with the numbers
        playerAnimator.SetLayerWeight(1, 1);
        playerAnimator.SetFloat("SuperDeflect", superDeflectCharge);
        if (superDeflectCharge < 100)
            superDeflectCharge++;

        //playerRenderer.sprite = superDeflect1;

        //if (playerArm.transform.localRotation.eulerAngles.z <= 30 || playerArm.transform.localRotation.eulerAngles.z >= 315)
            tempAngle = playerArm.transform.localRotation.eulerAngles.z;
        /*
        else if (playerArm.transform.localRotation.eulerAngles.z > 30 && playerArm.transform.localRotation.eulerAngles.z < 180)
        {
            tempAngle = 30;
        }
        else
        {
            tempAngle = 315;
            //arm.gameObject.SetActive(false);
        }
        */
        superDeflectAngle = tempAngle;

    playerTorso.transform.localRotation = Quaternion.Euler(0, 0, superDeflectAngle);
    }



    IEnumerator superDeflect()
    {
        //Deduct more for charge time, needs to be balanced and fixed
        //mana -= (superDeflectCharge/2);
        genericSpecial = true;
        superDeflecting = true;
        //playerRenderer.sprite = superDeflect2;
        superDeflectCharge = 0;

        Instantiate(superDeflectBox, transform.position, Quaternion.Euler(0, 0, superDeflectAngle));
        Shoot();
        playerAnimator.SetFloat("SuperDeflect", 0);
        playerAnimator.SetLayerWeight(1, 0);

        yield return new WaitForSeconds(.6F);
        //08-15-17W Make resetting sprite a method
        spriteReset();
        
        GetComponent<Health>().damageABLE = true;
        superDeflecting = false;
        genericSpecial = false;
    }

    //08-15-17W Make resetting sprite a method
    void spriteReset()
    {
        //playerRenderer.sprite = defaultSprite;
        playerAnimator.enabled = true;
        //playerArmRig.SetActive(true);
        playerAnimator.SetBool("Dashing", false);

        //playerArm.GetComponent<SpriteRenderer>().enabled = true;
        playerRig.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}