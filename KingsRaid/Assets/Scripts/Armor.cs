using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    public int baseDefence;

    // Additional immunity values

    public enum ClassType { NONE, SHOES, CHEST, HELMET  };
    public Armor(ItemFactory.ItemEssentials itemEssentials) : base(itemEssentials) { }

    public ClassType type = ClassType.NONE;

    // Right Click on the Item in the inventory
    public override void Use()
    {
        this.owner.SetArmor(this);
    }

    public override string GetName()
    {
        return this.itemName;
    }
}

public class Helmet : Armor
{
    public Helmet(ItemFactory.ItemEssentials itemEssentials) : base(itemEssentials) { this.type = ClassType.HELMET; baseDefence = GameManager.instance.currentLevel * Random.Range(GameManager.instance.currentLevel * 5, GameManager.instance.currentLevel * 10); }
}

public class Chest : Armor
{
    public Chest(ItemFactory.ItemEssentials itemEssentials) : base(itemEssentials) { this.type = ClassType.CHEST; baseDefence = GameManager.instance.currentLevel * Random.Range(GameManager.instance.currentLevel * 7, GameManager.instance.currentLevel * 14); }
}

public class Shoes : Armor
{
    public Shoes(ItemFactory.ItemEssentials itemEssentials) : base(itemEssentials) { this.type = ClassType.SHOES; baseDefence = GameManager.instance.currentLevel * Random.Range(GameManager.instance.currentLevel * 3, GameManager.instance.currentLevel * 6); }
}
