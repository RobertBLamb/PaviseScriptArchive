using UnityEngine;
using System.Collections;

public class GenericLookAtObject : MonoBehaviour
{

    public GameObject targetObj;
    public float angleEdit;

    // Use this for initialization
    void Start()
    {
        if (targetObj == null)
            targetObj = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        var pos = transform.position;
        var dir = pos - (targetObj.transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle + angleEdit, Vector3.forward);

    }
}
