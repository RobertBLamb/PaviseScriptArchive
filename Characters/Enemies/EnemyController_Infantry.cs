using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Infantry : EnemyController
{

    private void FixedUpdate()
    {
        if (state == EnemyState.trans)
        {
            GroundCheck();
        }
    }

    override public void JumpActivate()
    {
        if (state == EnemyState.idle)
        {
            currentCoroutine = StartCoroutine(JumpDown());
        }
	}

    IEnumerator JumpDown()
    {
        state = EnemyState.trans;
        SwitchStateObject(transState);

        //TEMP
        rb.velocity = Vector2.up * 3;
        yield return new WaitForSeconds(Random.Range(.1f, .3f));
        rb.velocity = Vector2.down * 10;
    }

    /*
    override public void Activate() 
    {
        state = EnemyState.active;
        rb.velocity = Vector2.zero;
        SwitchStateObject(activeState);
    }
    */

    void OnTriggerEnter2D(Collider2D col)
    {
        print(col.gameObject.tag);
        if (state == EnemyState.trans)
        {
            if (col.gameObject.tag == "Ground")
            {
                if (currentCoroutine != null)
                    StopCoroutine(currentCoroutine);
                currentCoroutine = StartCoroutine(Activate());
            }
        }
    }

    //Raycast for Ground instead? Can save a bit of memory by not calling OnCollisionEnter2D all the time
    bool GroundCheck()
    {
        //if ()
        return true;
    }
}
