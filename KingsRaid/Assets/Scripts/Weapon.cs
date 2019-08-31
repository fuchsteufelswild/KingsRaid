using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    public GameObject projectile = null;
    public GameObject effect = null;

    public GameObject skillProjectile = null;
    public GameObject skillEffect = null;

    public Sprite skillIcon;

    protected int baseDamage;
    protected Color baseColor = new Color(255.0f, 255.0f, 255.0f, 1.0f);

    public enum ClassType { SWORD, BOW, BUMERANG, SHORT_SWORD };
    public enum SkillClassType { FIREBALL, BOWFIVEARROW, BOWTHREEARROWS, SWORDWAWE };

    public ClassType attackType;

    public PostEffect.PostEffectType postEffect = PostEffect.PostEffectType.NONE;

    public Weapon(ItemFactory.ItemEssentials itemEssentials) : base(itemEssentials) { }

    public AttackType type;
    public SkillType skillType;

    public override ClassType GetClassType() { return attackType; }
    // Left Click
    public override void Attack(Transform attackOrigin)
    {
        // Trigger owner anim
    }

    // Right Click on the Item in the inventory
    public override void Use()
    {
        this.owner.SetWeapon(this);
    }

    public override string GetName()
    {
        return this.itemName;
    }
}

public class ConcreteWeapon : Weapon
{
    public ConcreteWeapon(int dmg, ClassType aType, SkillClassType sType, ItemFactory.ItemEssentials essentials, GameObject sEffect = null, GameObject sProj = null,
                                                                          GameObject pEffect = null, GameObject proj = null, Sprite pSkillIcon = null) : base(essentials)
    {
        baseDamage = dmg;
        projectile = proj;
        attackType = aType;
        this.effect = pEffect;
        this.projectile = proj;
        this.skillEffect = sEffect;
        this.skillProjectile = sProj;
        this.skillIcon = pSkillIcon;

        switch(aType)
        {
            case ClassType.SWORD:
                type = GameManager.instance.attackTypeFactory.GetAttackType("SWORD");
                break;
            case ClassType.BOW:
                type = GameManager.instance.attackTypeFactory.GetAttackType("BOW");
                break;
            case ClassType.BUMERANG:
                type = GameManager.instance.attackTypeFactory.GetAttackType("BUMERANG");
                break;
            default:
                break;
        }

        switch (sType)
        {
            case SkillClassType.BOWTHREEARROWS:
                skillType = GameManager.instance.skillTypeFactory.GetSkillType("BOWTHREEARROW");
                break;
            case SkillClassType.SWORDWAWE:
                skillType = GameManager.instance.skillTypeFactory.GetSkillType("SWORDWAWE");
                break;
            case SkillClassType.BOWFIVEARROW:
                skillType = GameManager.instance.skillTypeFactory.GetSkillType("BOWFIVEARROW");
                this.baseValue += (int)((GameManager.instance.currentLevel * 0.2f) * 30);
                break;
            case SkillClassType.FIREBALL:
                skillType = GameManager.instance.skillTypeFactory.GetSkillType("FIREBALL");
                break;
            default:
                break;
        }
    }

    public override int GetDamage()
    {
        return (int)(this.baseDamage * (1f + this.level * 0.2f));
    }

    public override void Attack(Transform attackOrigin)
    {
        this.type.OnAttack(attackOrigin, this.GetColor(), effect, this.projectile, this.postEffect);
    }

    public override void UseSkill(Transform attackOrigin)
    {
        this.skillType.OnSkillUse(attackOrigin, this.GetColor(), this.skillEffect, this.skillProjectile, this.postEffect);
    }
    public override void Use()
    {
        base.Use();
    }

    public override string GetName()
    {
        return this.itemName;
    }

    public override int GetValue()
    {
        return (int)(this.baseValue * (1f + this.level * 0.3f));
    }

    public override Color GetColor()
    {
        return this.baseColor;
    }
}

public abstract class WeaponAttribute : Weapon
{
    protected WeaponAttribute(ItemFactory.ItemEssentials essentials) : base(essentials) { }
}

// Attributes
public class FireAttribute : WeaponAttribute
{
    public Weapon wrappedObject;
    private int additionalDamage = 7;
    private int additionalValue = 12;
    private string attributeName = "Fire";
    private Color changeColorValue = new Color(100.0f, -255.0f, -255.0f);

