using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> mobs;
    public SpriteRenderer[] backgrounds;
    public GameObject leavePrompt;

    public List<GameObject> containers;

    public static LevelGenerator instance;

    public void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeBackgrounds(Sprite targetSprite)
    {
        foreach (SpriteRenderer spriteRenderer in backgrounds)
            spriteRenderer.sprite = targetSprite;
    }

    public void RemoveMob(GameObject toBeRemoved)
    {
        this.mobs.Remove(toBeRemoved);

        if (this.mobs.Count <= 0)
            this.leavePrompt.SetActive(true);
    }
}
