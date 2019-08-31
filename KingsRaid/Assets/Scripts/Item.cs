using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    [Header("General Item Attributes")]
    public Sprite itemIcon;
    public int baseValue;
    public int level;
    public string itemName;
    public int weight;
    public Vector2 size;
    public bool isEquippable;
    public bool isUsable;
    public ItemType itemType;

    [Header("External Components")]
    public Sprite renderableForm = null;
    public Actor owner = null;

    public ItemFactory.ItemEssentials mainItemEssentials;

    public enum ItemType { WEAPON, ARMOR, MISC, POTION }
    public Item(ItemFactory.ItemEssentials ingredients)
    {
        mainItemEssentials = ingredients;

        this.itemIcon = ingredients.itemIcon;
        this.baseValue = ingredients.baseValue;
        this.level = ingredients.level;
        this.itemName = ingredients.itemName;
        this.weight = ingredients.weight;
        this.size = ingredients.size;
        this.isEquippable = ingredients.isEquippable;
        this.isUsable = ingredients.isUsable;

        this.renderableForm = ingredients.renderableForm;
        this.owner = ingredients.owner;
        this.itemType = ingredients.itemType;
    }

    public virtual void UseSkill(Transform attackOrigin) { Debug.Log(" Invalid Attack "); }  // 
    public virtual void Attack(Transform attackOrigin) { Debug.Log(" Invalid Skill Use "); }  // 
    public virtual int  GetDamage() { Debug.Log("Invalid function call "); return -1; } // 
    public virtual Weapon.ClassType GetClassType() { Debug.Log("Invalid function call"); return Weapon.ClassType.SWORD; }
    public virtual Color GetColor() { Debug.Log("Invalid function call"); return new Color(0.0f, 0.0f, 0.0f); }
    public virtual int GetValue() { return this.baseValue; } // 

    public virtual void Use() { Debug.Log("Wrong use"); } //
    public virtual string GetName() { return this.itemName; } // 

}
