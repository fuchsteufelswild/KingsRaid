using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : GUIItem
{
    private void Awake()
    {
        if (item == null)
            image.gameObject.SetActive(false);
    }

    public override void LeftClick()
    {
        if (GameManager.instance.openContainer != null)
        {
            if (GameManager.instance.openContainer.firstEmptySlot < GameManager.instance.openContainer.containerSize)
            {
                GameManager.instance.openContainer.AddItem(item);
                GameManager.instance.openContainer.UpdateContainer();
                Character.instance.RemoveItem(item);

                item = null;
                image.gameObject.SetActive(false);
            }
        }

        else if (GameManager.instance.openStore != null)
        {
            // GameManager.instance.openStore.items.Add(item);
            Character.instance.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(0));
            Character.instance.RemoveItem(item);
            Character.instance.coin += item.GetValue();

            item = null;
            image.sprite = null;
            image.gameObject.SetActive(false);

            Character.instance.UpdateInventory();
            // GameManager.instance.openStore.UpdateContainer();
        }
    }

    public override void RightClick()
    {
        if(item.itemType == Item.ItemType.POTION)
            Character.instance.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(5));
        item.owner = Character.instance;
        Character.instance.RemoveItem(item);

        item.Use();
        item = null;
        image.gameObject.SetActive(false);
        Character.instance.UpdateInventory();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            CanvasScript.instance.itemInfo.SetActive(false);
            return;
        }

        CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>().type = 0;

        base.OnPointerEnter(eventData);
    }
}
