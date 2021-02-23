using UnityEngine;
using System.Collections;

public class fadeOnEnter : MonoBehaviour {
    public bool test;
    public SpriteRenderer thisSprite;
    public float timeBetweenFade;   
    //1 is the highest no matter what
    public float maxOp;
    public float minOp;
    public float modOp;
    public Color tmp;

    // Use this for initialization
    void Start ()
    {
        thisSprite = GetComponent<SpriteRenderer>();
        tmp = thisSprite.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("As intended");
            StopAllCoroutines();
            StartCoroutine(fadeOut());

        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("As intended");
            StopAllCoroutines();
            StartCoroutine(fadeIn());

        }
    }

    IEnumerator fadeOut()
    {
        while (tmp.a>minOp)
        {
            tmp.a -= modOp;
            thisSprite.color = tmp;
            yield return new WaitForSeconds(timeBetweenFade);
        }
    }
    IEnumerator fadeIn()
    {
        while (tmp.a < maxOp)
        {
            tmp.a += modOp;
            thisSprite.color = tmp;
            yield return new WaitForSeconds(timeBetweenFade);

        }
    }
}
