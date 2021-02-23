using UnityEngine;
using System.Collections;

public class PhysicsObjectInteractions : MonoBehaviour
{
    public bool playerColSafe;
    public bool superDeflect;
    public bool coRouteLimiter;
    public bool hingeless;
    public GameObject enemy;
    public GameObject player;
    public GameObject explosive;
    public int enemyDamage;
    public int playerDamage;
    public float speedForDamage;
    public float knockBackTime;

    private HingeJoint2D hinge;
    public float netSpeed;
    private SpriteRenderer physObj;
    private bool enemyDamagable;
    private bool playerDamagable;
    private float damageEnemyTime = 2f;
    [HideInInspector]
    public Rigidbody2D thisRigidBody;
    // Use this for initialization
    //ADD a not touching to the wrecking ball, prevents ms gain when player is moving it
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        physObj = GetComponent<SpriteRenderer>();
        thisRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //gets the speed for the obj this script is attached to
        var speed = thisRigidBody.velocity.magnitude;
        netSpeed = speed;
        //if fast enough allow for other gamneobjects to take damage
        if (netSpeed >= speedForDamage && !playerColSafe)
        {
            enemyDamagable = true;
            playerDamagable = true;
        }
        else
        {
            enemyDamagable = false;
            playerDamagable = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            enemy = other.gameObject;
            if (enemyDamagable)
            {
                enemy.GetComponent<Health>().health -= enemyDamage;
            }
            //if enemy dies makes sure that enemy becomes null
            if (enemy != null && enemy.GetComponent<Health>().health <= 0)
            {
                enemy = null;
            }
        }
        else if (other.tag == "Explosive")
        {
            explosive = other.gameObject;
            if (explosive.GetComponent<Projectile>().reflected)
            {
                hinge.enabled = false;
                hingeless = true;
                //ANGLE CODE, send this gameobject at angle of explosive
            }
        }
        //8-17R testing
        if (other.tag == "StaticEnemy")
        {
            Debug.Log("seen");
            other.gameObject.GetComponent<Health>().health = 0;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            enemy = null;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ForceBall")
        {
            //defines superdeflect here and if the forceball is super deflected unhinge the object
            superDeflect = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSpecialMoves>().superDeflecting;
            if (superDeflect)
            {
                hinge.enabled = false;
                hingeless = true;
            }
            StartCoroutine("DamageEnemies");
        }
        if (col.gameObject.tag == "Player")
        {
            player = col.gameObject;
            if (netSpeed < speedForDamage)
            {
                playerColSafe = true;
            }
            //modify
            if (playerDamagable && !coRouteLimiter)
            {
                player.GetComponent<Health>().Damage(playerDamage);
                playerDamagable = false;
                //StartCoroutine("KnockBack");
                //player.GetComponent<Rigidbody2D>().velocity += new Vector2(-thisRigidBody.velocity.x/2,0); 
            }
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //player = null;
            playerColSafe = false;
        }
    }
    IEnumerator DamageEnemies()
    {
        //physObj.color = new Color(0.1f, 0.5f, 0.1f, 1);
        enemyDamagable = true;
        yield return new WaitForSeconds(damageEnemyTime);
        enemyDamagable = false;
    }
    //pushes player gameobject in opposite direct of this gameobject
    IEnumerator KnockBack()
    {
        coRouteLimiter = true;
        player.GetComponent<PlayerController>().moveSpeed = thisRigidBody.velocity.x * 2;
        player.GetComponent<PlayerController>().jumpForce = thisRigidBody.velocity.y * 2;
        //Debug.Log("NO Control");
        player.GetComponent<PlayerController>().control = false;
        yield return new WaitForSeconds(knockBackTime);
        //Debug.Log("Control");
        player.GetComponent<PlayerController>().control = true;
        coRouteLimiter = false;
    }
}


