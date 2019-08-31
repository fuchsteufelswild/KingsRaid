using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    public Potion(ItemFactory.ItemEssentials itemEssentials) : base(itemEssentials) { essentials = itemEssentials; }
    public enum PotionType { HEALTH, MANA, STAMINA }

    public ItemFactory.ItemEssentials essentials;

    public float regenerateValue = GameManager.instance.currentLevel * 10;

    public PotionType potionType;
    // Right Click on the Item in the inventory
    public override void Use() { }

    public override string GetName()
    {
        return this.itemName;
    }
}

public class HealthPotion : Potion
{
    public HealthPotion(ItemFactory.ItemEssentials itemEssentials) : base(itemEssentials) { potionType = PotionType.HEALTH; }
    public HealthPotion(HealthPotion potion) : base(potion.essentials) { this.potionType = PotionType.HEALTH; }

    public override int GetDamage()
    {
        return (int)(regenerateValue);
    }
    public override void Use()
    {
        this.owner.Regenerate(this.regenerateValue, this.potionType);
    }
}



