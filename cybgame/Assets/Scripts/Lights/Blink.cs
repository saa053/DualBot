using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] float blinkSpeed;
    [SerializeField] float aplhaIncrement;
    [SerializeField] float maxAlpha;
    Material material;
    bool isFadingOut;
    bool isFadingIn;

    void Start()
    {
        isFadingOut = false;
        isFadingIn = false;
        material = GetComponent<MeshRenderer>().material;
        
        SetAplhaZero();
    }

    void Update()
    {
        if (material.color.a == 0f && !isFadingIn)
            StartCoroutine(FadeIn());
        else if (material.color.a == maxAlpha && !isFadingOut)
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        isFadingOut = true;
        float newAlpha = maxAlpha;

        while (material.color.a > 0)
        {
            Color newColor = material.color;
            newColor.a = newAlpha;

            material.color = newColor;

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

        while (material.color.a < maxAlpha)
        {
            Color newColor = material.color;
            newColor.a = newAlpha;

            material.color = newColor;

            newAlpha += aplhaIncrement;

            yield return new WaitForSeconds(blinkSpeed);
        }
        
        Color color = material.color;
        color.a = maxAlpha;
        material.color = color;

        isFadingIn = false;
    }

    public void SetAplhaZero()
    {
        Color color = material.color;
        color.a = 0f;
        material.color = color;
    }
}
