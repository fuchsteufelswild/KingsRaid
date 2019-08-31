using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItem : GUIItem
{
    private void Awake()
    {
        if (item == null)
            image.gameObject.SetActive(false);
    }

    public override void LeftClick()
    {
        if (GameManager.instance.openStore != null && item != null)
        {
            if (Character.instance.firstEmptyItemSlot < Character.instance.InventorySize)
            {
                if (Character.instance.coin < item.GetValue())
                    return;

                Character.instance.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(0));
                Character.instance.coin -= item.GetValue();

                if (item.itemType != Item.ItemType.POTION)
                {
                    Character.instance.AddItem(item);
                    GameManager.instance.openStore.items.Remove(item);
                    
                    item = null;
                    image.sprite = null;
                    image.gameObject.SetActive(false);
                }
                else
                    Character.instance.AddItem(new HealthPotion(((HealthPotion)item)));

                Character.instance.UpdateInventory();
            }
        }

    }

    public override void RightClick()
    {
        // Handle Right Click
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            CanvasScript.instance.itemInfo.SetActive(false);
            return;
        }
        CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>().type = 2;

        base.OnPointerEnter(eventData);
    }
}
