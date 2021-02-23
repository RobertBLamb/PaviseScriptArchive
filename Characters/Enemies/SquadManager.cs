using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : MonoBehaviour {

    public EnemyController[] members;

	// Use this for initialization
	void Start ()
    {
        /*
        if (members[0] == null)
        {
            foreach ()
        }
        */
	}
	
	public void ActivateMembers()
    {
        foreach (EnemyController member in members)
        {
            member.JumpActivate();
            member.transform.parent = null;
        }

        Destroy(this.gameObject);
	}
}
