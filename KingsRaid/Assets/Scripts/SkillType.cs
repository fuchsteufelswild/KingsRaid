using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillType : MonoBehaviour
{
    public abstract string GetName();
    public abstract void OnSkillUse(Transform targetTransform, Color color, GameObject effect = null, GameObject projectile = null, PostEffect.PostEffectType pEffect = PostEffect.PostEffectType.NONE);

    public void CreateProjectile(Transform targetTransform, GameObject projectile, Quaternion rot, Color color, float offsetX = 0, float offsetY = 0, float upForce = 0, PostEffect.PostEffectType pEffect = PostEffect.PostEffectType.NONE)
    {
        GameObject temp = Instantiate(projectile, new Vector3(targetTransform.position.x + offsetX, targetTransform.position.y + offsetY, targetTransform.position.z), rot) as GameObject;
        Projectile tProjectile = temp.GetComponent<Projectile>();
        tProjectile.typesToAttack = new List<string>();
        tProjectile.typesToAttack.Add("Enemy");
        tProjectile.damage = Character.instance.weapon.GetDamage();
        tProjectile.SetSpeed(2.0f);
        tProjectile.GetComponentInChildren<SpriteRenderer>().color = color;
        tProjectile.postEffect = pEffect;

        Vector3 newLocalScale = tProjectile.transform.localScale;
        if (Character.instance.transform.localScale.x > 0)
        {
            if (tProjectile != null && tProjectile.xScale != 0)
                newLocalScale.x = 1f + tProjectile.xScale;
            else
                newLocalScale.x = 1f - 0.3f;
            tProjectile.GetComponent<Projectile>().SetSpeed(1f * 12.0f);
        }
        else
        {
            if (rot != Quaternion.identity)
            {
                tProjectile.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.z * -1f));
            }
            if (tProjectile != null && tProjectile.xScale != 0)
                newLocalScale.x = -1f - tProjectile.xScale;
            else
                newLocalScale.x = -1f + 0.3f;
            tProjectile.GetComponent<Projectile>().SetSpeed(-1f * 12.0f);
        }
        tProjectile.transform.localScale = newLocalScale;

        tProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, upForce));
    }
}

public class BowThreeArrow : SkillType
{
    public override string GetName() { return "BOWTHREEARROW"; }
    public override void OnSkillUse(Transform targetTransform, Color color, GameObject effect = null, GameObject projectile = null, PostEffect.PostEffectType pEffect = PostEffect.PostEffectType.NONE)
    {
        effect.GetComponent<SpriteRenderer>().color = color;
        Instantiate(effect, new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z), Quaternion.identity);


        for (int i = 0; i < 3; ++i)
        {
            CreateProjectile(targetTransform, projectile, Quaternion.Euler(0f, 0f, 45 - i * 45), color, 0, 0, (45 - i * 45) * 8, pEffect);
        }
    }
}

public class BowFiveArrows : SkillType
{
    public override string GetName() { return "BOWFIVEARROW"; }
    public override void OnSkillUse(Transform targetTransform, Color color, GameObject effect = null, GameObject projectile = null, PostEffect.PostEffectType pEffect = PostEffect.PostEffectType.NONE)
    {
     
        effect.GetComponent<SpriteRenderer>().color = color;
        Instantiate(effect, new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z), Quaternion.identity);

        for (int i = 0; i < 5; ++i)
        {
            if(i > 3)
                CreateProjectile(targetTransform, projectile, Quaternion.identity, color, 0f, (i - 3) * 0.1f, 0, pEffect);
            else
                CreateProjectile(targetTransform, projectile, Quaternion.identity, color, 0f, -i * 0.1f, 0, pEffect);


        }
    }
}

public class FireBall : SkillType
{
    public override string GetName() { return "FIREBALL"; }
    public override void OnSkillUse(Transform targetTransform, Color color, GameObject effect = null, GameObject projectile = null, PostEffect.PostEffectType pEffect = PostEffect.PostEffectType.NONE)
    {
        effect.GetComponent<SpriteRenderer>().color = color;
        Instantiate(effect, new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z), Quaternion.identity);
        CreateProjectile(targetTransform, projectile, Quaternion.identity, color, 0, 0, 0, pEffect);
    }
}

public class SwordWawe : SkillType
{
    public override string GetName() { return "SWORDWAWE"; }
    public override void OnSkillUse(Transform targetTransform, Color color, GameObject effect = null, GameObject projectile = null, PostEffect.PostEffectType pEffect = PostEffect.PostEffectType.NONE)
    {
        Debug.Log("SwordWawe Used");
    }
}