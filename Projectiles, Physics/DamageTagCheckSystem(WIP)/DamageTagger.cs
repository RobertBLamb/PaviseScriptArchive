using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTagger : MonoBehaviour {

    public List<DamageTag> tagsList;
    
    //Important: put this prefab obj on a layer that mostly only interacts with objs with Health
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Health>() != null)
            AddDmgTagToObj(col.gameObject);
    }

    void AddDmgTagToObj(GameObject objToTag)
    {
       // DamageTag tagToInitialize = 
       // tagsList.Add(objToTag.AddComponent<DamageTag>());
    }

    void OnDestroy()
    {

    }    
}
