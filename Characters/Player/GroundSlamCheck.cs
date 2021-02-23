using UnityEngine;
using System.Collections;

public class GroundSlamCheck : MonoBehaviour {

    public bool shieldTouch;

    void OnTriggerStay2D(Collider2D plat)
    {
        if (plat.tag == "Wall"||plat.tag =="Platform"||plat.tag =="Explosive")
        {
             shieldTouch = true;
        }
    }
    void OnTriggerExit2D(Collider2D plat)
    {
        if (plat.tag == "Wall" || plat.tag == "Platform" || plat.tag == "Explosive")
        {
            shieldTouch = false;
        }
    }

}
