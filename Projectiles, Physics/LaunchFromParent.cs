using UnityEngine;
using System.Collections;

public class LaunchFromParent : Projectile {

    public Rigidbody2D myRigidBody;

    // Use this for initialization
    new void Start()
    {
        Object.Destroy(gameObject, destroyTime);
        myRigidBody = GetComponent<Rigidbody2D>();


        myRigidBody.velocity = transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity * 2F;
        //Vector2(launchForce, myRigidBody.velocity.y);
        transform.parent = null;

    }
    // Update is called once per frame
    void Update () {
	
	}
}
