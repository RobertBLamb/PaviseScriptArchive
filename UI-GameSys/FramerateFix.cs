using UnityEngine;
using System.Collections;

public class FramerateFix : MonoBehaviour
{

    public int targetFps;

    // Use this for initialization
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetFps != Application.targetFrameRate)
        {
            Application.targetFrameRate = targetFps;
        }
    }
}
