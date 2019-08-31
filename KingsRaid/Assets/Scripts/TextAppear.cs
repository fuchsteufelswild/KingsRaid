using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAppear : MonoBehaviour
{
    public string textToAppear = "...";
    public UnityEngine.UI.Text text;

    public GameObject[] toActivate;
    public bool continuous = true;
    // Start is called before the first frame 
    void Start()
    {
        // StartCoroutine(Appear());
    }

    private void OnEnable()
    {
        StartCoroutine(Appear());
    }

    IEnumerator Appear()
    { 
        text.text = "";
        
        foreach(char ch in textToAppear)
        {
            text.text += ch;
            yield return new WaitForSeconds(0.05f);
        }

        foreach(GameObject gameObject in toActivate)
        {
            gameObject.SetActive(true);
        }

        if (continuous)
        {
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(Appear());
        }
    }
}
