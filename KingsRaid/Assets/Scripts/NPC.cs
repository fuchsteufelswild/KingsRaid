using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Actor
{
    public Color alternativeRGB = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    public enum ActorType { PLAYER, COMPANION, MOB }
    public enum NPCType { MELEE, RANGED, MAGE, PRIEST }

    public float patrolLeftAmount;
    public float patrolRightAmount;

    public float projectileSpawnTime = 1.0f;
    public Transform attackPoint;
    public Transform shootPoint;
    public GameObject projectile = null;
    public GameObject hitPoint; // Object to be spawned for melee attack hits all target Actors who fall in Physics2D overlap

    public NPCType type;
    public State state;

    public int coinToDrop;
    public int damage;
    public int level;

    public float attackTimePassed = 0.0f;
    public float attackCooldown;
    public float attackTime = 3.0f;
    public float attackTimeCounter = 0.0f;

    public Actor lockedTarget;

    public Vector3 patrolLeft;
    public Vector3 patrolRight;

    public bool patrollingLeft = true;
    public bool attacking = false;

    public Rigidbody2D rigidbody;
    public Animator animator;

    public List<ActorType> typesToTarget;

    public AudioClip clipToPlay;
    //public AudioSource audio;

    protected override void Awake()
    {
        base.Awake();
        damage += 5 * level;
        typesToTarget = new List<ActorType>();

        rigidbody = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        this.audioSource = this.GetComponent<AudioSource>();
        this.spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    public IEnumerator WaitForOpponentCheck()
    {
        yield return new WaitForSeconds(2f);

        CheckForOpponent();
    }

    public void OpponentChecker()
    {
        StartCoroutine(WaitForOpponentCheck());
    }

    public void CheckForOpponent()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(patrolLeftAmount, patrolRightAmount), 0f);

        float minDistance = 1000f;
        Collider2D pickedOpponent = null;
        float temp;
        foreach(Collider2D collider in colliders)
        {

            bool ready = false;

            foreach(ActorType t in typesToTarget)
            {
                if (collider.GetComponent<Actor>() != null && t == collider.GetComponent<Actor>().actorType)
                    ready = true;
            }

            if (!ready)
                continue;

            temp = Mathf.Abs(this.transform.position.x - collider.transform.position.x);
            if ( temp < minDistance)
            {
                minDistance = temp;
                pickedOpponent = collider;
            }
        }

        if (pickedOpponent != null)
            this.lockedTarget = pickedOpponent.GetComponent<Actor>();
        else
        {
            this.lockedTarget = null;
            if (this.actorType == ActorType.COMPANION)
                this.SetState(new IdleState(this.gameObject));
            else if (this.actorType == ActorType.MOB)
                this.SetState(new PatrolState(this.gameObject));
        }
    }
    public void FinishAttack()
    {
        GameObject tempObject = null;
        if (type == NPCType.MELEE)
            tempObject = Instantiate(hitPoint, new Vector3(attackPoint.transform.position.x, attackPoint.transform.position.y, attackPoint.transform.position.z), Quaternion.identity) as GameObject;
        else if (type == NPCType.RANGED)
            tempObject = Instantiate(projectile, new Vector3(shootPoint.transform.position.x, shootPoint.transform.position.y, shootPoint.transform.position.z), Quaternion.identity) as GameObject;

        if(type == NPCType.RANGED)
        {
            Projectile tProjectile = tempObject.GetComponent<Projectile>();
            tProjectile.owner = this;
            tProjectile.damage = (int)(this.damage * (1f + GameManager.instance.currentLevel * 0.3f));
            Vector3 newLocalScale = tempObject.transform.localScale;
            if (this.transform.localScale.x > 0)
            {
                if (tProjectile != null && tProjectile.xScale != 0)
                    newLocalScale.x = this.transform.localScale.x + tProjectile.xScale;
                else
                    newLocalScale.x = this.transform.localScale.x - 0.3f;
            }
            else
            {
                if (tProjectile != null && tProjectile.xScale != 0)
                    newLocalScale.x = this.transform.localScale.x - tProjectile.xScale;
                else
                    newLocalScale.x = this.transform.localScale.x + 0.3f;
            }
            tempObject.transform.localScale = newLocalScale;
            tempObject.GetComponent<Projectile>().SetSpeed(this.transform.localScale.x * 7.0f);

            if (this.actorType == ActorType.MOB)
            {
                tProjectile.typesToAttack.Add("Player");
                tProjectile.typesToAttack.Add("Companion");
            }
            else if(this.actorType == ActorType.COMPANION)
            {
                tProjectile.typesToAttack.Add("Enemy");
                tProjectile.typesToAttack.Add("Enemy");
            }
        }
        else if(type == NPCType.MELEE)
        {
            AttackAreaDamage area = tempObject.GetComponent<AttackAreaDamage>();
            area.owner = this;
            area.damage = (int)(this.damage * (1f + GameManager.instance.currentLevel * 0.3f));

            area.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

            if (this.actorType == ActorType.MOB)
            {
                area.typesToAttack.Add("Player");
                area.typesToAttack.Add("Companion");
            }
            else if (this.actorType == ActorType.COMPANION)
            {
                area.typesToAttack.Add("Enemy");
                area.typesToAttack.Add("Enemy");
            }
        }
        
        if (!alternativeRGB.a.Equals(0.0f))
        {
            tempObject.GetComponentInChildren<SpriteRenderer>().color = alternativeRGB;
        }
        

        audioSource.PlayOneShot(clipToPlay);
        
    }

    public override void Attack()
    {
        this.rigidbody.velocity = new Vector2(0.0f, 0.0f);
        animator.SetInteger("Walk", 0);
        
        // StartCoroutine(AttackMiddleStage());
    }

    IEnumerator AttackMiddleStage()
    {
        yield return new WaitForSeconds(projectileSpawnTime);

        FinishAttack();
    }


    public override bool TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if(this.health <= 0)
        {
            this.rigidbody.velocity = new Vector2(0f, 0f);
            this.Diee();
            return true;
        }

        return false;
    }

    public void Diee()
    {
        this.rigidbody.velocity = new Vector2(0.0f, 0.0f);
        attacking = true;
        attackTimeCounter = 0.0f;
        this.animator.SetTrigger("Die");
        if (this.actorType == ActorType.COMPANION)
        {
            Character.instance.Unsubscribe((Companion)this);
            Destroy(this.gameObject);
        }
    }

    protected virtual void Update()
    {
        // this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, GameManager.instance.mapBounds.x, GameManager.instance.mapBounds.y), this.transform.position.y, this.transform.position.z);
        attackTimeCounter += Time.deltaTime;
        if(attackTimeCounter >= attackTime)
        {
            attacking = false;
            attackTimeCounter = 0.0f;
        }
        if (attacking)
            return;


        attackTimePassed += Time.deltaTime;

        state.PerformAction();
    }


    public void SetState(State newState)
    {
        if(this.state != null)
            this.state.parent = null;
        animator.SetInteger("Walk", 0);
        patrolLeft = this.transform.position + new Vector3(-patrolLeftAmount, 0.0f, 0.0f);
        patrolRight = this.transform.position + new Vector3(patrolRightAmount, 0.0f, 0.0f);

        this.state = newState;
    }
}
