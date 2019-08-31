using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackType : MonoBehaviour
{
    public abstract string GetName();
    public abstract void OnAttack(Transform targetTransform, Color color, GameObject effect = null, GameObject projectile = null, PostEffect.PostEffectType pEffect = PostEffect.PostEffectType.NONE);
}

public class BowAttack : AttackType
{
    public override string GetName() { return "BOW"; }

    public override void OnAttack(Transform targetTransform, Color color, GameObject effect = null, GameObject projectile = null, PostEffect.PostEffectType pEffect = PostEffect.PostEffectType.NONE)
    {
        //effect.GetComponent<SpriteRenderer>().color = color;
        //Instantiate(effect, new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z), Quaternion.identity);

        GameObject temp = Instantiate(projectile, new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z), Quaternion.identity) as GameObject;
        Projectile tProjectile = temp.GetComponent<Projectile>();
        tProjectile.typesToAttack = new List<string>();
        tProjectile.typesToAttack.Add("Enemy");
        tProjectile.damage = Character.instance.weapon.GetDamage();
        tProjectile.SetSpeed(2.0f);
        tProjectile.postEffect = pEffect;
        tProjectile.GetComponentInChildren<SpriteRenderer>().color = color;

        Vector3 newLocalScale = tProjectile.transform.localScale;
        if (Character.instance.transform.localScale.x > 0)
        {
            if (tProjectile != null && tProjectile.xScale != 0)
                newLocalScale.x = 1f + tProjectile.xScale;
            else
                newLocalScale.x = 1f - 0.3f;
            tProjectile.GetComponent<Projectile>().SetSpeed(1f * 7.0f);
        }
        else
        {
            if (tProjectile != null && tProjectile.xScale != 0)
                newLocalScale.x = -1f - tProjectile.xScale;
            else
                newLocalScale.x = -1f + 0.3f;
            tProjectile.GetComponent<Projectile>().SetSpeed(-1f * 7.0f);
        }
        tProjectile.transform.localScale = newLocalScale;
        
    }
}

public class SwordAttack : AttackType
{
    public override string GetName() { return "SWORD"; }

    public override void OnAttack(Transform targetTransform, Color color, GameObject effect = null, GameObject projectile = null, PostEffect.PostEffectType pEffect = PostEffect.PostEffectType.NONE)
    {
        
        GameObject temp = Instantiate(effect, new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z), Quaternion.identity) as GameObject;
        GameObject tArea = Instantiate(Character.instance.meleeAttackArea, new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z), Quaternion.identity) as GameObject;
        AttackAreaDamage Area = tArea.GetComponent<AttackAreaDamage>();
        Area.damage = Character.instance.weapon.GetDamage();
        Area.typesToAttack.Add("Enemy");
        Area.typesToAttack.Add("Enemy");
        Area.postEffect = pEffect;

        temp.GetComponent<SpriteRenderer>().color = color;
        Vector3 newLocalScale = temp.transform.localScale;
        if (Character.instance.transform.localScale.x > 0)
            newLocalScale.x = 1f - 0.3f;
        else
            newLocalScale.x = -1f + 0.3f;

        temp.transform.localScale = newLocalScale;
    }
}

public class BumerangAttack : AttackType
{
    public override string GetName() { return "BUMERANG"; }

    public override void OnAttack(Transform targetTransform, Color color, GameObject effect = null, GameObject projectile = null, PostEffect.PostEffectType pEffect = PostEffect.PostEffectType.NONE)
    {

    }
}