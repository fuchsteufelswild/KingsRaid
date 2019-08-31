using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [Header("Basic Actor Stats")]
    public float health;
    public float stamina;
    public float mana;
    public string actorName;
    public float maxHealth;

    [Header("Actor Attributes")]
    public int defence;
    public int attack;
    public int attackRate;
    public Item[] items;

    [Header("Physics Actor Stats")]
    public float moveSpeed;
    public float jumpForce;

    public NPC.ActorType actorType;

    public Animator mAnimator;
    public AudioSource audioSource;
    public AudioSource feetAudioSource;

    public AudioClip[] stepClips;

    public int stepCount = 0;

    protected SpriteRenderer spriteRenderer;
    public Sprite actorIcon;
    public virtual bool TakeDamage(float damage)
    {
        if (damage > defence && (damage - defence) > 0)
            health -= (damage - defence);
        if (health < 0)
            health = 0.0f;

        Companion tComp = this.gameObject.GetComponent<Companion>();
        if (tComp != null)
        {
            HeaderUI headerUI = CanvasScript.instance.headerUI.GetComponent<HeaderUI>();
            RectTransform rTransform = headerUI.companionFills[tComp.id].rectTransform;
            float lackAmount = 1 - this.health / this.maxHealth;
            headerUI.companionFills[tComp.id].transform.localPosition = new Vector3(-122.42f * lackAmount * 0.5f, 0.0f, 0.0f);
            rTransform.sizeDelta = new Vector2(122.42f * (1 - lackAmount), rTransform.sizeDelta.y);

            headerUI.companionIcons[tComp.id].sprite = tComp.actorIcon;
            // CanvasScript.instance.headerUI.GetComponent<HeaderUI>().companionSliders[tComp.id].normalizedValue = tComp.health / tComp.maxHealth;
        }
        else if(this == Character.instance)
        {
            HeaderUI headerUI = CanvasScript.instance.headerUI.GetComponent<HeaderUI>();
            RectTransform rTransform = headerUI.actorFill.rectTransform;
            float lackAmount = 1 - this.health / this.maxHealth;
            headerUI.actorFill.transform.localPosition = new Vector3(-122.42f * lackAmount * 0.5f, 0.0f, 0.0f);
            rTransform.sizeDelta = new Vector2(122.42f * (1 - lackAmount), rTransform.sizeDelta.y);
            headerUI.actorIcon.sprite = Character.instance.actorIcon;
            // CanvasScript.instance.headerUI.GetComponent<HeaderUI>().actorSlider.normalizedValue = Character.instance.health / 100.0f;
        }
        else
        {
            Mob tMob = this.gameObject.GetComponent<Mob>();
            RectTransform rTransform = tMob.healthBarFill.rectTransform;
            float lackAmount = 1 - this.health / tMob.maxHealth;
            tMob.healthBarFill.transform.localPosition = new Vector3(-1 * lackAmount * 0.5f, 0.0f, 0.0f);
            rTransform.sizeDelta = new Vector2(1 - lackAmount, rTransform.sizeDelta.y);
        }

        return false;
    }


    public void PlaySound()
    {
        if (this.actorType == NPC.ActorType.MOB)
            return;

        this.feetAudioSource.PlayOneShot(this.stepClips[stepCount]);
        ++stepCount;
        if (stepCount >= this.stepClips.Length)
            stepCount = 0;

    }

    public virtual void Die()
    {
        
        this.mAnimator.SetTrigger("Die");

    }

    public virtual void FinishDeath()
    {
        //
    }

    public void EnableAudioSource()
    {
        this.feetAudioSource.gameObject.SetActive(true);
    }

    public void DisableAudioSource()
    {
        this.feetAudioSource.gameObject.SetActive(false);
    }

    public virtual void Attack()
    {
        // Handle Attack
    }

    public virtual void UseSkill()
    {
        // Handle Skill Use
    }

    public virtual void ToggleInventory()
    {
        // Handle
    }

    public virtual void ToggleCharacterWindow() {
        // 
    } 
    public virtual void UpdateInventory()
    {

    }

    public virtual void EquipItem(Item newItem)
    {
        if (newItem.itemType == Item.ItemType.WEAPON)
            SetWeapon(newItem);
        else if (newItem.itemType == Item.ItemType.ARMOR)
            SetArmor(newItem);
    }
    public virtual void UnequipItem(Item oldItem)
    {
        if (oldItem.itemType == Item.ItemType.WEAPON)
            UnsetWeapon(oldItem);
        else if (oldItem.itemType == Item.ItemType.ARMOR)
            UnsetArmor(oldItem);
    }

    public virtual void SetWeapon(Item newWeapon) { }
    public virtual void SetArmor(Item newArmor) { }

    public virtual void UnsetWeapon(Item oldWeapon) { }
    public virtual void UnsetArmor(Item oldArmor) { }

    protected virtual void Awake()
    {
        this.mAnimator = this.GetComponent<Animator>();
        this.spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    public void Regenerate(float regenerationValue, Potion.PotionType potType)
    {
        switch(potType)
        {
            case Potion.PotionType.HEALTH:
                this.health = Mathf.Clamp(health + regenerationValue, 0f, this.maxHealth);
                Companion tComp = this.gameObject.GetComponent<Companion>();
                if (tComp != null)
                {
                    HeaderUI headerUI = CanvasScript.instance.headerUI.GetComponent<HeaderUI>();
                    RectTransform rTransform = headerUI.companionFills[tComp.id].rectTransform;
                    float lackAmount = 1 - this.health / this.maxHealth;
                    headerUI.companionFills[tComp.id].transform.localPosition = new Vector3(-122.42f * lackAmount * 0.5f, 0.0f, 0.0f);
                    rTransform.sizeDelta = new Vector2(122.42f * (1 - lackAmount), rTransform.sizeDelta.y);
                    headerUI.companionIcons[tComp.id].sprite = tComp.actorIcon;
                }
                else if (this == Character.instance)
                {
                    HeaderUI headerUI = CanvasScript.instance.headerUI.GetComponent<HeaderUI>();
                    RectTransform rTransform = headerUI.actorFill.rectTransform;
                    float lackAmount = 1 - this.health / this.maxHealth;
                    headerUI.actorFill.transform.localPosition = new Vector3(-122.42f * lackAmount * 0.5f, 0.0f, 0.0f);
                    rTransform.sizeDelta = new Vector2(122.42f * (1 - lackAmount), rTransform.sizeDelta.y);
                    headerUI.actorIcon.sprite = Character.instance.actorIcon;
                    if (CanvasScript.instance.characterPanel.activeInHierarchy)
                        CanvasScript.instance.characterPanel.GetComponent<CharacterStats>().health.text = "Health: " + this.health;
                }
                break;
            case Potion.PotionType.MANA:
                break;
            case Potion.PotionType.STAMINA:
                break;
            default:
                break;
        }
    }


    public void AddPostEffect(PostEffect newPostEffect)
    {
        StartCoroutine(PostEffectTimer(newPostEffect));
    }

    IEnumerator PostEffectTimer(PostEffect postEffect)
    {
        PostEffect.PostEffectType postEffectType = postEffect.type;

        switch(postEffectType)
        {
            case PostEffect.PostEffectType.BURN:
            {
                for(int i = 0; i < 3; ++i)
                {
                        Color oldColor = spriteRenderer.color;
                    postEffect.PerformEffect();
                    yield return new WaitForSeconds(0.3f);
                        spriteRenderer.color = oldColor;
                        yield return new WaitForSeconds(0.8f);

                    
                }
                    break;
            }
            case PostEffect.PostEffectType.FREEZE:
                {
                    postEffect.PerformEffect();
                    yield return new WaitForSeconds(2f);
                }
                break;
            default:
                break;
        }

        postEffect.RevertEffect();

    }
}
