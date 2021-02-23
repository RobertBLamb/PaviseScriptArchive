using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used to make sure that prefabs can be updated when 
//used in other prefabs and level setups.
public class InstantiateAtPoint : MonoBehaviour {

    [SerializeField]
    private GameObject prefabToInstantiate;
    private GameObject instantiatedObj;
    [SerializeField]
    private Transform instantiateParent;
    [SerializeField]
    private Transform instantiatePoint;

    // Use this for initialization
    void Start () {
        //Sprite to help place the obj
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().enabled = false;

        if (instantiatePoint == null)
            instantiatePoint = transform;

        instantiatedObj = Instantiate(prefabToInstantiate, instantiatePoint.position, instantiatePoint.rotation);

        //If parent is specified, inst. it as its child
        if (instantiateParent != null)
            instantiatedObj.transform.parent = instantiateParent;
        Destroy(gameObject);
    }
}
