
using UnityEngine;
using System.Collections;

public class AttackSighted : MonoBehaviour {

    public GameObject sightRadiusObj;
    public GameObject arm;
    public GameObject trailObj;
    public GameObject muzzleObj;

    //public Collider2D sightRadius;

    public GameObject currentTarget;
    public float targetDelay = .8f;
    public Sprite idleSprite;
    public Sprite firingSprite;

    // Use this for initialization
    void Start()
    {
        idleSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    void Update()
    {

        {
            if (sightRadiusObj.GetComponent<SightRadius>().currentSighted != null && currentTarget == null)
            {
                currentTarget = sightRadiusObj.GetComponent<SightRadius>().currentSighted;
                StartCoroutine("Fire");
            }

            /*
            if (sightRadiusObj.GetComponent<SightRadius>().doneSighting == true)
            {
                StartCoroutine("AttackAll");
            }
            */
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
    if (currentTarget != null)
        {
            var pos = arm.transform.position;
            var dir = pos - (
                currentTarget
                .transform.position);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
    IEnumerator AttackAll()
    {
        for (int i = 0; i < sightRadiusObj.GetComponent<SightRadius>().sightedObjects.Count; i++)
        {
            while (sightRadiusObj.GetComponent<SightRadius>().sightedObjects[i] == null)
            {
                i++;
            }
                currentTarget = sightRadiusObj.GetComponent<SightRadius>().sightedObjects[i];
            yield return new WaitForSeconds(targetDelay);
            StartCoroutine("Fire");
        }

        sightRadiusObj.GetComponent<SightRadius>().sightedObjects.Clear();
        sightRadiusObj.GetComponent<SightRadius>().doneSighting = false;
    }
    IEnumerator Fire()
    {
        yield return new WaitForSeconds(targetDelay);

        if (firingSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = firingSprite;
            yield return new WaitForSeconds(.05f);
            GetComponent<SpriteRenderer>().sprite = idleSprite;
        }

        if (trailObj != null)
        {
            trailObj.GetComponent<LineRenderer>().enabled = true;

            trailObj.GetComponent<LineRenderer>().SetPosition(0, muzzleObj.transform.position);

            if (currentTarget != null)
                trailObj.GetComponent<LineRenderer>().SetPosition(1, currentTarget.transform.position);
        }
        if (currentTarget != null)
        {

            if (currentTarget.GetComponent<Projectile>().explosion != null)
            {
                StartCoroutine(currentTarget.GetComponent<Projectile>().Disable());
                currentTarget = null;
            }

            else
                Destroy(currentTarget);
            currentTarget = null;

            if (firingSprite != null)
            {
                //GetComponent<SpriteRenderer>().sprite = idleSprite;
            }
        }
        if (trailObj != null)
        {
            trailObj.SetActive(true);
            yield return new WaitForSeconds(.5f);
            trailObj.GetComponent<LineRenderer>().enabled = false;
        }
        currentTarget = null;

    }
}
