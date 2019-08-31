using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : NPC
{
    public SpotEnemy detector;
    public static int idCounter = 0;
    public int id;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        id = idCounter++;
        Companion tComp = this.gameObject.GetComponent<Companion>();
        HeaderUI tHeader = CanvasScript.instance.headerUI.GetComponent<HeaderUI>();
        tHeader.companionIcons[tComp.id].gameObject.SetActive(true);
        // tHeader.companionFills[tComp.id].gameObject.SetActive(true);
        tHeader.images[tComp.id].gameObject.SetActive(true);
        tHeader.companionIcons[tComp.id].sprite = tComp.actorIcon;
        // tHeader.companionFills[tComp.id].normalizedValue = tComp.health / this.maxHealth;

        detector = GetComponentInChildren<SpotEnemy>();

        this.actorType = NPC.ActorType.COMPANION;
        typesToTarget.Add(NPC.ActorType.MOB);
        
        AttackAreaDamage temp = hitPoint.GetComponent<AttackAreaDamage>();
        temp.damage = this.damage;
        this.SetState(new IdleState(this.gameObject));

    }


    protected override void Update()
    {
        base.Update();
    }
}
