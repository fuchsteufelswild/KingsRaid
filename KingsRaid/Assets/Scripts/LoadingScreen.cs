using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public UnityEngine.UI.Image image;
    public UnityEngine.UI.Text text;
    public UnityEngine.UI.Text text1;

    public float timeToFade = 2f;
    public bool zeroOne = true;

    public bool gameOver = false;

    public float fadeInStartVal;
    public float fadeOutStartVal;


    public void Perform()
    {
        One();
    }

    public void Zero()
    {
        StartCoroutine(StartFadeOut(timeToFade));
    }

    public void One()
    {
        StartCoroutine(StartFadeIn(timeToFade));
    }




    IEnumerator StartFadeOut(float time)
    {
        for (float t = fadeOutStartVal; t > 0f; t -= Time.deltaTime / time)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, t);
            text.color = new Color(text.color.r, text.color.g, text.color.b, t);
            text1.color = new Color(text1.color.r, text1.color.g, text1.color.b, t);
            yield return null;
        }

        image.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        text1.gameObject.SetActive(false);

        PlayerController pController = Character.instance.GetComponent<PlayerController>();
        pController.background.clip = pController.backgroundClips[1 - 1 * pController.current];
        pController.current = 1 - 1 * pController.current;
        pController.background.Play();
        Character.instance.blockInput = false;
    }

    IEnumerator StartFadeIn(float time)
    {
        Character.instance.blockInput = true;

        image.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        text1.gameObject.SetActive(true);

        for (float t = fadeInStartVal; t < 1f; t += Time.deltaTime / time)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, t);
            text.color = new Color(text.color.r, text.color.g, text.color.b, t);
            text1.color = new Color(text1.color.r, text1.color.g, text1.color.b, t);
            yield return null;
        }

        if (zeroOne)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(StartFadeOut(time));
        }

        if(gameOver)
        {
            int i = 0;
            while (true)
            {
                ++i;
                if(i > 1000)
                {
                    Scene scene = SceneManager.GetActiveScene();
                    image.gameObject.SetActive(false);
                    text.gameObject.SetActive(false);
                    text1.gameObject.SetActive(false);
                    GameManager.instance.currentLevel = 1;
                    Destroy(CanvasScript.instance);
                    GameManager.instance.mapBounds.x = -10.99f;
                    GameManager.instance.mapBounds.y = 8.85f;
                    Companion.idCounter = 0;
                    LevelGenerator.instance.containers.Clear();
                    LevelGenerator.instance.mobs.Clear();
                    SceneManager.LoadScene(scene.name);
                    break;
                }
                yield return new WaitForSeconds(0.01f);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Scene scene = SceneManager.GetActiveScene();
                    image.gameObject.SetActive(false);
                    text.gameObject.SetActive(false);
                    text1.gameObject.SetActive(false);
                    GameManager.instance.currentLevel = 1;
                    Destroy(CanvasScript.instance.headerUI.gameObject);
                    Destroy(CanvasScript.instance.gameObject);
                    GameManager.instance.mapBounds.x = -10.99f;
                    GameManager.instance.mapBounds.y = 8.85f;
                    Companion.idCounter = 0;
                    LevelGenerator.instance.containers.Clear();
                    LevelGenerator.instance.mobs.Clear();
                    SceneManager.LoadScene(scene.name);
                    break;
                }
            }
        }
    }
}
