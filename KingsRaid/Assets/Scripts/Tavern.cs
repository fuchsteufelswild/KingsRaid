using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tavern : MonoBehaviour
{
    public bool toOpen = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CanvasScript.instance.prompt.gameObject.SetActive(true);
            toOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CanvasScript.instance.prompt.gameObject.SetActive(false);
            CanvasScript.instance.companionInfo.gameObject.SetActive(false);
            CanvasScript.instance.companionShopPanel.gameObject.SetActive(false);
            toOpen = false;
        }
    }

    private void Update()
    {
        if(!toOpen)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!CanvasScript.instance.companionShopPanel.gameObject.activeInHierarchy)
            {
                Character.instance.audioSource.PlayOneShot(Character.instance.panelAudioClips[1]);
                CanvasScript.instance.companionShopPanel.gameObject.SetActive(true);
                CanvasScript.instance.prompt.gameObject.SetActive(false);
            }
            else
            {
                Character.instance.audioSource.PlayOneShot(Character.instance.panelAudioClips[0]);
                CanvasScript.instance.companionShopPanel.gameObject.SetActive(false);
                CanvasScript.instance.companionInfo.gameObject.SetActive(false);
                CanvasScript.instance.prompt.gameObject.SetActive(false);
            }
        }
    }
}
