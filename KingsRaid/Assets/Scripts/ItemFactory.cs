using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory
{
    public struct ItemEssentials
    {
        public Sprite itemIcon;
        public int baseValue;
        public int level;
        public string itemName;
        public int weight;
        public Vector2 size;
        public bool isEquippable;
        public bool isUsable;
        public Item.ItemType itemType;

        public Sprite renderableForm;
        public Actor owner;

        public ItemEssentials(Sprite pItemIcon, int pBaseValue, int pLevel, string pItemName, int pWeight, bool pIsEquippable, Vector2 pSize, bool pIsUsable, Item.ItemType pItemType,
                       Sprite pRenderableForm = null, Weapon pWeaponForm = null, Actor pOwner = null)
        {
            this.itemIcon = pItemIcon;
            this.baseValue = pBaseValue;
            this.level = pLevel;
            this.itemName = pItemName;
            this.weight = pWeight;
            this.size = pSize;
            this.isEquippable = pIsEquippable;
            this.isUsable = pIsUsable;
            this.itemType = pItemType;

            this.renderableForm = pRenderableForm;
            this.owner = pOwner;
        }
    }

   public Item CreateItem(ItemEssentials ingredients)
    {
        Item newItem = new Item(ingredients);

        return newItem;
    }
}
