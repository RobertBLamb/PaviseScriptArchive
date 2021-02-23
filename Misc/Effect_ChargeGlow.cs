using UnityEngine;
using System.Collections;

public class Effect_ChargeGlow : MonoBehaviour {

    public SpriteRenderer chargeSpriteRend;
    public Vector3 defaultScale;
    public Vector4 defaultColor;

    public GameObject arm;

    //MAKE GENERIC
    //MAKE SCALE FACTOR ADAPT TO CHARGE TIME
    //Charge effect should apply to laser sight LineRenderer as well

	// Use this for initialization
	void Start () {
        chargeSpriteRend = GetComponent<SpriteRenderer>();
        defaultScale = transform.localScale;
        defaultColor = chargeSpriteRend.color;
        //Temp Workaround for default alpha
        defaultColor = new Vector4(chargeSpriteRend.color.r, chargeSpriteRend.color.g, chargeSpriteRend.color.b, .1f);
    }

    // Update is called once per frame
    void Update()
    {
        //Specific to HitscanEnemyArm
        if (arm.GetComponent<HitscanEnemyArm>().charging)
        {
            transform.localScale *= .99f;
            chargeSpriteRend.color = new Vector4(chargeSpriteRend.color.r, chargeSpriteRend.color.g, chargeSpriteRend.color.b, chargeSpriteRend.color.a * 1.05f);
        }
        else
        {
            transform.localScale = defaultScale;
            chargeSpriteRend.color = defaultColor;
        }
    }
}
