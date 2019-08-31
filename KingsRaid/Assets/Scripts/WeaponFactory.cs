using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory
{
    public struct WeaponEssentials
    {
        public AttributeType attr;
        public Weapon.ClassType wep;
        public GameObject attackEffect;
        public GameObject attackProjectile;
        public int baseDamage;

        public Weapon.SkillClassType sWep;
        public GameObject skillEffect;
        public GameObject skillProjectile;
        public Sprite skillIcon;

        public WeaponEssentials(AttributeType pAttr, Weapon.ClassType pWep, GameObject pAttackEffect, int pBaseDamage,
                                Weapon.SkillClassType pSWep, GameObject pSkillEffect, GameObject pSkillProjectile = null, GameObject pAttackProjectile = null, Sprite pSkillIcon = null)
        {
            attr = pAttr;
            wep = pWep;
            attackEffect = pAttackEffect;
            attackProjectile = pAttackProjectile;
            baseDamage = pBaseDamage;
            skillIcon = pSkillIcon;

            sWep = pSWep;
            skillEffect = pSkillEffect;
            skillProjectile = pSkillProjectile;
        }
    }
    public enum AttributeType
    {
        NONE,
        FIRE,
        ICE
    }

    public Weapon CreateWeapon(WeaponEssentials ingredients, ItemFactory.ItemEssentials itemEssentials)
    {
        Weapon weapon = new ConcreteWeapon(ingredients.baseDamage, ingredients.wep, ingredients.sWep, itemEssentials, ingredients.skillEffect, ingredients.skillProjectile,
                                           ingredients.attackEffect, ingredients.attackProjectile, ingredients.skillIcon);
        if (ingredients.attr == AttributeType.FIRE)
            weapon = new FireAttribute((Weapon)weapon);
        else if (ingredients.attr == AttributeType.ICE)
            weapon = new IceAttribute((Weapon)weapon);
        return weapon;
    }
}
