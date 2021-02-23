using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExoBoneManager : MonoBehaviour {

    //NOTE: enum "ShieldColors" is defined on ArmPoses script, but defined here. It's not used on that script as of 05/31/18, yet.

    public SpriteRenderer[] exoBoneSprites;
    //Set with default green
    public Color targetColor = new Vector4 (105/255, 245/255, 0, 1);


	// Update is called once per frame
	void Update () {
        foreach (SpriteRenderer sprite in exoBoneSprites)
        {
            sprite.color = targetColor;
        }
	}

    public void testUpdateWithInt(int i)
    {

    }
}
