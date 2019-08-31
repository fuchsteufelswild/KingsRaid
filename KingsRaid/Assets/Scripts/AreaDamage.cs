using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    public float radius;
    public float explosionDamage;

    public int explosionType;

    public string[] typesToTarget;
    
    void Start()
    {
        this.GetComponent<Animator>().SetInteger("ExplosionType", explosionType);
        Collider2D[] collided = Physics2D.OverlapCircleAll(this.transform.position, radius);
        
        foreach(Collider2D collide in collided)
        {
            if(collide.tag == typesToTarget[0] || collide.tag == typesToTarget[1])
                collide.GetComponent<Actor>().TakeDamage(explosionDamage);
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
