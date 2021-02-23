using UnityEngine;
using System.Collections;

//Base Enemy class, mainly for switching between idle to active.
//Enemy types have different EnemyControllers for different scripted ways of activating
public class EnemyController : MonoBehaviour {

    // public GameObject arm;
    public GameObject currentStateObj;

    public GameObject idleState;
    public GameObject transState;
    public GameObject activeState;

    public enum EnemyState
    {
        idle,
        trans,
        active
    }

    public EnemyState state;
    protected Rigidbody2D rb;

    //Important if we need to cancel the Coroutine/EnemyState transition
    public Coroutine currentCoroutine;


    void Start()
    {
        state = EnemyState.idle;
        rb = GetComponent<Rigidbody2D>();
    }

    protected void SwitchStateObject(GameObject targetState)
    {
        currentStateObj.SetActive(false);
        currentStateObj = targetState;
        currentStateObj.SetActive(true);
    }

    public virtual IEnumerator Activate()
    {
        state = EnemyState.active;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(Random.Range(.1f, .3f));
        SwitchStateObject(activeState);
    }

    public virtual void JumpActivate()
    {
        if (state == EnemyState.idle)
        {
            //currentCoroutine = StartCoroutine(JumpDown());
        }
    }


}