    public FireAttribute(Weapon weapon) : base(weapon.mainItemEssentials) { wrappedObject = weapon; this.type = wrappedObject.type; this.effect = wrappedObject.effect; this.skillType = wrappedObject.skillType;
                this.skillEffect = wrappedObject.skillEffect; this.skillProjectile = wrappedObject.skillProjectile; this.projectile = wrappedObject.projectile; this.skillIcon = wrappedObject.skillIcon; this.attackType = wrappedObject.attackType;
        this.postEffect = PostEffect.PostEffectType.BURN;
    }

    public override void Attack(Transform attackOrigin)
    {
        this.type.OnAttack(attackOrigin, this.GetColor(), effect, this.projectile, this.postEffect);
    }

    public override void UseSkill(Transform attackOrigin)
    {
        this.skillType.OnSkillUse(attackOrigin, this.GetColor(), this.skillEffect, this.skillProjectile, this.postEffect);
    }
    public override void Use()
    {
        base.Use();
    }

    public override string GetName()
    {
        return attributeName + " " + wrappedObject.GetName();
    }

    public override int GetValue()
    {
        return wrappedObject.baseValue + (int)(this.additionalValue * (1f + 0.2f * this.level));
    }

    public override int GetDamage()
    {
        return this.wrappedObject.GetDamage() + (int)(additionalDamage * (1f + 0.2f * this.level));
    }

    public override Color GetColor()
    {
        Color tempColor = this.wrappedObject.GetColor();
        return new Color(Mathf.Clamp(tempColor.r + changeColorValue.r, 0.0f, 255.0f),
                         Mathf.Clamp(tempColor.g + changeColorValue.g, 0.0f, 255.0f),
                         Mathf.Clamp(tempColor.b + changeColorValue.b, 0.0f, 255.0f),
                         Mathf.Clamp(tempColor.a + changeColorValue.a, 0.0f, 1.0f) );
    }

}


public class IceAttribute : WeaponAttribute
{
    public Weapon wrappedObject;
    private int additionalDamage = 5;
    private int additionalValue = 8;
    private string attributeName = "Ice";
    private Color changeColorValue = new Color(-255.0f, -255.0f, 100f);

    public IceAttribute(Weapon weapon) : base(weapon.mainItemEssentials)
    {
        wrappedObject = weapon; this.type = wrappedObject.type; this.effect = wrappedObject.effect; this.skillType = wrappedObject.skillType;
        this.skillEffect = wrappedObject.skillEffect; this.skillProjectile = wrappedObject.skillProjectile; this.projectile = wrappedObject.projectile; this.skillIcon = wrappedObject.skillIcon; this.attackType = wrappedObject.attackType;
        this.postEffect = PostEffect.PostEffectType.FREEZE;
    }

    public override void Attack(Transform attackOrigin)
    {
        this.type.OnAttack(attackOrigin, this.GetColor(), effect, this.projectile, this.postEffect);
    }

    public override void UseSkill(Transform attackOrigin)
    {
        this.skillType.OnSkillUse(attackOrigin, this.GetColor(), this.skillEffect, this.skillProjectile, this.postEffect);
    }

    public override void Use()
    {
        base.Use();
    }

    public override string GetName()
    {
        return attributeName + " " + wrappedObject.GetName();
    }

    public override int GetValue()
    {
        return wrappedObject.baseValue + (int)(this.additionalValue * (1f + 0.2f * this.level));
    }

    public override int GetDamage()
    {
        return this.wrappedObject.GetDamage() + (int)(additionalDamage * (1f + 0.2f * this.level));
    }

    public override Color GetColor()
    {
        Color tempColor = this.wrappedObject.GetColor();
        return new Color(Mathf.Clamp(tempColor.r + changeColorValue.r, 0.0f, 255.0f),
                         Mathf.Clamp(tempColor.g + changeColorValue.g, 0.0f, 255.0f),
                         Mathf.Clamp(tempColor.b + changeColorValue.b, 0.0f, 255.0f),
                         Mathf.Clamp(tempColor.a + changeColorValue.a, 0.0f, 1.0f));
    }

}