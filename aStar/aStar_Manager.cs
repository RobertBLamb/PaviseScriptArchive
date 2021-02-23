using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aStar_Manager : MonoBehaviour {

    public static aStar_Manager S;
    public GameObject aStarPrefab;
    public GameObject active_aStarObj;

	// Use this for initialization
	void Start () {
        S = this;
	}

    public void ReplaceGrid(Transform newGridTrans)
    {
        Destroy(active_aStarObj);
        active_aStarObj = (Instantiate(aStarPrefab, newGridTrans.position, aStarPrefab.transform.rotation) as GameObject);
    }
}

