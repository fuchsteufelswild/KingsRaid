using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public float fadeOutTime = 1f;
    public float fadeOutTimePassed = 0.0f;

    public Image mImage;

    public Image[] additionalImages;
    public Text[] additionalTexts;

    public float mImageMaxAlpha;
    public Indicator indicator;

    private void Start()
    {
        mImageMaxAlpha = mImage.color.a;

        ZeroOut();
    }

    public void FadeO(float fTime)
    {
        StartCoroutine(Fade(fTime));
    }

    public void Refresh()
    {
        mImage.color = new Color(mImage.color.r, mImage.color.g, mImage.color.b, 0.5f);

        for (int i = 0; i < additionalImages.Length; ++i)
            additionalImages[i].color = new Color(additionalImages[i].color.r, additionalImages[i].color.g, additionalImages[i].color.b, 0.5f);

        for (int i = 0; i < additionalTexts.Length; ++i)
            additionalTexts[i].color = new Color(additionalTexts[i].color.r, additionalTexts[i].color.g, additionalTexts[i].color.b, 1f);
    }

    public void ZeroOut()
    {
        mImage.color = new Color(mImage.color.r, mImage.color.g, mImage.color.b, 0f);

        for (int i = 0; i < additionalImages.Length; ++i)
            additionalImages[i].color = new Color(additionalImages[i].color.r, additionalImages[i].color.g, additionalImages[i].color.b, 0f);

        for (int i = 0; i < additionalTexts.Length; ++i)
            additionalTexts[i].color = new Color(additionalTexts[i].color.r, additionalTexts[i].color.g, additionalTexts[i].color.b, 0f);
    }

    IEnumerator Fade(float time)
    {
        for(float t = 0.5f; t > 0f; t -= Time.deltaTime / time)
        {
            mImage.color = new Color(mImage.color.r, mImage.color.g, mImage.color.b, t);

            for (int i = 0; i < additionalImages.Length; ++i)
                additionalImages[i].color = new Color(additionalImages[i].color.r, additionalImages[i].color.g, additionalImages[i].color.b, t);

            for (int i = 0; i < additionalTexts.Length; ++i)
                additionalTexts[i].color = new Color(additionalTexts[i].color.r, additionalTexts[i].color.g, additionalTexts[i].color.b, t);

            yield return null;
        }
    }
}
