using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    public static CanvasScript instance;

    public GameObject inventoryPanel;
    public GameObject containerPanel;
    public GameObject prompt;
    public GameObject NPCPanel;
    public GameObject NPCArmorPanel;
    public GameObject itemInfo;
    public GameObject travelPrompt;
    public GameObject companionInfo;
    public GameObject companionShopPanel;
    public GameObject headerUI;
    public GameObject order;
    public GameObject potionPanel;
    public GameObject characterPanel;

    public GameObject hintPrompt;
    public GameObject hints;
    public GameObject welcomeText;

    public GameObject loadingScreen;
    public GameObject gameOver;
    public GameObject levelAppear;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
}
