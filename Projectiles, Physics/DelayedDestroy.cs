using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour {
    public float destroyTime;

    // Use this for initialization
    void Start () {
        Object.Destroy(gameObject, destroyTime);

    }
}
