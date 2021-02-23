using UnityEngine;
using System.Collections;

public class DashHit : MonoBehaviour
{

    public GameObject shield;

    private PlayerController playerScript;
    protected Collider2D hitCollider;

    public int damageValue;

    protected bool blockable = true;
    protected bool reflectABLE;
    protected bool reflected;

    //public GameObject explosion;
    //public Transform target1;
    //public Transform target2;

    void Start()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        Object.Destroy(gameObject, .5F);

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        hitCollider = other;
        CollisionID();

    }

    protected void CollisionID()
    {

        if (hitCollider.CompareTag("Enemy"))
        {
            //Instantiate(explosion, transform.position, transform.rotation);

            hitCollider.gameObject.GetComponent<Health>().Damage(damageValue);
            //Destroy(gameObject);
        }
    }
}


