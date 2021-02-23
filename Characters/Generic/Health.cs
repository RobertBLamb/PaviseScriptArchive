using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

    //Allows Inspector-setting the "topmost" GameObject the Health applies to, 
    //allowing the hitbox to be a child of the actual obj
    public GameObject mainGameObj;

    public bool isPlayer;
    public bool breakablePlat;

    public int maxHealth;
    public int health;

    //Multipliers for damage types
    //Set to 0 to make immune
    public float bulletMultiplier = 1;
    public float explMultiplier = 1;
    public float elecMultiplier = 1;

    //Should be structured up when adding different projectile types, but is good for now
    public bool dodgesBullets = false;

    protected bool hasMultiplier;

    public bool ded = false;
    public bool fallen = false;
    
    public bool damageABLE = true;

    public Vector4 flickerColor = Color.red;
    public int flickerCount;
    public bool flickering;

    protected int newHealth;
    public Rigidbody2D myRigidBody;

    public GameObject playerSpriteObject;

    public SpriteRenderer characterRenderer;

    public Color defaultColor;

    // Use this for initialization
    protected virtual void Start () {

        if (mainGameObj == null)
            mainGameObj = gameObject;

        if (isPlayer)
        {
            //playerSpriteObject = GameObject.FindGameObjectWithTag("PlayerTorso");
            characterRenderer = playerSpriteObject.GetComponent<SpriteRenderer>();
        }
        //If SpriteRenderer is not set, ref the MAIN GameObject
        else if (characterRenderer == null)
            characterRenderer = mainGameObj.GetComponent<SpriteRenderer>();

        defaultColor = characterRenderer.color;

        if (bulletMultiplier != 1 || explMultiplier != 1 || elecMultiplier != 1)
            hasMultiplier = true;
        
        if (myRigidBody == null)
            myRigidBody = mainGameObj.GetComponent<Rigidbody2D>();
        newHealth = health;
        flickering = false;
	}

    //Projectiles should call this to check for resistances and weaknesses
    public void HitCheck(int dmg, GameObject projectile)
    {
        string projType;
        float dmgF = (float)dmg;

        if (hasMultiplier)
        {
            projType = projectile.tag;

            if (projType == "Bullet" && bulletMultiplier != 1)
            {
                dmgF *= bulletMultiplier;
            }

            if (projType == "Explosive" && explMultiplier != 1)
            {
                dmgF *= explMultiplier;
            }

            if (projType == "Electric" && elecMultiplier != 1)
            {
                dmgF *= elecMultiplier;
            }
        }

        //Set flicker color based on if damage was resisted
        if ((int)dmgF == 0)
        {
            //White normal color with 80% opacity
            flickerColor = new Vector4(.5F, .5F, .5F, .8F);
            flickerCount = 2;
        }
        else if (dmgF < (float)dmg)
        {
            flickerColor = Color.yellow;
        }
        //else, flickercolor should remain as the one set in Inspector
        /*
        else
        {
            flickerColor = new Vector4(.5F, 0, 0, 1F);
        }
        */
        //CeilToInt always rounds up, and converts to int
        //Converts back to int to have even damage numbers, i.e. 33.3 -> 34

        Damage(Mathf.CeilToInt (dmgF));

    }

    //Temp, just for bullets for now
    public bool DodgeCheck(string projTag)
    {
        if (projTag == "Bullet" && dodgesBullets)
        {
            flickerColor = new Vector4(.5F, .5F, .5F, .8F);
            flickerCount = 2;
            StartCoroutine(flickerEnemy());
            return (true);
        }
        else
            return (false);
    }

    public virtual void Damage(int dmg) {
        //if (damageABLE == true)
        {
            int tempHealth;
            tempHealth = health;
            health -= dmg;

            CheckIfDead();

            if (isPlayer)
                StartCoroutine(flicker());
            else
                StartCoroutine(flickerEnemy());
        }
    }
    //Should upgrade with parameters for color and bool for if invincibility should be on
    protected IEnumerator flicker() {
        flickering = true;
        if (isPlayer)
        {
            damageABLE = false;
        }
        for (int i = 0; i < flickerCount; i++)
        {
            characterRenderer.material.color = defaultColor;
            yield return new WaitForSeconds(.05F);
            characterRenderer.material.color = new Vector4(.5F, .5F, .5F, .5F);
            yield return new WaitForSeconds(.05F);
        }
        characterRenderer.material.color = defaultColor;

        flickering = false;
        damageABLE = true;
    }

    protected IEnumerator flickerEnemy()
    {
        flickering = true;
        for (int i = 0; i < flickerCount; i++)
        {
            characterRenderer.material.color = defaultColor;
            yield return new WaitForSeconds(.05F);
            characterRenderer.material.color = flickerColor;
            //* new Vector4(1F, 0, 0, .5F);
            yield return new WaitForSeconds(.05F);
        }
        characterRenderer.material.color = defaultColor;

        flickering = false;
    }

    void CheckIfDead()
    {
        //Only checks for death on being damaged
        if (breakablePlat && health <= 0)
        {
            Destroy(gameObject);
        }
        if (health <= 0 && !ded && !fallen)
        {
            ded = true;

            if (GetComponent<spawnOnKill>() != null)
                GetComponent<spawnOnKill>().spawnAll();
            fall();
        }
    }

    protected void fall()
    {
        foreach(Transform child in mainGameObj.transform)
            Destroy(child.gameObject);
        mainGameObj.transform.DetachChildren();
        mainGameObj.transform.parent = null;

        if (mainGameObj.GetComponent<Collider2D>() != null)
            mainGameObj.GetComponent<Collider2D>().enabled = false;

        if (mainGameObj.GetComponent<SpriteRenderer>() != null)
            mainGameObj.GetComponent<SpriteRenderer>().enabled = false;

        if (mainGameObj.GetComponent<HitPoses>() != null)
        {
            mainGameObj.GetComponent<HitPoses>().enabled = false;
        }

        if (mainGameObj.GetComponent<ChaserEnemy>() != null)
        {
            mainGameObj.GetComponent<ChaserEnemy>().enabled = false;
        }
        //8-17R added to make drone stop following after taking enough damage
        else if(mainGameObj.GetComponent<StalkerDrone>() != null)
        {
            mainGameObj.GetComponent<StalkerDrone>().enabled = false;
        }

        //Might want to change this to reference the tag of THIS obj (the hitbox?)
        if (mainGameObj.CompareTag("Enemy") || mainGameObj.CompareTag("Enemy1"))
        {
            if (mainGameObj.GetComponent<Collider2D>())
                mainGameObj.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(delete());
            fallen = true;
        }

        //Destroy(gameObject);
    }
    IEnumerator delete()
    {
        yield return new WaitForSeconds(3);
        GameObject.Destroy(mainGameObj);
    }

}
