using UnityEngine;
using System.Collections;

public class ArmControl3 : MonoBehaviour
{
    GameObject player;
    //PlayerAnimationController playerAnimController;

    //1-15-18W : Replacing 2nd ver animation system with 3rd ver
    //Need safeguard FindByTags
    public GameObject playerTorso;
    public GameObject playerRig;
    Animator playerAnimator;
    
    private GameObject playerArmRig;

    //public GameObject playerHead;

    public GameObject ReflectBox;
    public Transform projectileSpawn;
    public GameObject projectile1;

    public float currentAngle;

    public Vector3 rot;
    public Vector3 localRot;

    public bool tapped;
    public bool holding;


    // Use this for initialization
    void Start()
    {
        ReflectBox.GetComponent<PolygonCollider2D>().enabled = false;

        playerArmRig = GameObject.FindGameObjectWithTag("PlayerArmRig");

        player = GameObject.FindGameObjectWithTag("Player");
        //////REMEMBER TO CHANGE UP TAGS SO THAT THE TORSO IS ACTUALLY TAGGED AS SUCH//////
        if (playerRig == null)
            playerRig = GameObject.FindGameObjectWithTag("PlayerTorso");
        //playerAnimController = player.GetComponent<PlayerAnimationController>();
        playerAnimator = playerRig.GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        //GetComponent<PolygonCollider2D>().enabled = false;
    }

    void FixedUpdate()
    {
        var pos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - pos;

        if (player.GetComponent<PlayerController>().control && !player.GetComponent<PlayerSpecialMoves>().superDeflecting)
        {
            currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
            //playerHead.transform.rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
        }

        if (Input.GetMouseButtonDown(0) && player.GetComponent<PlayerController>().control)
        {
            StartCoroutine(ReflectActive());
        }
        //Debug.DrawRay(transform.position, transform.rotation * -Vector2.left * 10, Color.green, 4F);

        else if (Input.GetMouseButton(0) && player.GetComponent<PlayerController>().control)
        {
            holding = true;
        }

        if (Input.GetMouseButtonUp(0) || player.GetComponent<PlayerSpecialMoves>().charging)
        {
            ReflectBox.GetComponent<PolygonCollider2D>().enabled = false;
            holding = false;
        }

        rot = transform.rotation.eulerAngles;
        localRot = transform.localRotation.eulerAngles;

        ////                                                                                    ////
        ///-------------SET UP SEPERATE LAYER AND STATES FOR TORSO, ARMS AND LEGS!!!!!--------///
        ////                                                                                    ////

        bool armWasSet = false;
        if (rot.z > 90F && rot.z < 270F)
        {
            if (playerRig.activeSelf && playerAnimator.enabled)
                //playerAnimController.changeSharedAnimParameter("FacingBack", true);
                playerAnimator.SetBool("FacingBack", true);
            //GetComponent<SpriteRenderer>().flipY = true;
            playerArmRig.transform.localScale = new Vector3(playerArmRig.transform.localScale.x, -1, 1);
            playerTorso.transform.rotation = Quaternion.AngleAxis((rot.z * .2f - 75), Vector3.forward);

            armWasSet = true;
        }
        else if (!armWasSet)
        {
            //---- Facing Front ----//
            if (playerRig.activeSelf && playerAnimator.enabled)
                playerAnimator.SetBool("FacingBack", false);
            //GetComponent<SpriteRenderer>().flipY = false;
            playerArmRig.transform.localScale = new Vector3(playerArmRig.transform.localScale.x, 1, 1);

            // 0-50
            if (rot.z <= 50 && rot.z >= 0)
            {
                playerTorso.transform.rotation =
                    //Quaternion.AngleAxis(rot.z - 10 * (rot.z / 90), Vector3.forward);
                    Quaternion.AngleAxis(((rot.z * .8f) - 40), Vector3.forward);

            }
            // 50-90
            else if (rot.z > 50 && rot.z <= 90)
            {
                playerTorso.transform.rotation =
                   Quaternion.AngleAxis(0, Vector3.forward);
            }
            // 340-360
            else if (rot.z < 360 && rot.z >= 340)
            {
                playerTorso.transform.rotation =
                    //Quaternion.AngleAxis(rot.z + 10 * (rot.z / 90), Vector3.forward);
                    Quaternion.AngleAxis((rot.z - 40), Vector3.forward);
            }
            else
                // 270-340
                playerTorso.transform.rotation =
                   Quaternion.AngleAxis(300, Vector3.forward);
        }

        //Allow mouse input after rotation calculations
        if (Input.GetMouseButtonDown(0))
            Shoot();

    }

    void Shoot()
    {
        Instantiate(projectile1, projectileSpawn.position, transform.rotation);
    }

    IEnumerator ReflectActive()
    {
        ReflectBox.GetComponent<PolygonCollider2D>().enabled = true;
        tapped = true;
        yield return new WaitForSeconds(0.07F);
        tapped = false;
    }
}