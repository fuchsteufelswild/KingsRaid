using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContainerItem : GUIItem
{
    private void Awake()
    {
        if (item == null)
            image.gameObject.SetActive(false);
    }

    public override void LeftClick()
    {
        if (item != null && Character.instance.firstEmptyItemSlot < Character.instance.InventorySize)
        {
            Character.instance.AddItem(item);
            if (CanvasScript.instance.inventoryPanel.activeInHierarchy)
                Character.instance.UpdateInventory();
            GameManager.instance.openContainer.RemoveItem(item);
            item = null;
            image.gameObject.SetActive(false);

            if (CanvasScript.instance.containerPanel.activeInHierarchy)
                GameManager.instance.openContainer.UpdateContainer();
        }
    }

    public override void RightClick()
    {
        // Empty
        return;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            CanvasScript.instance.itemInfo.SetActive(false);
            return;
        }
        CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>().type = 1;

        base.OnPointerEnter(eventData);
    }

}
