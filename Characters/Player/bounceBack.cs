using UnityEngine;
using System.Collections;
//new!!!!!!!!!!!!
public class bounceBack : MonoBehaviour
{
    private Rigidbody2D playerBody;
    private float platLength;
    private float timeTouchingPlat;
    private bool enableEnemy;
    private bool enablePlat;
    private bool enableHazard;
    private float enemyLength;
    private bool touchingPlat;
    private bool touchingHazard;
    private float hazardLength;
    private bool touchingEnemy;
    private float timeToGetCrushed;
    private bool isEnemy;
    //prevents bounceback while using a special
    public bool usingSpecial;


    //public bool triggered1, triggered2, triggered3, triggered4, triggered5;
    public float maxTouchTime;
    public float boostSpeed;
    public float boostTime;
    public int damageAmount;
    public bool boosted;
    public int damageAmountEnemy;
    public float boostSpeedEnemy;
    public float boostTimeEnemy;
    public int damageAmountHazard;
    public float boostSpeedHazard;
    public float boostTimeHazard;

    //modify value in inspector
    public float crushedReset;


    // Use this for initialization
    void Start()
    {
        //
        playerBody = GetComponent<Rigidbody2D>();
        timeToGetCrushed = crushedReset;
        if (tag == "Player")
        {
            enableEnemy = true;
            enableHazard = true;
            enablePlat = true;
        }
        else if (tag == "Enemy")
        {
            enableHazard = true;
            enablePlat = true;
            isEnemy = true;
        }
    }
    void Update()
    {
        //8-17R reenabled
        if(!isEnemy)
            usingSpecial = GetComponent<PlayerSpecialMoves>().genericSpecial;
        if (touchingPlat && enablePlat)
        {
            timeTouchingPlat += 1 * Time.deltaTime;
            if (timeTouchingPlat >= maxTouchTime)
            {
                //function
                if (transform.position.x < platLength && !boosted && !isEnemy)
                {
                    StartCoroutine("pushBack");
                }
                else if (transform.position.x > platLength && !boosted && !isEnemy)
                {
                    StartCoroutine("pushForward");
                }
                else if (transform.position.x < platLength && !boosted && isEnemy)
                {
                    StartCoroutine("pushBack4E");
                }
                else if (transform.position.x > platLength && !boosted && isEnemy)
                {
                    StartCoroutine("pushForward4E");
                }
                //for killing player while under plat for too long
                timeToGetCrushed -= Time.deltaTime;
                if (timeToGetCrushed <= 0)
                {
                    GetComponent<Health>().health = 0;
                }
            }
        }
        else
        {
            timeToGetCrushed = crushedReset;
        }

        if (touchingEnemy && enableEnemy)
        {
            playerBody = GetComponent<PlayerController>().playerRigidBody;
            if (transform.position.x < enemyLength && !boosted && !usingSpecial)
            {
                StartCoroutine("pushBackEnemy");
            }
            else if (transform.position.x > enemyLength && !boosted && !usingSpecial)
            {
                StartCoroutine("pushForwardEnemy");
            }
        }
        else if (touchingHazard && enableHazard)
        {
            if (!isEnemy)
            {
                playerBody = GetComponent<PlayerController>().playerRigidBody;
                if (transform.position.x < hazardLength && !boosted && !usingSpecial)
                {
                    StartCoroutine("pushBackHazard");
                }
                else if (transform.position.x > hazardLength && !boosted && !usingSpecial)
                {
                    StartCoroutine("pushForwardHazard");
                }
            }
            else if (isEnemy)
            {
                if (transform.position.x < hazardLength && !boosted)
                {
                    StartCoroutine("pushBackHazard4E");
                }
                else if (transform.position.x > hazardLength && !boosted)
                {
                    StartCoroutine("pushForwardHazard4E");
                }
            }
        }
        //makes sure bounce only triggers once
        //if a trigger and enemy dies would go on forever
        if (touchingEnemy = true && enemyLength != 0)
        {
            touchingEnemy = false;
            enemyLength = 0;
        }
        else if (touchingHazard = true && hazardLength != 0)
        {
            touchingHazard = false;
            hazardLength = 0;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            enemyLength = col.gameObject.transform.position.x;
            touchingEnemy = true;
        }
        else if (col.tag == "StageHazard")
        {
            hazardLength = col.gameObject.transform.position.x;
            touchingHazard = true;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "underSPlat")
        {
            platLength = col.transform.gameObject.transform.position.x;
            touchingPlat = true;
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "underSPlat")
        {
            platLength = 0;
            touchingPlat = false;
            timeTouchingPlat = 0;
        }
    }
    IEnumerator pushForward()
    {
        boosted = true;
        GetComponent<PlayerController>().control = false;
        GetComponent<PlayerController>().moveSpeed = boostSpeed;
        GetComponent<PlayerController>().playerRigidBody.velocity = new Vector2(GetComponent<PlayerController>().platSpeedX
            + GetComponent<PlayerController>().moveSpeed, GetComponent<PlayerController>().playerRigidBody.velocity.y);
        GetComponent<Health>().health -= damageAmount;
        GetComponent<Health>().damageABLE = false;
        yield return new WaitForSeconds(boostTime);
        GetComponent<PlayerController>().control = true;
        boosted = false;
        GetComponent<Health>().damageABLE = true;

        yield return new WaitForSeconds(1f);
    }
    IEnumerator pushBack()
    {
        boosted = true;
        GetComponent<PlayerController>().control = false;
        GetComponent<PlayerController>().moveSpeed = -1 * boostSpeed;
        GetComponent<PlayerController>().playerRigidBody.velocity = new Vector2(GetComponent<PlayerController>().platSpeedX
            + GetComponent<PlayerController>().moveSpeed, GetComponent<PlayerController>().playerRigidBody.velocity.y);
        GetComponent<Health>().health -= damageAmount;
        GetComponent<Health>().damageABLE = false;
        yield return new WaitForSeconds(boostTime);
        GetComponent<PlayerController>().control = true;
        boosted = false;
        GetComponent<Health>().damageABLE = true;

        yield return new WaitForSeconds(1f);
    }
    IEnumerator pushForwardEnemy()
    {
        boosted = true;
        GetComponent<PlayerController>().control = false;
        GetComponent<PlayerController>().moveSpeed = boostSpeedEnemy;
        GetComponent<PlayerController>().playerRigidBody.velocity = new Vector2(GetComponent<PlayerController>().platSpeedX
            + GetComponent<PlayerController>().moveSpeed, GetComponent<PlayerController>().playerRigidBody.velocity.y);
        GetComponent<Health>().health -= damageAmountEnemy;
        GetComponent<Health>().damageABLE = false;
        yield return new WaitForSeconds(boostTimeEnemy);
        GetComponent<PlayerController>().control = true;
        boosted = false;
        GetComponent<Health>().damageABLE = true;

        yield return new WaitForSeconds(1f);
    }
    IEnumerator pushBackEnemy()
    {
        boosted = true;
        GetComponent<PlayerController>().control = false;
        GetComponent<PlayerController>().moveSpeed = -1 * boostSpeedEnemy;
        GetComponent<PlayerController>().playerRigidBody.velocity = new Vector2(GetComponent<PlayerController>().platSpeedX
            + GetComponent<PlayerController>().moveSpeed, GetComponent<PlayerController>().playerRigidBody.velocity.y);
        GetComponent<Health>().health -= damageAmountEnemy;
        GetComponent<Health>().damageABLE = false;
        yield return new WaitForSeconds(boostTimeEnemy);
        GetComponent<PlayerController>().control = true;
        boosted = false;
        GetComponent<Health>().damageABLE = true;

        yield return new WaitForSeconds(1f);
    }
    IEnumerator pushForwardHazard()
    {
        boosted = true;
        GetComponent<PlayerController>().control = false;
        GetComponent<PlayerController>().moveSpeed = boostSpeedHazard;
        GetComponent<PlayerController>().playerRigidBody.velocity = new Vector2(GetComponent<PlayerController>().platSpeedX
            + GetComponent<PlayerController>().moveSpeed, GetComponent<PlayerController>().playerRigidBody.velocity.y);
        GetComponent<Health>().health -= damageAmountHazard;
        GetComponent<Health>().damageABLE = false;
        yield return new WaitForSeconds(boostTimeHazard);
        GetComponent<PlayerController>().control = true;
        boosted = false;
        GetComponent<Health>().damageABLE = true;

        yield return new WaitForSeconds(1f);
    }
    IEnumerator pushBackHazard()
    {
        boosted = true;
        GetComponent<PlayerController>().control = false;
        GetComponent<PlayerController>().moveSpeed = -1 * boostSpeedHazard;
        GetComponent<PlayerController>().playerRigidBody.velocity = new Vector2(GetComponent<PlayerController>().platSpeedX
            + GetComponent<PlayerController>().moveSpeed, GetComponent<PlayerController>().playerRigidBody.velocity.y);
        GetComponent<Health>().health -= damageAmountHazard;
        GetComponent<Health>().damageABLE = false;
        yield return new WaitForSeconds(boostTimeHazard);
        GetComponent<PlayerController>().control = true;
        boosted = false;
        GetComponent<Health>().damageABLE = true;

        yield return new WaitForSeconds(1f);
    }
    IEnumerator pushForward4E()
    {
        boosted = true;
        GetComponent<ChaserEnemy>().control = false;
        playerBody.velocity = new Vector2(boostSpeed, playerBody.velocity.y);
        GetComponent<Health>().health -= damageAmount;
        GetComponent<Health>().damageABLE = false;
        yield return new WaitForSeconds(boostTime);
        GetComponent<ChaserEnemy>().control = true;
        boosted = false;
        GetComponent<Health>().damageABLE = true;
        yield return new WaitForSeconds(1f);
    }
    IEnumerator pushBack4E()
    {
        boosted = true;
        GetComponent<ChaserEnemy>().control = false;
        playerBody.velocity = new Vector2(-1 * boostSpeed, playerBody.velocity.y);
        GetComponent<Health>().health -= damageAmount;
        GetComponent<Health>().damageABLE = false;
        yield return new WaitForSeconds(boostTime);
        GetComponent<ChaserEnemy>().control = true;
        boosted = false;
        GetComponent<Health>().damageABLE = true;
        yield return new WaitForSeconds(1f);
    }
    IEnumerator pushForwardHazard4E()
    {
        boosted = true;
        GetComponent<ChaserEnemy>().control = false;
        //GetComponent<PlayerController>().moveSpeed = boostSpeedHazard;
        playerBody.velocity = new Vector2(boostSpeedHazard, playerBody.velocity.y);
        GetComponent<Health>().health -= damageAmountHazard;
        GetComponent<Health>().damageABLE = false;
        yield return new WaitForSeconds(boostTimeHazard);
        GetComponent<ChaserEnemy>().control = true;
        boosted = false;
        GetComponent<Health>().damageABLE = true;

        yield return new WaitForSeconds(1f);
    }
    IEnumerator pushBackHazard4E()
    {
        boosted = true;
        GetComponent<ChaserEnemy>().control = false;
        playerBody.velocity = new Vector2(-1 * boostSpeedHazard, playerBody.velocity.y);
        //GetComponent<PlayerController>().moveSpeed = -1 * boostSpeedHazard;
        GetComponent<Health>().health -= damageAmountHazard;
        GetComponent<Health>().damageABLE = false;
        yield return new WaitForSeconds(boostTimeHazard);
        GetComponent<ChaserEnemy>().control = true;
        boosted = false;
        GetComponent<Health>().damageABLE = true;

        yield return new WaitForSeconds(1f);
    }
}
