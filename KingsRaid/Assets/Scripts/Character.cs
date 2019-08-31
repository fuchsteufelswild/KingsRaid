using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : Actor
{
    [Header("Character Spesific")]
    public Item weapon = null;
    public Item helmet = null;
    public Item chestArmor = null;
    public Item shoes = null;
    public Transform weaponTransform;
    public Transform skillTransform;
    public int InventorySize = 28;
    public int coin = 100;

    public Sprite testIcon;
    public Sprite testRenderableForm;

    public GameObject toBeInteracted;
    public bool toInteract = false;

    public static Character instance;

    public List<Companion> companions;

    public int firstEmptyItemSlot = 0;

    public Transform rangedAttack; // Where projectiles will be instantiated
    public Transform meleeAttack; // Where attack collider will be spawned

    public bool attackUsed = true;

    public bool isAttacking = false;

    public GameObject meleeAttackArea;

    public bool blockInput = false;
    public bool started = false;

    //public AudioSource audioSource;
    public AudioClip[] panelAudioClips;

    public void AddItem(Item newItem)
    {
        items[firstEmptyItemSlot] = newItem;
        ++firstEmptyItemSlot;
    }

    public void RemoveItem(Item oldItem)
    {
        for (int i = 0; i < InventorySize; ++i)
        {
            if (oldItem == items[i])
            {
                if(firstEmptyItemSlot > i)
                    firstEmptyItemSlot = i;
                items[i] = null;
            }
        }
    }

    public override bool TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if(CanvasScript.instance.characterPanel.activeInHierarchy)
            CanvasScript.instance.characterPanel.GetComponent<CharacterStats>().health.text = "Health: " + this.health;

        if (this.health <= 0)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            this.isAttacking = true;
            this.mAnimator.SetTrigger("Die");
            return true;
        }

        return false;
    }

    public override void FinishDeath()
    {
        Destroy(this.gameObject);
        CanvasScript.instance.gameOver.GetComponent<LoadingScreen>().One();
        EndGame();
    }

    private void EndGame()
    {
        StartCoroutine(Waiter());
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(4f);
        Application.Quit();
    }


    // Bind companions to character
    public void Subscribe(Companion companion)
    {
        
        Companion newCompanion = Instantiate(companion, this.transform.position, Quaternion.identity) as Companion;
        companions.Add(newCompanion);
    }

    public void Unsubscribe(Companion companion)
    {
        companions.Remove(companion);
    }

    // sID denotes command type
    public void Notify(int sID)
    {
        switch (sID)
        {
            case 0:
            {
                foreach (Companion companion in companions)
                    if(companion.lockedTarget == null)
                        companion.SetState(new PatrolState(companion.gameObject));
                break;
            }
            case 1:
            {
                foreach (Companion companion in companions)
                {
                    companion.lockedTarget = null;
                    companion.SetState(new IdleState(companion.gameObject));
                }
                break;
            }
            default:
                break;
        }
    }

    public void Teleport(Vector3 targetPos)
    {
        this.transform.position = targetPos;

        foreach(Companion companion in companions)
        {
            companion.transform.position = new Vector3(targetPos.x - 0.2f, targetPos.y + 0.5f, targetPos.z);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        this.items = new Item[InventorySize];
        this.actorType = NPC.ActorType.PLAYER;
        this.audioSource = this.GetComponent<AudioSource>();
        // this.feetAudioSource = this.GetComponentInChildren<AudioSource>();

        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        CanvasScript.instance.headerUI.GetComponent<HeaderUI>().actorIcon.sprite = Character.instance.actorIcon;
        // CanvasScript.instance.headerUI.GetComponent<HeaderUI>().actorSlider.normalizedValue = Character.instance.health / 100.0f;
    }

    

    public void FixedUpdate()
    {
        
    }
    public override void ToggleInventory()
    {
        GameObject inventoryPanel = CanvasScript.instance.inventoryPanel;
        inventoryPanel.SetActive(!inventoryPanel.gameObject.activeInHierarchy);
        if (!inventoryPanel.gameObject.activeInHierarchy)
        {
            this.audioSource.PlayOneShot(panelAudioClips[3]);


            if (CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>().type == 0)
                CanvasScript.instance.itemInfo.SetActive(false);
            return;
        }
        else
        {
            this.audioSource.PlayOneShot(panelAudioClips[2]);
        }
        
        UpdateInventory();
    }

    public override void UpdateInventory()
    {
        CanvasScript.instance.inventoryPanel.GetComponent<InventoryPanel>().coin.text = "Coin: \n" + this.coin.ToString();

        GUIItem[] UIitems = CanvasScript.instance.inventoryPanel.GetComponent<UIPanels>().itemHolders;

        for (int i = 0; i < InventorySize; ++i)
            UIitems[i].SetItem(items[i]);
    }

    public void UpdateCharacterWindow()
    {

        GUIItem[] UIitems = CanvasScript.instance.characterPanel.GetComponent<UIPanels>().itemHolders;

        UIitems[0].SetItem(this.helmet);
        UIitems[1].SetItem(this.weapon);
        UIitems[2].SetItem(this.chestArmor);
        UIitems[3].SetItem(this.shoes);

        CharacterStats st = CanvasScript.instance.characterPanel.GetComponent<CharacterStats>();
        st.health.text = "Health: " + this.health;
        if (this.weapon != null)
            st.attack.text = "Attack: " + this.weapon.GetDamage();
        else
            st.attack.text = "Attack: " + 0; 
        st.defence.text = "Defence: " + this.defence;
    }

    public override void Attack()
    {
        if (weapon != null)
        {
            attackUsed = true;
            Character.instance.mAnimator.SetInteger("Walk", 0);
            isAttacking = true;
            if (((Weapon)weapon).attackType == Weapon.ClassType.BOW)
                this.GetComponent<Animator>().SetTrigger("BowAttack");
            else
                this.mAnimator.SetTrigger("SwordAttack");
        }
    }


    public void FinishAttack()
    {
        
        if (this.weapon != null && ((Weapon)weapon).attackType == Weapon.ClassType.BOW)
        {

            if (attackUsed)
            {
                this.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(6));
                weapon.Attack(rangedAttack);
            }
            else
            {
                this.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(6));
                weapon.UseSkill(rangedAttack);
            }
        }
        else if(this.weapon != null)
        {

            if (attackUsed)
            {
                this.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(7));
                weapon.Attack(meleeAttack);
            }
            else
            {
                this.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(9));
                weapon.UseSkill(meleeAttack);
            }
        }
        isAttacking = false;
    }

    public override void SetWeapon(Item newWeapon)
    {
        if (this.weapon != null)
        {
            this.weapon.owner = null;
            this.AddItem(this.weapon);
        }

        if (((Weapon)newWeapon).attackType == Weapon.ClassType.BOW)
            this.audioSource.PlayOneShot(panelAudioClips[7]);
        else
            this.audioSource.PlayOneShot(panelAudioClips[6]);

        this.weapon = newWeapon;
        Character.instance.attack = this.weapon.GetDamage();
        newWeapon.owner = this;
        UpdateInventory();
        UpdateCharacterWindow();
    }

    public override void UnsetWeapon(Item oldWeapon)
    {
        this.weapon.owner = null;
        this.weapon = null;
        UpdateCharacterWindow();
    }

    public override void UnsetArmor(Item oldArmor)
    {
        Armor.ClassType classType = ((Armor)oldArmor).type;

        switch (classType)
        {
            case Armor.ClassType.HELMET:
                {
                    this.helmet.owner = null;
                    this.helmet = null;
                    break;
                }
            case Armor.ClassType.CHEST:
                {
                    this.chestArmor.owner = null;
                    this.chestArmor = null;
                    break;
                }
            case Armor.ClassType.SHOES:
                {
                    this.shoes.owner = null;
                    this.shoes = null;
                    break;
                }
        }

        UpdateCharacterWindow();
    }

    public override void ToggleCharacterWindow()
    {
        GameObject characterWindow = CanvasScript.instance.characterPanel;
        characterWindow.SetActive(!characterWindow.gameObject.activeInHierarchy);
        if (!characterWindow.gameObject.activeInHierarchy)
        {
            this.audioSource.PlayOneShot(this.panelAudioClips[3]);
            if (CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>().type == 3)
                CanvasScript.instance.itemInfo.SetActive(false);
            return;
        }
        else
        {
            this.audioSource.PlayOneShot(this.panelAudioClips[2]);
        }

        UpdateCharacterWindow();
    }
    public override void SetArmor(Item newArmor)
    {
        Armor.ClassType classType = ((Armor)newArmor).type;
        Character.instance.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(3));
        switch (classType)
        {
            case Armor.ClassType.HELMET:
                {
                    if(this.helmet != null)
                    {
                        Character.instance.defence -= ((Armor)this.helmet).baseDefence;
                        this.AddItem(this.helmet);
                        this.helmet = null;
                        
                    }
                    this.helmet = newArmor;
                    Character.instance.defence += ((Armor)newArmor).baseDefence;
                    break;
                }
            case Armor.ClassType.CHEST:
                {
                    if (this.chestArmor != null)
                    {
                        Character.instance.defence -= ((Armor)this.chestArmor).baseDefence;
                        this.AddItem(this.chestArmor);
                        this.chestArmor = null;
                    }
                    this.chestArmor = newArmor;
                    Character.instance.defence += ((Armor)newArmor).baseDefence;
                    break;
                }
            case Armor.ClassType.SHOES:
                {
                    if (this.shoes != null)
                    {
                        Character.instance.defence -= ((Armor)this.shoes).baseDefence;
                        this.AddItem(this.shoes);
                        this.shoes = null;
                    }
                    this.shoes = newArmor;
                    Character.instance.defence += ((Armor)newArmor).baseDefence;
                    break;
                }
        }

        newArmor.owner = this;
        UpdateInventory();
        UpdateCharacterWindow();
    }

    public override void UseSkill()
    {
        if (weapon != null)
        {
            attackUsed = false;
            Character.instance.mAnimator.SetInteger("Walk", 0);
            isAttacking = true;
            if (((Weapon)weapon).attackType == Weapon.ClassType.BOW)
                this.GetComponent<Animator>().SetTrigger("BowAttack");
            else
                this.mAnimator.SetTrigger("SwordAttack");
        }
    }

    public override void Die()
    {
        base.Die();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < InventorySize; ++i)
            items[i] = null;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        if (!this.started)
            return;
        Vector3 newPos = new Vector3(Mathf.Clamp(this.transform.position.x, GameManager.instance.mapBounds.x + 6.3f, GameManager.instance.mapBounds.y - 6.15f), this.transform.position.y + 1.5f, Camera.main.transform.position.z);

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPos, 100 * Time.deltaTime);
    }
}
