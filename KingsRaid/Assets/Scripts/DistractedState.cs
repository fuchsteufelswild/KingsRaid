using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractedState : State
{
    public DistractedState(GameObject sParent) : base(sParent) { }

    public override void PerformAction()
    {
        if (parent == null)
            return;

        if (parent.lockedTarget == null)
        {
            parent.CheckForOpponent();
            return;
        }

        Vector3 targetPosition = parent.lockedTarget.transform.position;
        float offset = 0.8f;

        if (parent.type == NPC.NPCType.RANGED)
            offset = 3.0f;

        Vector3 newScale = parent.transform.localScale;
        if (targetPosition.x < parent.transform.position.x)
            newScale.x = -1;
        else
            newScale.x = 1;

        if ((targetPosition.x < parent.transform.position.x && targetPosition.x + offset >= parent.transform.position.x) ||
            (targetPosition.x > parent.transform.position.x && targetPosition.x - offset <= parent.transform.position.x))
        {
            if (parent.attacking == true)
                return;

            parent.transform.localScale = newScale;

            parent.rigidbody.velocity = new Vector2(0.0f, 0.0f);

            if (parent.attacking)
                return;
            parent.attacking = true;

            parent.animator.SetTrigger("Attack");
            parent.Attack();
        
        }
        else if(targetPosition.x + offset < parent.transform.position.x)
        {
            newScale.x = -1;
            parent.transform.localScale = newScale;

            parent.rigidbody.velocity = new Vector2(-parent.moveSpeed, 0.0f);
            parent.animator.SetInteger("Walk", 2);
        }

        else if (targetPosition.x - offset > parent.transform.position.x)
        {
            newScale.x = 1;
            parent.transform.localScale = newScale;

            parent.rigidbody.velocity = new Vector2(parent.moveSpeed, 0.0f);
            parent.animator.SetInteger("Walk", 2);
        }
    }

}
