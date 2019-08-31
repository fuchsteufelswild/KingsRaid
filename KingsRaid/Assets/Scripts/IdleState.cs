using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(GameObject sParent) : base(sParent) {  }

    public override void PerformAction()
    {
        Vector3 targetPos = Character.instance.transform.position;
        Vector3 newScale = parent.transform.localScale;
        if (targetPos.x < parent.transform.position.x)
            newScale.x = -1;
        else
            newScale.x = 1;

        if (parent.transform.position.x + 2f <= targetPos.x)
        {
            newScale.x = 1;
            parent.transform.localScale = newScale;

            parent.rigidbody.velocity = new Vector2(parent.moveSpeed, 0.0f);
            parent.animator.SetInteger("Walk", 2);
        }
        else if(parent.transform.position.x - 2f >= targetPos.x)
        {
            newScale.x = -1;
            parent.transform.localScale = newScale;

            parent.rigidbody.velocity = new Vector2(-parent.moveSpeed, 0.0f);
            parent.animator.SetInteger("Walk", 2);
        }
        else
        {
            parent.rigidbody.velocity = new Vector2(0.0f, 0.0f);
            parent.animator.SetInteger("Walk", 0);
        }
    }
}
