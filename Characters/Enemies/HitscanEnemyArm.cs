using UnityEngine;
using System.Collections;

public class HitscanEnemyArm : MonoBehaviour
{
    public Transform projectileSpawn;
    //velocity of Projectile

    public GameObject player;
    //Target a set distance from player

    int ShieldBlockLayer;
    int ShieldBlockMask;

    private Sprite defaultSprite;
    //Firing "animation"
    public Sprite[] firingPoses;
    //Charge visual effect
    public bool charging;
    /*
    //Moved to separate script
    public GameObject chargeObject;
    public Sprite chargeSprite;
    */

    public float readyDelay = 1F;
    public float shootDelay = .8F;
    public float reloadTimeR = 2;
    public int magSize = 1;

    public bool leadTarget;
    public Vector3 targetEdit;
    public Vector3 currentTargetEdit;
    /*
    public float vXEditRatio = .3F;
    public float vYEditRatio = .1F;
    public float angleEdit;
    public float debugAngle1;
    public float debugAngle2;
    */
    public bool limited;
    public float lowerAngleLim = 0;
    public float upperAngleLim = 360;
    public float debugAngle1;


    // Mask from Shield Layer


    // Use this for initialization
    IEnumerator Start()
    {
        defaultSprite = GetComponent<SpriteRenderer>().sprite;
        player = GameObject.FindGameObjectWithTag("Player");

        ShieldBlockLayer = 21;

        ShieldBlockMask = 1 << ShieldBlockLayer;

        yield return new WaitForSeconds(readyDelay);
        while (0 == 0)
        {

            for (int i = 0; i < magSize; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(projectileSpawn.transform.position, transform.rotation * Vector2.left, 100, ShieldBlockMask);
                Debug.DrawRay(projectileSpawn.transform.position, transform.rotation * Vector2.left * 100, new Vector4 (0,1,1,.4f), reloadTimeR);
                //RaycastHit2D
                //Instantiate(projectile, projectileSpawn.position, transform.rotation);
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
                charging = false;

                GetComponent<SpriteRenderer>().sprite = defaultSprite;

                yield return new WaitForSeconds(shootDelay);
            }
            {
                //Temporary specific setup for RailSniper

                charging = true;
                yield return new WaitForSeconds(reloadTimeR);
            }

        }
    }
        void FixedUpdate()
    {
            var pos = transform.position;
            var dir = pos - (player.transform.position + currentTargetEdit);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            debugAngle1 = angle;

            if (limited)
            {
                if (angle > upperAngleLim)
                    angle = upperAngleLim;
                if (angle < lowerAngleLim)
                    angle = lowerAngleLim;
            }
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }