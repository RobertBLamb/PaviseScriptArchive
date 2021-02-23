using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyArm : MonoBehaviour
{

    public int localScaleX;
    public int shooterLocalScaleX;
    //Make sure to uncheck for enemies that go right, usually chasers
    public bool startsFacingLeft;
    public int defaultFacingLeftMultiplier;
    public int currentFacingLeftMultiplier;


    public GameObject shooter;
    public GameObject shooterMovingPlat;
    public float vXMovingPlat;
    public float vYMovingPlat;

    public bool isTraverse;
    public GameObject vision;
    public Transform projectileSpawn;
    public GameObject projectile;
    public GameObject shotProjectile;
    public GameObject nextShotProjectile;


    public GameObject player;
    //velocity of Player
    public float vPlayer;
    //Target a set distance from player

    public float vXShotProjectile;
    public float vYShotProjectile;

    //First projectile shot, used as sample of velocity
    //public GameObject firstShot;
    //velocity of Projectile

    public float vProjectile = 0;

    protected Sprite defaultSprite;
    public Sprite[] firingPoses;

    public float readyDelay = 1F;
    public float shootDelay = .8F;
    public float reloadTimeR = 2;
    public int magSize = 4;

    //09-12-17W Finally prevents enemies from trying to shoot you from across the level and spawning way too many projectiles
    public float sightRadius = 30;

    public bool facingDefaultDir = false;

    public bool leadTarget;
    public Vector3 targetEdit;
    public Vector3 currentTargetEdit;
    public float vXEditRatio = .3F;
    public float vYEditRatio = .1F;
    public float angle;
    public float angleEdit;
    public float debugAngle1;
    public float debugAngle2;

    public float defaultAngle;

    public bool limited;
    public float lowerAngleLim = 0;
    public float upperAngleLim = 360;

    // Use this for initialization
    void Start()
    {
        if (startsFacingLeft)
            defaultFacingLeftMultiplier = 1;
        else
            defaultFacingLeftMultiplier = -1;

        //Check if minigunner script is present
        isTraverse = GetComponent<MinigunArm>();

        if (!isTraverse)
        {
            StartCoroutine(shootCycle());
        }

        //bool platSet = false;

        shooter = transform.parent.gameObject;
        //if (GetComponent<SpriteRenderer>() != null)
        defaultSprite = GetComponent<SpriteRenderer>().sprite;
        player = GameObject.FindGameObjectWithTag("Player");

        defaultAngle = transform.rotation.eulerAngles.z;
    }
    void FixedUpdate()
    {
        localScaleX = Mathf.RoundToInt(transform.localScale.x);
        shooterLocalScaleX = Mathf.RoundToInt(transform.parent.localScale.x);

        //if (shotProjectile != null)
        //Debug.Log(shotProjectile.GetComponent<Rigidbody2D>().velocity);
        if (Vector2.Distance(transform.position, player.transform.position) < sightRadius)
            angleCheck();

        if(facingDefaultDir || (!startsFacingLeft && facingDefaultDir))
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        else
            transform.rotation = Quaternion.AngleAxis(angle + 180, Vector3.forward);
    }

    protected IEnumerator shootCycle()
    {
        if (isTraverse)
            StopCoroutine(shootCycle());
            yield return new WaitForSeconds(readyDelay);
        while (0 == 0)
        {
            //09-12-17W Holds coroutine if player is out of range, BETWEEN RELOADS
            while (Vector2.Distance(transform.position, player.transform.position) > sightRadius)
            {
                if (facingDefaultDir || (!startsFacingLeft && facingDefaultDir))
                    transform.rotation = Quaternion.Euler(0, 0, defaultAngle);
                else
                    transform.rotation = Quaternion.Euler(0, 0, defaultAngle + 180);

                yield return new WaitForSeconds(.2F);
            }
            if (leadTarget)
            {
                /*
                if (vProjectile == 0)
                    firstShot = (Instantiate(projectile, projectileSpawn.position, transform.rotation) as GameObject);
                */
                if (!projectile.GetComponent<Projectile>().isPhysical)
                {
                    //vProjectile = firstShot.GetComponent<Rigidbody2D>().velocity.magnitude;
                    vProjectile = Mathf.Abs(projectile.GetComponent<Projectile>().travelSpeed);
                }
            }

            for (int i = 0; i < magSize; i++)
            {
                //movingPlatCheck();
                //X1-18R temp disabled until update from mr will
                //if (vision.GetComponent<EnemyVision>().sighted)
                {
                    shoot();
                }
                if (firingPoses.Length > 0)
                {
                    if (firingPoses.Length > 1)
                    {
                        //Need to be changed so it adapts to length of array
                        if (Random.value < .5F)
                            GetComponent<SpriteRenderer>().sprite = firingPoses[0];
                        else
                            GetComponent<SpriteRenderer>().sprite = firingPoses[1];
                    }

                    else
                        GetComponent<SpriteRenderer>().sprite = firingPoses[0];
                }
                yield return new WaitForSeconds(.07F);
                GetComponent<SpriteRenderer>().sprite = defaultSprite;

                yield return new WaitForSeconds(shootDelay);
            }
            yield return new WaitForSeconds(reloadTimeR);
        }
    }
    protected void angleCheck()
    {
        var pos = transform.position;
        //+ (0,3,0) is permanent correction of targeting since the player obj's "center" is now at its pivot point on the bottom.
        var dir = pos - (player.transform.position + new Vector3 (0,2) + currentTargetEdit);
        angle =
            //Mathf.Sign(shooter.transform.localScale.x) * 
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Angle range when enemy is aiming forwards
        if ((angle > 0 && angle < 90) || (angle < 0 && angle > -90))
        {
            shooter.transform.localScale = new Vector3(Mathf.Abs(shooter.transform.localScale.x) * defaultFacingLeftMultiplier, shooter.transform.localScale.y, shooter.transform.localScale.z);
            currentFacingLeftMultiplier = 1;
            facingDefaultDir = true;

        }
        else
        {
            //if ((angle < -90 && angle >= -180) || (angle >= 90 ))
            shooter.transform.localScale = new Vector3(Mathf.Abs(shooter.transform.localScale.x) * -1 * defaultFacingLeftMultiplier, shooter.transform.localScale.y, shooter.transform.localScale.z);
            currentFacingLeftMultiplier = -1;
            facingDefaultDir = false;
        }
        debugAngle1 = angle;

        //vPlayer = player.GetComponent<Rigidbody2D>().velocity.magnitude;

        if (leadTarget)
        {
            currentTargetEdit = new Vector3(targetEdit.x + vXEditRatio * player.GetComponent<Rigidbody2D>().velocity.x, targetEdit.y + vYEditRatio * player.GetComponent<Rigidbody2D>().velocity.y);
            /*
                angleEdit = Mathf.Asin(vPlayer / vProjectile) * Mathf.Rad2Deg;
                angle = angleEdit;
            */
        }
        debugAngle2 = angle;
        if (limited)
        {
            if (angle > upperAngleLim)
                angle = upperAngleLim;
            if (angle < lowerAngleLim)
                angle = lowerAngleLim;
        }
    }

    /* protected void movingPlatCheck()
    {
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

        //Supports plugging in a specific plat!!!
        if (shooterMovingPlat == null)
        {
            if (shooter.transform.parent != null)
                if (shooter.transform.parent.transform.parent != null)
                    //Default plat found based on parent of plat being actual moving obj
                    if (shooter.transform.parent.transform.parent.GetComponent<Rigidbody2D>() != null)
                    {
                        shooterMovingPlat = shooter.transform.parent.transform.parent.gameObject;
                    }
        }

        if (shooterMovingPlat != null)
        {
            vXMovingPlat = shooterMovingPlat.GetComponent<Rigidbody2D>().velocity.x;
            vYMovingPlat = shooterMovingPlat.GetComponent<Rigidbody2D>().velocity.y;
        }
            //shotProjectile.GetComponent<Rigidbody2D>().velocity += new Vector2(vXMovingPlat, vYMovingPlat);
    }

        */

    protected void shoot()
    {
        if (facingDefaultDir)
            shotProjectile = (GameObject)Instantiate(projectile, projectileSpawn.position, transform.rotation * Quaternion.Euler(0, 0, 180));
        else
            shotProjectile = (GameObject)Instantiate(projectile, projectileSpawn.position, transform.rotation);

        shotProjectile.GetComponent<Projectile>().shooterArm = gameObject;
        shotProjectile.GetComponent<Projectile>().SpeedSet(vXMovingPlat);
        //X1-18R temp changed
        //shotProjectile.GetComponent<Projectile>().SpeedSet(player.GetComponent<PlayerController>().moveSpeedMod);

    }


    //Correct velocity when shooter is on moving plat
    /*
    if (shooterMovingPlat != null && shotProjectile != null)
    {
        //shotProjectile.GetComponent<Projectile>().SpeedSet(shooterMovingPlat.GetComponent<Rigidbody2D>().velocity.x);
        //Debug.Log("Stage 1:");
        //Debug.Log(shotProjectile.GetComponent<Rigidbody2D>().velocity);
        /*
vXMovingPlat = shooterMovingPlat.GetComponent<Rigidbody2D>().velocity.x;
vYMovingPlat = shooterMovingPlat.GetComponent<Rigidbody2D>().velocity.y;

vXShotProjectile = shotProjectile.GetComponent<Rigidbody2D>().velocity.x;
vYShotProjectile = shotProjectile.GetComponent<Rigidbody2D>().velocity.y;
*/

    //StartCoroutine(shotProjectile.GetComponent<Projectile>().Start());

    /*
    shotProjectile.GetComponent<Projectile>().ChangeVelocity(
    new Vector2(
    shotProjectile.GetComponent<Rigidbody2D>().velocity.x + vXMovingPlat,
    shotProjectile.GetComponent<Rigidbody2D>().velocity.y + vYMovingPlat
    )


    );

    // Debug.Log("Stage 2:");
    //Debug.Log(shotProjectile.GetComponent<Rigidbody2D>().velocity);

}
*/
}
