using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinigunArm : EnemyArm
{
    public float traversalSpeed = -0.35F;

    private bool aimSet = false;
    public GameObject visionMini;
    //Spawn
    //Track player for short time, + target edit X distance
    //Begin shooting, sweeping (angular translate) torwards player
    //"Reload"
    //Retrack player

    // Use this for initialization
    void Start()
    {
        if (startsFacingLeft)
            defaultFacingLeftMultiplier = 1;
        else
            defaultFacingLeftMultiplier = -1;

        shooter = transform.parent.gameObject;

        defaultSprite = GetComponent<SpriteRenderer>().sprite;
        player = GameObject.FindGameObjectWithTag("Player");

        isTraverse = true;
        StartCoroutine(traverseCycle());

    }
    /*
    IEnumerator AimTimer()
    {
        
    }
    */
    void FixedUpdate()
    {
        localScaleX = Mathf.RoundToInt(transform.localScale.x);
        shooterLocalScaleX = Mathf.RoundToInt(transform.parent.localScale.x);

        if (Vector2.Distance(transform.position, player.transform.position) < sightRadius)
            angleCheck();

        if (aimSet == false)
        {
            //var angle = 0;
            if (facingDefaultDir)
                transform.rotation = Quaternion.AngleAxis(angle + angleEdit, Vector3.forward);
            else
                transform.rotation = Quaternion.AngleAxis(angle + 180 + angleEdit, Vector3.forward);
        }
        if (aimSet == true)
        {

            transform.rotation *= Quaternion.Euler(0, 0, traversalSpeed );
        }

        if (shooterMovingPlat != null)
        {
            vXMovingPlat = shooterMovingPlat.GetComponent<Rigidbody2D>().velocity.x;
            vXMovingPlat = shooterMovingPlat.GetComponent<Rigidbody2D>().velocity.y;

        }

        if (shotProjectile != null)
        {
            shotProjectile.GetComponent<Projectile>().shooterArm = this.gameObject;

            vXShotProjectile = shotProjectile.GetComponent<Rigidbody2D>().velocity.x;

            shotProjectile.GetComponent<Rigidbody2D>().velocity += new Vector2(vXMovingPlat, vYMovingPlat);
        }

        //if (shotProjectile != null)
        //Debug.Log(shotProjectile.GetComponent<Rigidbody2D>().velocity);
        if (facingDefaultDir)
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        else
            transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
    }

  protected IEnumerator traverseCycle()
    {
        while (0 == 0)
        {
            //09-12-17W Holds coroutine if player is out of range, BETWEEN RELOADS
            while (Vector2.Distance(transform.position, player.transform.position) > sightRadius)
                yield return new WaitForSeconds(.2F);

            yield return new WaitForSeconds(readyDelay);
            aimSet = true;
            for (int i = 0; i < magSize; i++)
            {
                //movingPlatCheck();
                //X1-18R temp disabled until update from mr will
                //if (visionMini.GetComponent<EnemyVision>().sighted)
                {
                    shoot();
                }
                yield return new WaitForSeconds(shootDelay);
            }
            aimSet = false;
            yield return new WaitForSeconds(reloadTimeR);
        }
    }
}