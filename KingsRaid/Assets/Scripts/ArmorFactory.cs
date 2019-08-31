using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorFactory
{

    public struct ArmorEssentials
    {
        public Armor.ClassType armType;
        // Maybe aura effects may added as PostEffects

        public ArmorEssentials(Armor.ClassType type) { armType = type; }
    }

    public Armor CreateArmor(ArmorEssentials armorEssentials, ItemFactory.ItemEssentials itemEssentials)
    {
        switch(armorEssentials.armType)
        {
            case Armor.ClassType.HELMET:
                return new Helmet(itemEssentials);
            case Armor.ClassType.CHEST:
                return new Chest(itemEssentials);
            case Armor.ClassType.SHOES:
                return new Shoes(itemEssentials);
            default:
                return null;
        }
    }
}
