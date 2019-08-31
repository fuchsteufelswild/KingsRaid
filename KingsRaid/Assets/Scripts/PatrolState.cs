using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public PatrolState(GameObject sParent) : base(sParent) { }

    public void MoveToTarget(Vector3 targetPosition)
    {
        Vector3 newScale = parent.transform.localScale;
        if (targetPosition.x <= parent.transform.position.x)
        {
            newScale.x = -1;
            parent.transform.localScale = newScale;

            parent.rigidbody.velocity = new Vector2(-parent.moveSpeed, 0.0f);
            parent.animator.SetInteger("Walk", 2);
        }
        else if (targetPosition.x > parent.transform.position.x)
        {
            newScale.x = 1;
            parent.transform.localScale = newScale;

            parent.rigidbody.velocity = new Vector2(parent.moveSpeed, 0.0f);
            parent.animator.SetInteger("Walk", 2);
        }
    }

    public override void PerformAction()
    {

        if (parent.transform.position.x - 0.5f <= parent.patrolLeft.x || parent.transform.position.x - 0.2f <= GameManager.instance.mapBounds.x)
        {
            parent.patrollingLeft = false;
        }
        else if (parent.transform.position.x + 0.5f >= parent.patrolRight.x || parent.transform.position.x + 0.2f >= GameManager.instance.mapBounds.y)
        {
            parent.patrollingLeft = true;
        }

        if (parent.patrollingLeft)
            MoveToTarget(parent.patrolLeft);
        else
            MoveToTarget(parent.patrolRight);
    }
}
