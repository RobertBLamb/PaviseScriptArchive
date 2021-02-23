using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public Vector2 velocity;
    public float smoothTimeX;
    public float smoothTimeY;
    public GameObject player;
    public float shakeTimer;
    public float shakeAmount;
    public Vector2 shakePos;
    public bool activeCR;
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        //transform.position = new Vector3(posX, posY, transform.position.z);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !activeCR)
        {
            StartCoroutine(screenShakeCaller(shakeTimer, shakeAmount));
        }
    }
    public IEnumerator screenShakeCaller(float timer, float power)
    {

        activeCR = true;
        //Debug.Log("howdy");
        StartCoroutine(screenShake(timer, power));
        yield return new WaitForEndOfFrame();

    }
    public IEnumerator screenShake(float timer, float power)
    {
        //Debug.Log("hi there");
        while(timer>0)
        {
            shakePos = Random.insideUnitCircle * shakeAmount;
            transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y + shakePos.y, transform.position.z);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        activeCR = false;
    }
}