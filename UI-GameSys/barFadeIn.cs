using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class barFadeIn : MonoBehaviour
{
    public Text text;
    public Image border;
    public Image fill;
    public Image mask;

    public float opacityTarget = 1;

    public bool fadeInOnPlay = true;

    Color borderColor;
    Color fillColor;
    Color maskColor;
    Color textColor;

    // Use this for initialization
    void Start()
    {
        borderColor = border.color;
        borderColor.a = 0;
        border.color = borderColor;

        fillColor = fill.color;
        fillColor.a = 0;
        fill.color = fillColor;

        maskColor = mask.color;
        maskColor.a = 0;
        mask.color = maskColor;

        if (text != null)
        {
            textColor = text.color;
            textColor.a = 0;
            text.color = textColor;
        }

        if (fadeInOnPlay)
            StartCoroutine(increaseOpacity());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            StartCoroutine(increaseOpacity());
        }
        if (Input.GetKeyDown("y"))
        {
            StartCoroutine(decreaseOpacity());
        }
    }

    IEnumerator increaseOpacity()
    {

        while (fill.color.a < opacityTarget)
        {
            borderColor.a += .02f;
            fillColor.a += .01f;
            maskColor.a += .01f;
            if (text != null)
                textColor.a += .01f;
            border.color = borderColor;
            if (text != null)
                text.color = textColor;
            fill.color = fillColor;
            mask.color = maskColor;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator decreaseOpacity()
    {
        while (fill.color.a > 0)
        {
            borderColor.a -= .02f;
            fillColor.a -= .01f;
            maskColor.a -= .01f;
            if (text != null)
                textColor.a -= .01f;
            border.color = borderColor;
            if (text != null)
                text.color = textColor;
            fill.color = fillColor;
            mask.color = maskColor;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
}
