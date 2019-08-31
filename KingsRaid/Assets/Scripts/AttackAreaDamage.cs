using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaDamage : MonoBehaviour
{
    public List<string> typesToAttack;
    public int damage;
    public Actor owner = null;
    public PostEffect.PostEffectType postEffect = PostEffect.PostEffectType.NONE;

    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        Collider2D[] collided = Physics2D.OverlapCapsuleAll(this.transform.position, new Vector2(0.29f, 0.79f), CapsuleDirection2D.Vertical, 0.0f);

        foreach(Collider2D collide in collided)
        {
            bool hit = false;
            for (int i = 0; i < typesToAttack.Count; ++i)
                if (collide.tag == typesToAttack[i])
                    hit = true;

            if (hit)
            {
                source.PlayOneShot(GameObjectContainers.instance.GetAudioClip(8));
                if (this.postEffect == PostEffect.PostEffectType.BURN)
                    collide.GetComponent<Actor>().AddPostEffect(new BurnEffect(collide.GetComponent<Actor>()));
                else if (this.postEffect == PostEffect.PostEffectType.FREEZE)
                    collide.GetComponent<Actor>().AddPostEffect(new FreezeEffect(collide.GetComponent<Actor>()));
                bool result = collide.gameObject.GetComponent<Actor>().TakeDamage(damage);

                if (owner != null && result)
                    ((NPC)owner).CheckForOpponent();
            }
        }

        Destroy(this.gameObject);
    }
}
