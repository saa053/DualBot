using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    [SerializeField] float blinkSpeed;
    [SerializeField] float aplhaIncrement;
    [SerializeField] float maxAlpha;
    TextMeshProUGUI text;
    bool isFadingOut;
    bool isFadingIn;

    void Start()
    {
        isFadingOut = false;
        isFadingIn = false;

        text = GetComponent<TextMeshProUGUI>();
        
        SetAplhaZero();
    }

    void Update()
    {
        if (text.color.a == 0f && !isFadingIn)
            StartCoroutine(FadeIn());
        else if (text.color.a == maxAlpha && !isFadingOut)
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        isFadingOut = true;
        float newAlpha = maxAlpha;

        while (text.color.a > 0)
        {
            Color newColor = text.color;
            newColor.a = newAlpha;

            text.color = newColor;

            newAlpha -= aplhaIncrement;

            yield return new WaitForSeconds(blinkSpeed);
        }
        
        SetAplhaZero();

        isFadingOut = false;
    }

    IEnumerator FadeIn()
    {
        isFadingIn = true;
        float newAlpha = 0f;

        while (text.color.a < maxAlpha)
        {
            Color newColor = text.color;
            newColor.a = newAlpha;

            text.color = newColor;

            newAlpha += aplhaIncrement;

            yield return new WaitForSeconds(blinkSpeed);
        }
        
        Color color = text.color;
        color.a = maxAlpha;
        text.color = color;

        isFadingIn = false;
    }

    public void SetAplhaZero()
    {
        Color color = text.color;
        color.a = 0f;
        text.color = color;
    }
}
