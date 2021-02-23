using UnityEngine;
using System.Collections;

public class SuperDeflect : MonoBehaviour
{

    public GameObject shield;
    public GameObject playerSpriteObject;
    public GameObject reflectedObject;
    public GameObject SuperDeflectProj;
    public GameObject SuperDeflectProjInstance;
    public GameObject flashParryPrefab;
    public GameObject flashParryInstance;

    public float actionTime;
    public float launchForce;

    [Header("Use ONE of these to select which behavior to perform")]
    public bool flashParries;
    public bool superDeflects;

    //private PlayerController playerScript;
    protected Collider2D hitCollider;

    public int damageValue;

    private Transform playerArm;
    private Transform shieldReflectPoint;

    //public GameObject explosion;

    void Start()
    {
        playerArm = GameObject.FindGameObjectWithTag("PlayerArm").transform;
        shieldReflectPoint = GameObject.FindGameObjectWithTag("ShieldReflectPoint").transform;

        transform.SetParent(playerArm);
        transform.position = shieldReflectPoint.position;
        //playerSpriteObject = GameObject.FindGameObjectWithTag("PlayerTorso");

        Object.Destroy(gameObject, actionTime);

    }
    /*
    void Update()
    {
        /*
        if (reflectedObject != null && reflectedObject.CompareTag("MedProjectile"))
        {
        }
        */
        /*
        if (reflectedObject != null && reflectedObject.GetComponent<Projectile>().isPhysical)
        {
            transform.rotation = reflectedObject.transform.rotation;
        }

    }
    */

    void OnTriggerEnter2D(Collider2D other)
    {
        hitCollider = other;
        CollisionID();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        hitCollider = other;
        CollisionID();
    }

    protected void CollisionID()
    {
        //playerSpriteObject = GameObject.FindGameObjectWithTag("PlayerTorso");


        if (hitCollider.CompareTag("Enemy"))
        {
            //Instantiate(explosion, transform.position, transform.rotation);
            hitCollider.gameObject.GetComponent<Health>().Damage(damageValue);
            //Destroy(gameObject);
        }

        if (hitCollider.CompareTag("Bullet"))
        {

        }
        /*  NOTES NOTES NOTES NOTES
        Grenade probably needs either another tag or projectiles need that generic script to be referenced.
        Might be better to make the superReflected projectile a new straight non-kinematic object, 
        and have it take the sprite of the reflected object in a random rotation 

        */
        else if (hitCollider.CompareTag("Explosive"))
        {
            if (superDeflects)
            {
                reflectedObject = hitCollider.gameObject;

                if (!reflectedObject.GetComponent<Projectile>().superReflected)
                {
                    reflectedObject.GetComponent<Projectile>().superDeflectSet();

                    reflectedObject.GetComponent<Projectile>().reflect();

                    reflectedObject.transform.position = transform.position + new Vector3(1, 0, 0);
                    if (reflectedObject.GetComponent<Projectile>().isPhysical)
                    {
                        reflectedObject.GetComponent<Rigidbody2D>().velocity = playerArm.transform.rotation * new Vector2(launchForce, 0);
                    }

                    reflectedObject.transform.rotation = playerArm.transform.rotation;

                    //http://forum.unity3d.com/threads/instantiating-prefab-as-child-of-existing-gameobject-c.83598/
                    (Instantiate(SuperDeflectProj, transform.position, playerArm.transform.rotation) as GameObject).transform.parent = reflectedObject.transform;

                    reflectedObject.GetComponent<Projectile>().reflected = true;

                }
            }

            if (flashParries)
            {
                //Currently nulls at trying to find shieldReflectPoint, even though it should be set at start
                (Instantiate(flashParryPrefab, 
                    shieldReflectPoint.position, 
                    shieldReflectPoint.transform.rotation) 
                    as GameObject).transform.parent = 
                    playerArm.transform;
                Destroy(gameObject);
            }
        }
    }
}