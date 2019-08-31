using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Actor owner = null;
    public float speed;
    public float damage;
    public float xScale = 0f;
    public PostEffect.PostEffectType postEffect = PostEffect.PostEffectType.NONE;

    public int explosionType;
    public GameObject aftermathEffect = null;

    public List<string> typesToAttack;

    public bool animated;
    public int projectileSprite;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetSpeed(float newSpeed)
    {
        if(aftermathEffect != null)
            aftermathEffect.GetComponent<AreaDamage>().explosionType = this.explosionType;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(newSpeed, 0.0f);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool found = false;
        for (int i = 0; i < typesToAttack.Count; ++i)
        {
            if (collision.tag == typesToAttack[i])
            {
                found = true;
            }
        }


        if (!found && collision.tag != "Platform")
            return;


        if (!found && collision.GetComponent<Actor>())
            return;

        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);

        if (found)
        {
            if (this.postEffect == PostEffect.PostEffectType.BURN)
                collision.gameObject.GetComponent<Actor>().AddPostEffect(new BurnEffect(collision.GetComponent<Actor>()));
            else if (this.postEffect == PostEffect.PostEffectType.FREEZE)
                collision.gameObject.GetComponent<Actor>().AddPostEffect(new FreezeEffect(collision.gameObject.GetComponent<Actor>()));

            if (collision.gameObject == Character.instance.gameObject)
            {
                if (!((this.transform.localScale.x > 0 && Character.instance.transform.localScale.x > 0) ||
                    (this.transform.localScale.x < 0 && Character.instance.transform.localScale.x < 0)))
                {
                    bool result = collision.gameObject.GetComponent<Actor>().TakeDamage(damage);

                    if (owner != null && result)
                        ((NPC)owner).CheckForOpponent();
                }

            }
            else
            {
                bool result = collision.gameObject.GetComponent<Actor>().TakeDamage(damage);

                if (owner != null && result)
                    ((NPC)owner).CheckForOpponent();
            }
        }
        if (aftermathEffect != null)
            Instantiate(aftermathEffect, new Vector3(this.transform.position.x + (this.transform.localScale.x * 0.1f), this.transform.position.y, this.transform.position.z), Quaternion.identity);


        if (this.GetComponent<Animator>() != null)
        {
            this.GetComponent<Eraser>().timeElapsed = this.GetComponent<Eraser>().lifeTime - 0.3f;
            this.GetComponent<Animator>().SetTrigger("Impact");
        }
        else
            Destroy(this.gameObject);
        
    }
}
