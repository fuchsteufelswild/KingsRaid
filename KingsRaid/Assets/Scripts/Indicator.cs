using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private UnityEngine.UI.Image mImage;
    public float fadeOutTime = 0.3f;
    public float fadeInTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        mImage = this.GetComponent<UnityEngine.UI.Image>();
        mImage.color = new Color(mImage.color.r, mImage.color.g, mImage.color.b, 0f);
    }

    public void Blink()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        for (float t = 1f; t > 0f; t -= Time.deltaTime / fadeOutTime)
        {
            mImage.color = new Color(mImage.color.r, mImage.color.g, mImage.color.b, t);

            yield return null;
        }

    }

    IEnumerator FadeIn()
    {
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadeOutTime)
        {
            mImage.color = new Color(mImage.color.r, mImage.color.g, mImage.color.b, t);

            yield return null;
        }

        StartCoroutine(FadeOut());
    }
}
