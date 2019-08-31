using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionFactory 
{
    public struct PotionEssentials
    {
        // Maybe additional buffs Speed post effect etc.
        public Potion.PotionType potType;
        public float regenValue;

        public PotionEssentials(Potion.PotionType type, float regen) { potType = type; regenValue = regen; }
    }

    public Potion CreatePotion(PotionEssentials potEssentials, ItemFactory.ItemEssentials itemEssentials)
    {
        Potion pot = null;
        switch(potEssentials.potType)
        {
            case Potion.PotionType.HEALTH:
                pot = new HealthPotion(itemEssentials);
                break;
            case Potion.PotionType.MANA:
                break;
            case Potion.PotionType.STAMINA:
                break;
        }

        // 


        return pot;
    }
}
