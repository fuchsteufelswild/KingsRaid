using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [Header("Container Essentials")]
    public Sprite renderableForm;
    public Item[] items;
    public string containerName;
    public int containerSize = 20;
    public int firstEmptySlot = 0;

    public void AddItem(Item newItem)
    {
        items[firstEmptySlot] = newItem;
        ++firstEmptySlot;
    }

    public void RemoveItem(Item oldItem)
    {
        for(int i = 0; i < containerSize; ++i)
        {
            if(oldItem == items[i])
            {
                if (firstEmptySlot > i)
                    firstEmptySlot = i;
                items[i] = null;
            }
        }
    }

    public void Awake()
    {
        items = new Item[containerSize];
        for (int i = 0; i < containerSize; ++i)
            items[i] = null;
        this.GetComponent<SpriteRenderer>().sprite = renderableForm;
    }
    public void SetImageActivity(bool newActivity)
    {
        CanvasScript.instance.containerPanel.gameObject.SetActive(newActivity);
        CanvasScript.instance.prompt.gameObject.SetActive(false);
        UpdateContainer();
    }

    public void UpdateContainer()
    {
        GUIItem[] containerItems = CanvasScript.instance.containerPanel.GetComponent<UIPanels>().itemHolders;

        for (int i = 0; i < items.Length; ++i)
            containerItems[i].SetItem(items[i]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Character>().toBeInteracted = this.gameObject;
            collision.GetComponent<Character>().toInteract = true;

            CanvasScript.instance.prompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Character>().toBeInteracted = null;
            collision.GetComponent<Character>().toInteract = false;

            CanvasScript.instance.prompt.gameObject.SetActive(false);
            CanvasScript.instance.containerPanel.gameObject.SetActive(false);

            GameManager.instance.openContainer = null;

            if (CanvasScript.instance.itemInfo.activeInHierarchy && CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>().type == 1)
                CanvasScript.instance.itemInfo.SetActive(false);
        }
    }

}
