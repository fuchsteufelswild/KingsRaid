using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public NPC parent;

    public State(GameObject sParent)
    {
        parent = sParent.GetComponent<NPC>();
    }

    public virtual void PerformAction()
    {

    }
}
