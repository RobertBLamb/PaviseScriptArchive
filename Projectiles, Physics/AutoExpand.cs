using UnityEngine;
using System.Collections;

public class AutoExpand : MonoBehaviour {

    public float originalScale;
    public float currentScale;
    public float scaleFactor;
    public float maxScaleFactor = 30;

	// Use this for initialization
	void Start () {
        originalScale = Vector3.Magnitude(transform.localScale);

	}
	
	// Update is called once per frame
	void Update () {
        currentScale = Vector3.Magnitude(transform.localScale);
        scaleFactor = currentScale / originalScale;
        if (scaleFactor <= maxScaleFactor)
            transform.localScale *= 1.25f;
        else
            Object.Destroy(gameObject);
    }
}
