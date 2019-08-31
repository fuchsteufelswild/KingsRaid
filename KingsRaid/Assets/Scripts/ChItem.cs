using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChItem : GUIItem
{
    private void Awake()
    {
        if (item == null)
            image.gameObject.SetActive(false);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            CanvasScript.instance.itemInfo.SetActive(false);
            return;
        }

        CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>().type = 3;

        base.OnPointerEnter(eventData);
    }

    public override void RightClick()
    {
        if (Character.instance.firstEmptyItemSlot < Character.instance.InventorySize)
        {
            item.owner = Character.instance;
            Character.instance.AddItem(item);
            
            
            if(item.itemType == Item.ItemType.WEAPON)
            {
                if (((Weapon)item).attackType == Weapon.ClassType.BOW)
                    Character.instance.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(2));
                else
                    Character.instance.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(1));
                Character.instance.attack -= item.GetDamage();
            }
            else
            {
                Character.instance.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(4));
                Character.instance.defence -= ((Armor)item).baseDefence;
            }

            Character.instance.UnequipItem(item);
            item = null;
            image.gameObject.SetActive(false);
            Character.instance.UpdateInventory();
        }
    }
}
