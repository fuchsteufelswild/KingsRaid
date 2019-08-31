using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect
{
    public enum PostEffectType { NONE, FREEZE, BURN }
    public Actor owner;
    public float lifeTime = 2f;
    public float timePassed = 0.0f;
    public PostEffectType type;

    protected SpriteRenderer parentRenderer;

    protected PostEffect(Actor _owner){ this.owner = _owner; parentRenderer = owner.GetComponentInChildren<SpriteRenderer>(); }

    public virtual void PerformEffect() { while (owner == null) ;  }

    public virtual void RevertEffect() { }
}

public class FreezeEffect : PostEffect
{
    public FreezeEffect(Actor _owner) : base(_owner) { type = PostEffectType.FREEZE; }
    public float oldSpeed;
    public override void PerformEffect()
    {
        oldSpeed = owner.moveSpeed;
        owner.moveSpeed -= 1f;
        parentRenderer.color = new Color(0.0f, 0.0f, 250.0f, 1.0f);
    }

    public override void RevertEffect()
    {
        owner.moveSpeed = oldSpeed;
        parentRenderer.color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
    }
    

}

public class BurnEffect : PostEffect
{
    public float nextEffect = 0.0f;
    public float damage = 6f;

    public BurnEffect(Actor _owner) : base(_owner) { type = PostEffectType.BURN; }

    public override void PerformEffect()
    {
        parentRenderer.color = new Color(150.0f, 0.0f, 0.0f, 1.0f);
        owner.TakeDamage(this.damage * ( GameManager.instance.currentLevel * 1.5f));
    }

    public override void RevertEffect()
    {
        parentRenderer.color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
    }

}
