using UnityEngine;
using System.Collections;

public class fallPlatS : MonoBehaviour
{
    private Rigidbody2D plat2D;
    private GameObject fallingPlatform;

    private bool destroying;
    public float countdownToDestroy;
    public float totalToDestroy;
    // Use this for initialization
    void Start()
    {
        plat2D = GetComponent<Rigidbody2D>();
        fallingPlatform = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (destroying)
        {
            //plat2D.GetComponent<movingPlat>().enabled = true;
            fallingPlatform.GetComponent<movingPlat>().enabled = true;
            countdownToDestroy += Time.deltaTime;
            if (countdownToDestroy >= totalToDestroy)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            destroying = true;
        }
    }
}
