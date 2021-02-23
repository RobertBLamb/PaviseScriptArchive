using UnityEngine;
using System.Collections;

public class PlayerSpecialHitbox : MonoBehaviour
{

    public GameObject shield;
    public float actionTime;

    //private PlayerController playerScript;
    protected Collider2D hitCollider;

    public int damageValue;

    //public GameObject explosion;

    void Start()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        Object.Destroy(gameObject, actionTime);

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


