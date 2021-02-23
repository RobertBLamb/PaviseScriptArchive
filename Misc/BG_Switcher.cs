using UnityEngine;
using System.Collections;

public class BG_Switcher : MonoBehaviour {

    public Sprite[] BGs;
    public int currentBGNum = 0;
    public int defaultBGNum = 0;

    private SpriteRenderer myRenderer;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<SpriteRenderer>();
        myRenderer.sprite = BGs[defaultBGNum];
        currentBGNum = defaultBGNum;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown("f"))
        {
            currentBGNum++;
            if (currentBGNum >= BGs.Length)
            {
                currentBGNum = 0;
            }
            myRenderer.sprite = BGs[currentBGNum];
        }
    }
}
