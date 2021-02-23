using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHealth : Health {

    //Should be around 3, if explosives do 2 dmg and bullets do 1.
    public int healthPerBlock;

    [System.Serializable]
    public struct healthBlock
    {
        //public int maxHealth;
        public int health;
        //public Sprite barSprite;
        public int index;
    };

    //public int numHealthBlocks;

    public healthBlock[] healthBlocks;
    //public Sprite[] healthBlockSprites;
    public BarDisplay[] healthBlockDisplays;
    public BarDisplay currentBlockDisplay;


    //The healthblock currently open to damage/healing
    [SerializeField]
    public healthBlock currentHealthBlock;

    public Coroutine regenDelay;
    public Coroutine regen;

    // Use this for initialization
    protected override void Start () {

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

        //Health Block Exclusive

        //healthBlocks = new healthBlock[numHealthBlocks];

        //Fill all health blocks
        for (int i = 0; i < healthBlocks.Length; i++)
        {
            healthBlocks[i].health = healthPerBlock;
            //healthBlocks[i].barSprite = healthBlockSprites[i];
            healthBlocks[i].index = i;
        }

        foreach (BarDisplay blockDisplay in healthBlockDisplays)
        {
            //Initialize bar value then disable
            blockDisplay.maxValue = healthPerBlock;
            blockDisplay.value = healthPerBlock;
            blockDisplay.enabled = false;
        }

        // Blocks go from LAST to FIRST (0)
        currentHealthBlock = healthBlocks[healthBlocks.Length - 1];
        currentBlockDisplay = healthBlockDisplays[currentHealthBlock.index];
        currentBlockDisplay.enabled = true;
        
    }

    //Need to rewrite for block system
    public override void Damage(int dmg)
    {
        bool switchedBlocks = false;

        if (currentHealthBlock.health <= 0)
        {
            //Testing: Check this at the beginning of the frame so that BarDisplay can update after?
            //Iterate down to next undamaged block
            currentBlockDisplay.enabled = false;
            currentHealthBlock = healthBlocks[currentHealthBlock.index - 1];
            currentBlockDisplay = healthBlockDisplays[currentHealthBlock.index];
            Debug.Log(currentBlockDisplay.name);
            currentBlockDisplay.enabled = true;

            switchedBlocks = true;

            if (currentHealthBlock.index != 0)
            {
                if (isPlayer)
                    StartCoroutine(flicker());
                else
                    StartCoroutine(flickerEnemy());

            }
        }

        //if (damageABLE == true)
        //int tempHealth;
        //tempHealth = currentHealthBlock.health;

        //Important: the annoying UI block bug (not all health blocks update) seems to be due to execution order, be sure to check that if it goes wrong.
        //Prevent damaging again on the same frame as a block switch
        if (!switchedBlocks)
            currentHealthBlock.health -= dmg;

        if (currentHealthBlock.health <= 0)
        {
            Debug.LogWarning("SwitchingBlocks");

            currentHealthBlock.health = 0;

            if (currentHealthBlock.index == 0)
            {
                CheckIfDead();
                //Check to make work with existing "death" functions
                //ded = true;
                //fall();
                Destroy(gameObject);
            }
            
        }

            //Needs a "damaged" flicker in addition to the "mercy I-frames" flicker above
            /*
            if (isPlayer)
                StartCoroutine(flicker());
            else
                StartCoroutine(flickerEnemy());
            */
            //Cancel the wait countdown for regenning health block if damaged
            if (regenDelay != null)
            {
                StopCoroutine(regenDelay);
            }
            if (regen != null)
            {
                StopCoroutine(regen);
            }

            regenDelay = StartCoroutine(DelayRegen());
    }

    public void InstaKill()
    {
        currentHealthBlock = healthBlocks[0];
        currentHealthBlock.health = 0;
        CheckIfDead();
    }

    //TEMP JUST TO MAKE PLAYER DIE FOR TESTING
    void CheckIfDead()
    {
        //if (currentHealthBlock.)
        Destroy(gameObject);

    }

    //
    IEnumerator DelayRegen()
    {
        int stableHealth = currentHealthBlock.health;
        yield return new WaitForSeconds(3f);
        //If was not damaged for the regen delay time
        if (stableHealth == currentHealthBlock.health)
            regen = StartCoroutine(Regen());

    }

    //Regen the health block after a certain time without damage
    //Need to cap max health and cancel if damaged
    IEnumerator Regen()
    {
        while (currentHealthBlock.health < healthPerBlock)
        {
            //Need to base wait time on either the system time or a calculated interval
            yield return new WaitForSeconds(.1f);
            //Might also need to tweak this to make the regen time reasonable
            currentHealthBlock.health++;
        }

        if (currentHealthBlock.health >= healthPerBlock)
            currentHealthBlock.health = healthPerBlock;

    }

}
