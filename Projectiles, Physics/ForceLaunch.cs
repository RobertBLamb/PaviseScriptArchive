using UnityEngine;
using System.Collections;

public class ForceLaunch : MonoBehaviour {

    public GameObject player;
    public GameObject playerArm;

    public float specialAngle;

	public float ForceX = 50;

	private Rigidbody2D myRigidBody;

	// Use this for initialization
	void Start () {

        Object.Destroy(gameObject, .1F);

        player = GameObject.FindWithTag("Player");
        playerArm = GameObject.FindWithTag ("Shield");

        myRigidBody = GetComponent<Rigidbody2D>();
        /*
        if (player.GetComponent<PlayerSpecialMoves>().superDeflecting)
        {
            myRigidBody.velocity = Quaternion.Euler(0, 0, player.GetComponent<PlayerSpecialMoves>().superDeflectAngle) * new Vector2(ForceX, myRigidBody.velocity.y);
            myRigidBody.AddForce(Quaternion.Euler(0, 0, player.GetComponent<PlayerSpecialMoves>().superDeflectAngle) * (transform.forward * ForceX));
        }

        else if (playerArm != null)
        {
            myRigidBody.velocity = playerArm.transform.rotation * new Vector2(ForceX, myRigidBody.velocity.y);
            myRigidBody.AddForce(playerArm.transform.rotation * (transform.forward * ForceX));
        }
        else
        {
            myRigidBody.velocity = new Vector2(ForceX, myRigidBody.velocity.y);
            myRigidBody.AddForce(transform.forward * ForceX);
        }
        */
        myRigidBody.velocity = transform.localRotation * new Vector2(ForceX, myRigidBody.velocity.y);
        myRigidBody.AddForce(transform.localRotation * transform.forward * ForceX);

    }
	// Update is called once per frame
	void Update () {
	
		// Automatic deletion when far enough
        if (playerArm != null)
		if (Vector3.Distance (playerArm.transform.position, transform.position) > 5) {

            Destroy(gameObject);
        }
		// Track with position variable at spawn for more efficiency?

	}
}
