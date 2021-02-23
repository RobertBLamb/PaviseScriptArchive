using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT IS ON THE RIG OBJECT, TO GET ANIMATED ON
public class PlayerAnimationController : MonoBehaviour {

    //These are for tracking that the sprites HAVE been shown
    private bool
        showingFrontTorso,
        showingBackTorso;

    //This is a "switch" trigger to activate TO show the sprites once, so thhe show function doesn't run every frame.

    public bool showBackTorso;

    //All the sprite renderers that should show if Svel's torso is facing the viewer
    public SpriteRenderer[] frontTorsoSprites;
    //Same, but for when her back is turned to the viewer
    public SpriteRenderer[] backTorsoSprites;

    private void Start()
    {
        ShowFrontTorso();
        
    }

    //see if we can do this without an update?
    private void Update()
    {
        if (!showBackTorso)
        {
            if (showingBackTorso)
                ShowFrontTorso();
        }
        else if (showBackTorso)
        {
            if (showingFrontTorso)
                ShowBackTorso();
        }
    }

    public void ShowFrontTorso()
    {
        foreach (SpriteRenderer sprite in backTorsoSprites)
        {
            sprite.enabled = false;
        }

        foreach (SpriteRenderer sprite in frontTorsoSprites)
        {
            sprite.enabled = true;
        }

        showingFrontTorso = true;
    }

    public void ShowBackTorso()
    {
        foreach (SpriteRenderer sprite in frontTorsoSprites)
        {
            sprite.enabled = false;
        }

        foreach (SpriteRenderer sprite in backTorsoSprites)
        {
            sprite.enabled = true;
        }

        showingBackTorso = true;
    }
}
