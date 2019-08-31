using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotEnemy : MonoBehaviour
{
    public NPC parent;
    public string[] toSpot;
    // Start is called before the first frame update
    void Start()
    {
        parent = this.GetComponentInParent<NPC>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (parent.lockedTarget != null || (collision.tag != toSpot[0] && collision.tag != toSpot[1]))
            return;

        parent.lockedTarget = collision.GetComponent<Actor>();
        parent.SetState(new DistractedState(this.parent.gameObject));
    }
}
