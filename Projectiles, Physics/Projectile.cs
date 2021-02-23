using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public GameObject shooterArm;
    public GameObject shield;
    public GameObject explosion;
    public GameObject superExplosion;
    public GameObject superExplAOE;
    public GameObject trailObject;
    public GameObject shieldSticker;

    //private PlayerController playerScript;

    private Rigidbody2D myRigidBody;
    protected Collider2D hitCollider;


    public LayerMask surface = 1 << 8;

    public float destroyTime = 7;

    public float customReflectAngle;

    //Bool should be true if projectile is affected by gravity
    public bool isPhysical;
    public bool isBullet;
    public bool bounced;

    public Vector2 rbVelocity;

    public bool fastProjectile;
    public RaycastHit2D[] checkedObjs;
    public GameObject[] checkedGameObjs;

    public float hideTime = .03F;

    public float travelSpeed;

    //public float launchSpeed = 20;
    public float launchForce = 20;
    public float reflectForce= 30;

    public int damageValue;
    public bool damageActive = true;

    public bool blocked;

    public GameObject reflectedCopy;

    //8-08W Gets shooter facing at shot, not at reflect when shooter could be dead already
    public bool shooterDir;
    //  public bool reflectABLE;
    //8-08W Marks whether functions are being done during initial firing or reflected "firing"
    public bool reflecting;
    public bool reflected;
    public bool superReflected;

    protected bool blockable = true;

    protected bool fastBlocked = false;

    public IEnumerator Start()
    {
        //Debug.Log(transform.eulerAngles.z);
        myRigidBody = GetComponent<Rigidbody2D>();

        if (explosion != null)
        {
            yield return new WaitForSeconds(destroyTime);
            StartCoroutine(Explode());
        }  
        else
        Object.Destroy(gameObject, destroyTime);       
    }

    void FixedUpdate()
    {
        rbVelocity = myRigidBody.velocity;
        if (Physics2D.IsTouchingLayers(GetComponent<Collider2D>(), surface))
        {
            StartCoroutine(Explode());
        }
        /*
        if (!isPhysical)
        {
            float translation = Time.deltaTime * travelSpeed;
            transform.Translate(translation, 0, 0);
        }
        */
        if (isPhysical && Physics2D.IsTouchingLayers(GetComponent<Collider2D>(), surface))
        {
            if (bounced)
            {
                StartCoroutine(Explode());

                if (gameObject.GetComponent<Rigidbody2D>() != true)
                    myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 10);
            }
            else if (!bounced)
            {
                if (gameObject.GetComponent<Rigidbody2D>() != true)
                    myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, myRigidBody.velocity.y * .5F);
                bounced = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        hitCollider = other;
        CollisionID();
        if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            //bulletImpact.GetComponent<Explode>(transform.rotation, transform.localScale) 
            if (superReflected)
            {
                Instantiate(superExplAOE, transform.position, transform.rotation);
                StartCoroutine(Explode());
            }
            else if (explosion != null)
            {
                StartCoroutine(Explode());
                //Debug.Log("HitWall");
            }

            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //reflectABLE = false;

        if (other.CompareTag("Shield") && !reflected && damageActive == false)
        {
            GameObject.FindWithTag("PlayerArm").GetComponent<ArmPoses>().flashShield(3);
            StartCoroutine("Explode");
            //GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    /*
    public void ChangeVelocity(Vector2 vNew)
    {

        Debug.Log("LIES");
        Debug.Log(GetComponent<Rigidbody2D>().velocity);
        GetComponent<Rigidbody2D>().velocity = new Vector2 (vNew.x,vNew.y);
        Debug.Log("DAMNED LIES");
        Debug.Log(GetComponent<Rigidbody2D >().velocity);
    }
    */
    protected void CollisionID()
    {
        if (hitCollider.CompareTag("Shield") && GameObject.FindGameObjectWithTag("PlayerArm").GetComponent<ArmControl3>().holding != true && !reflected)
        {
            reflect();
            reflected = true;
        }
        else if (hitCollider.CompareTag("Shield") && isBullet && !reflected)
        {
            reflect();
            reflected = true;
        }

        else if (hitCollider.CompareTag("ShieldBlock"))
        {
            if (//clickGood && 
                blockable)
            {
                blocked = true;
                damageActive = false;
                GetComponent<SpriteRenderer>().enabled = false;
                if (explosion != null)
                {
                    StartCoroutine(Explode());
                }
                else
                    Destroy(gameObject);

                if (trailObject != null)
                    trailObject.SetActive(false);

                GameObject.FindWithTag("PlayerArm").GetComponent<ArmPoses>().flashShield(3);

                /*
                shieldSticker = new GameObject();
                shieldSticker.transform.parent = GameObject.FindGameObjectWithTag("PlayerArm").transform;

                shieldSticker.transform.position = transform.position;
                //shieldSticker.transform.localPosition = new Vector3 (transform.position.x - 1, transform.localPosition.y, transform.localPosition.z);

                shieldSticker.transform.rotation = GameObject.FindGameObjectWithTag("PlayerArm").transform.rotation;
                */
            }
        }

        else if (hitCollider.CompareTag("Hitbox_Player") && !hitCollider.CompareTag("ShieldBlock") && !reflected && damageActive)
        {

            fastBlockCheck();
            if (!fastBlocked)
            {
                if (hitCollider.GetComponent<PlayerHealthHitbox>().health.damageABLE)
                {
                    hitCollider.GetComponent<PlayerHealthHitbox>().health.HitCheck(damageValue, gameObject);
                }
                StartCoroutine(Explode());
            }
        }

        if (hitCollider.CompareTag("Enemy") && reflected && damageActive)
        {
            if (!hitCollider.gameObject.GetComponent<Health>().DodgeCheck(tag))
            {
                hitCollider.gameObject.GetComponent<Health>().HitCheck(damageValue, gameObject);
                StartCoroutine(Explode());
            }
        }

        if (hitCollider.CompareTag("breakPlat"))
        {
            hitCollider.gameObject.GetComponent<Health>().HitCheck(damageValue, gameObject);
            StartCoroutine(Explode());

        }
    }

    public void SpeedSet(float speedMod)
    {
        if (!reflecting)
            shooterDir = shooterArm.GetComponent<EnemyArm>().facingDefaultDir;
        myRigidBody = GetComponent<Rigidbody2D>();
        //transform.localScale = new Vector3(transform.localScale.x * shooterArm.GetComponent<EnemyArm>().currentFacingLeftMultiplier, transform.localScale.y, transform.localScale.z);
        //7-17R when commented no velocity
        myRigidBody.velocity = transform.rotation * new Vector2(launchForce, myRigidBody.velocity.y);
        //7-17R when commented no apparent change
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x + speedMod, myRigidBody.velocity.y);
    }

    public void reflect(float customAngle = default(float))
    {
        var randomMod = Quaternion.Euler(0, 0, 0);
        shield = GameObject.FindWithTag("PlayerArm");
        //StartCoroutine("flashShield");
        GameObject.FindWithTag("PlayerArm").GetComponent<ArmPoses>().flashShield(1);

        GetComponent<SpriteRenderer>().color = new Vector4(0.3F, 1F, 0F, 1);

        reflecting = true;

        if (isBullet)
        {
            if (GameObject.FindGameObjectWithTag("PlayerArm").GetComponent<ArmControl3>().holding && !GameObject.FindGameObjectWithTag("PlayerArm").GetComponent<ArmControl3>().tapped)
            {

                if (shooterDir)
                {
                    randomMod = Quaternion.Euler(0, 0, 40 - (Random.value * 80));

                }
                else
                {
                    randomMod = Quaternion.Euler(0, 0, 40 - (Random.value * 80));
                    //transform.localScale = new Vector3(-1 * transform.localScale.x, -1* transform.localScale.y, transform.localScale.z);
                }

                GetComponent<SpriteRenderer>().color = new Vector4(.5F, 0F, 0F, 1);

            }
        }

        //var reflectRotation = Quaternion.identity;

        //reflectRotation.eulerAngles = new Vector3 (0,0, shield.transform.rotation.eulerAngles.z);
        //RENAME PLAYERCONTROLLER SCRIPT WHEN NEW PLAYERCONTROLLER IS APPROVED
        //launchForce = -1* (reflectForce);
        //if (shooterArm.GetComponent<EnemyArm>().facingDefaultDir)

        //transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        if (!superReflected)
        {
            reflectedCopy = (GameObject)Instantiate(gameObject,
                GameObject.FindWithTag("ShieldReflectPoint")
                //shield
                .transform.position,
                //reflectRotation
                //*= Quaternion.Euler(0,0,180) 
                shield.transform.rotation * randomMod);
            //Debug.Log(shield.transform.rotation.eulerAngles);
        }
        else
        {
            reflectedCopy = (GameObject)Instantiate(gameObject, GameObject.FindWithTag("ShieldReflectPoint").transform.position, Quaternion.Euler(0,0,customReflectAngle));
            //Debug.Log(shield.transform.rotation.eulerAngles);
        }

        reflectedCopy.GetComponent<Projectile>().reflectSet();

        //Quaternion.Euler(new Vector3(0, 0, shield.transform.rotation.z))
        //Debug.Log(launchForce);

        Destroy(gameObject);
    }

    public void reflectSet()
    {
        //Shooooould reset the autodestruct when reflected, so the player's projectiles stay active much longer
        destroyTime += 3;
        StartCoroutine(Start());
        /*
        if (trailObject != null && !superReflected)

            StartCoroutine("tempTrailHide");
        else if (superReflected && trailObject != null)
        {
            trailObject.SetActive(false);
        }
        */

        SpeedSet(GameObject.FindWithTag("Player").GetComponent<PlayerController>().platSpeedX);

        blockable = false;
        shield = GameObject.FindWithTag("Shield");
        GetComponent<SpriteRenderer>().enabled = true;
   
        //Real time... COLOR CHANGE
        gameObject.layer = 17;

        damageValue += 1;
        travelSpeed *= 1.5F;
        /*
        //if (isPhysical)
        {
            myRigidBody = GetComponent<Rigidbody2D>();
            myRigidBody.velocity = transform.rotation * new Vector2(reflectForce, myRigidBody.velocity.y);
        }
        */

        if (fastProjectile)
        {
            StartCoroutine("tempHide");
        }
        reflected = true;
        damageActive = true;
    }

    public bool fastBlockCheck()
    {
        int ShieldBlockMask = LayerMask.GetMask("ShieldBlock", "PlayerHitbox");
        float rotationAngle = transform.rotation.eulerAngles.z;
        //Debug.Log(rotationAngle);
        rotationAngle *= Mathf.Deg2Rad;

        //Physics2D.queriesHitTriggers = true;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.rotation * Vector2.right, 7, ShieldBlockMask);
        //checkedObjs = Physics2D.RaycastAll(transform.position, Quaternion.Euler(0, 0, transform.localRotation.z + 180) * Vector2.right, 15, ShieldBlockMask);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.rotation * Vector2.right, 7, ShieldBlockMask);

        checkedObjs = Physics2D.RaycastAll(transform.position, transform.rotation * Vector2.left , 15, ShieldBlockMask);

        //checkedObjs = Physics2D.RaycastAll(transform.position, new Vector2 (transform.position.x + Mathf.Cos(rotationAngle), transform.position.y + Mathf.Sin(rotationAngle)), 15, ShieldBlockMask);

        checkedGameObjs = new GameObject [checkedObjs.Length];

        int shieldBlockOrder = -1;
        int playerHitboxOrder = -1;

        for (int i = 0; i < checkedObjs.Length; i++)
        {
            checkedGameObjs[i] = checkedObjs[i].collider.gameObject;
            if (checkedObjs[i].collider.CompareTag("ShieldBlock"))
                shieldBlockOrder = i;
            else if (checkedObjs[i].collider.CompareTag("Hitbox_Player"))
                playerHitboxOrder = i;

            //Debug.Log(checkedGameObjs[i].name);
        }

        if ((playerHitboxOrder > -1 && shieldBlockOrder > -1) && playerHitboxOrder < shieldBlockOrder)
            fastBlocked = true;
        if (playerHitboxOrder < 0 && shieldBlockOrder > 0)
            fastBlocked = true;


        Debug.DrawRay(transform.position, transform.rotation * Vector2.left * 15, Color.cyan);

        /*
        if (hit.collider != null)
        {
            fastBlocked = true;
        }
        */
        if (fastBlocked == true)
            GameObject.FindWithTag("PlayerArm").GetComponent<ArmPoses>().flashShield(3);

        return fastBlocked;
    }

    public void superDeflectSet()
    {
        explosion = superExplosion;

        customReflectAngle = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSpecialMoves>().superDeflectAngle;
        superReflected = true;

        if (trailObject != null)
            trailObject.SetActive(false);
    }

    public IEnumerator Explode()
    {
        GetComponent<Collider2D>().enabled = false;
        {
            yield return new WaitForSeconds(.01F);
            if (superReflected)
                Instantiate(superExplAOE, transform.position, transform.rotation);

            if (explosion != null)
                Instantiate(explosion, transform.position, transform.rotation);

            Destroy(gameObject);
                 
            /*
            if (damageActive)
            {
                yield return new WaitForSeconds(.5F);
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else
            */
            //if (blocked || bounced)

            /*
            Instantiate(explosion, shieldSticker.transform.position, shieldSticker.transform.rotation * Quaternion.Euler(0, 0, -90));
            Destroy(shieldSticker);
            Destroy(gameObject);
            */

        }
    }

    //Explodes without setting off AOE
    public IEnumerator Disable()
    {
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(.01F);

        if (explosion != null)
            Instantiate(explosion, transform.position, transform.rotation);

        Destroy(gameObject);
    }
    IEnumerator tempHide()

    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(hideTime);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(.02F);
    }
    IEnumerator tempTrailHide()

    {
        trailObject.SetActive(false);
        yield return new WaitForSeconds(hideTime);
        trailObject.SetActive(true);
    }
}