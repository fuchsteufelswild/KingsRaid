using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item = null;
    public Companion companion = null;
    public Image image;

    public virtual void LeftClick()
    {

    }

    public virtual void RightClick()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
            RightClick();
        else if (eventData.button == PointerEventData.InputButton.Left && item != null)
            LeftClick();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 cameraPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 infoPos = Camera.main.ScreenToWorldPoint(this.transform.position);

        if (cameraPos.x + 6.4 > infoPos.x + 2.4)
            CanvasScript.instance.itemInfo.transform.position = new Vector3(this.transform.position.x + 360.0f, this.transform.position.y, this.transform.position.z);
        else if (cameraPos.x < infoPos.x - 2.4)
            CanvasScript.instance.itemInfo.transform.position = new Vector3(this.transform.position.x - 360.0f, this.transform.position.y, this.transform.position.z);
        
        CanvasScript.instance.itemInfo.SetActive(true);
        if (item == null)
        {
            //
        }

        ItemInfoScript itemInfo = CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>();
        itemInfo.itemName.text = item.GetName();
        itemInfo.value.text = "Value:  " + item.GetValue().ToString();
        itemInfo.itemIcon.sprite = item.itemIcon;
        itemInfo.itemLevel.text = "Level: " + item.level.ToString();

        if (item.itemType == Item.ItemType.WEAPON)
        {
            itemInfo.damage.text = "Damage: " + item.GetDamage().ToString();
            itemInfo.skillDamage.text = "Skill Damage: " + item.GetDamage().ToString();
            itemInfo.skillName.text = ((Weapon)item).skillType.GetName();
            itemInfo.skillIcon.sprite = ((Weapon)item).skillIcon;


            itemInfo.skillDamage.gameObject.SetActive(true);
            itemInfo.skillName.gameObject.SetActive(true);
            itemInfo.skillIcon.gameObject.SetActive(true);
            itemInfo.skillProjectile.gameObject.SetActive(true);


            if(!(((Weapon)item).attackType == Weapon.ClassType.BOW))
                itemInfo.itemProjectile.gameObject.SetActive(false);

            if (((Weapon)item).projectile != null)
            {
                if (((Weapon)item).attackType == Weapon.ClassType.BOW)
                {
                    itemInfo.itemProjectile.gameObject.SetActive(true);
                    itemInfo.itemProjectile.sprite = ((Weapon)item).projectile.GetComponentInChildren<SpriteRenderer>().sprite;
                }
            }
            if (((Weapon)item).skillProjectile != null)
                itemInfo.skillProjectile.sprite = ((Weapon)item).skillProjectile.GetComponentInChildren<SpriteRenderer>().sprite;
        }
        else if(item.itemType == Item.ItemType.POTION)
        {
            itemInfo.damage.text = "Regen: " + item.GetDamage().ToString();

            itemInfo.skillDamage.gameObject.SetActive(false);
            itemInfo.skillName.gameObject.SetActive(false);
            itemInfo.skillIcon.gameObject.SetActive(false);
            itemInfo.itemProjectile.gameObject.SetActive(false);
            itemInfo.skillProjectile.gameObject.SetActive(false);
        }
        else if(item.itemType == Item.ItemType.ARMOR)
        {
            itemInfo.damage.text = "Defence: " + ((Armor)item).baseDefence;

            itemInfo.skillDamage.gameObject.SetActive(false);
            itemInfo.skillName.gameObject.SetActive(false);
            itemInfo.skillIcon.gameObject.SetActive(false);
            itemInfo.itemProjectile.gameObject.SetActive(false);
            itemInfo.skillProjectile.gameObject.SetActive(false);
        }

        // Handle Enter
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        CanvasScript.instance.itemInfo.SetActive(false);
        CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>().type = -1;
        // Handle Exit
    }

    // Sets item for container space
    public virtual void SetItem(Item newItem)
    {
        if (newItem == null)
        {
            item = null;
            image.sprite = null;
            image.gameObject.SetActive(false);
            return;
        }
        item = newItem;
        image.sprite = item.itemIcon;
        image.gameObject.SetActive(true);
    }
}
