using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Essential Components
    private Character mActor;
    private Rigidbody2D mRigidbody2D;

    public AudioSource background;
    public AudioClip[] backgroundClips;
    public int current = 0;

    // Dynamics
    [Header("Dynamics")]
    public bool isFacingRight;
    public float attackTransitionTime;
    public float animationTransitionTime;
    public float timePassed;
    public float attackWaitTime = 0.5f;
    public float skillWaitTime = 0.5f;
    public float attackWaitTimePassed = 1.0f;
    public float skillWaitTimePassed = 1.0f;

    public bool orderOpen = false;

    [Header("Jump Dynamics")]
    public bool isGrounded;
    public Transform groundChecker;
    public float groundCheckRadius;
    public LayerMask groundMask;

    private OrderScript tOrder;

    private void Start()
    {
        this.mActor = this.GetComponent<Character>();
        this.mRigidbody2D = this.GetComponent<Rigidbody2D>();
        tOrder = CanvasScript.instance.order.GetComponent<OrderScript>();
    }

    
    private void ProcessInputs()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasScript.instance.inventoryPanel.gameObject.SetActive(false);
            CanvasScript.instance.characterPanel.gameObject.SetActive(false);
            CanvasScript.instance.NPCPanel.gameObject.SetActive(false);
            CanvasScript.instance.containerPanel.SetActive(false);
            CanvasScript.instance.NPCArmorPanel.gameObject.SetActive(false);
            CanvasScript.instance.potionPanel.gameObject.SetActive(false);
            CanvasScript.instance.hints.gameObject.SetActive(false);
            CanvasScript.instance.companionShopPanel.gameObject.SetActive(false);
            CanvasScript.instance.itemInfo.gameObject.SetActive(false);
            CanvasScript.instance.companionInfo.gameObject.SetActive(false);
            CanvasScript.instance.hintPrompt.gameObject.SetActive(false);
            CanvasScript.instance.prompt.gameObject.SetActive(false);
            CanvasScript.instance.travelPrompt.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.C))
            mActor.ToggleCharacterWindow();
        if (Input.GetKeyDown(KeyCode.I))
            mActor.ToggleInventory();

        if (Input.GetKeyDown(KeyCode.W))
            if (isGrounded)
                mRigidbody2D.AddForce(new Vector2(0.0f, mActor.jumpForce));
        
        

        if(orderOpen)
        {
            if(Input.GetKeyDown(KeyCode.F1))
            {
                for (int i = 0; i < tOrder.orders.Length; ++i)
                    if(i != 0)
                        tOrder.orders[i].FadeO(1f);

                tOrder.orders[0].indicator.Blink();
                tOrder.orders[0].FadeO(3f);

                orderOpen = false;

                Character.instance.Notify(0);
            }
            else if(Input.GetKeyDown(KeyCode.F2))
            {
                for (int i = 0; i < tOrder.orders.Length; ++i)
                    if (i != 1)
                        tOrder.orders[i].FadeO(1f);

                tOrder.orders[1].indicator.Blink();
                tOrder.orders[1].FadeO(3f);

                orderOpen = false;

                Character.instance.Notify(1);
            }
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            
            if (orderOpen)
            {
                orderOpen = false;
                foreach(FadeOut fadeOut in tOrder.orders)
                {
                    fadeOut.ZeroOut();
                }
            }
            else
            {
                orderOpen = true;
                foreach (FadeOut fadeOut in tOrder.orders)
                {
                    fadeOut.Refresh();
                }
            }
        }

        
        if (mActor.toInteract)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!CanvasScript.instance.inventoryPanel.activeInHierarchy)
                    Character.instance.ToggleInventory();

                Container toInteractContainer = Character.instance.toBeInteracted.GetComponent<Container>();


                if (!toInteractContainer && !GameManager.instance.openStore)
                {
                    mActor.audioSource.PlayOneShot(mActor.panelAudioClips[1]);
                    GameManager.instance.openStore = Character.instance.toBeInteracted.GetComponent<StoreScript>();
                    mActor.toBeInteracted.GetComponent<StoreScript>().SetImageActivity(true);
                }
                else if (!GameManager.instance.openStore && !GameManager.instance.openContainer)
                {
                    GameManager.instance.openContainer = Character.instance.toBeInteracted.GetComponent<Container>(); 
                    mActor.audioSource.PlayOneShot(mActor.panelAudioClips[4]);
                    mActor.toBeInteracted.GetComponent<Container>().SetImageActivity(true);
                }
                else if(GameManager.instance.openContainer != null)
                {
                    mActor.audioSource.PlayOneShot(mActor.panelAudioClips[5]);
                    mActor.toBeInteracted.GetComponent<Container>().SetImageActivity(false);

                    if (CanvasScript.instance.itemInfo.activeInHierarchy && CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>().type == 1)
                        CanvasScript.instance.itemInfo.SetActive(false);

                    GameManager.instance.openContainer = null;
                }
                else if(GameManager.instance.openStore != null)
                {
                    mActor.audioSource.PlayOneShot(mActor.panelAudioClips[0]);
                    mActor.toBeInteracted.GetComponent<StoreScript>().SetImageActivity(false);

                    if (CanvasScript.instance.itemInfo.activeInHierarchy && CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>().type == 2)
                        CanvasScript.instance.itemInfo.SetActive(false);

                    GameManager.instance.openStore = null;
                }
            }
        }
        else
        {
            if (GameManager.instance.toTravel && Input.GetKeyDown(KeyCode.F))
            {
                CanvasScript.instance.loadingScreen.GetComponent<LoadingScreen>().Perform();
                mRigidbody2D.velocity = new Vector2(0.0f, 0.0f);
                Character.instance.mAnimator.SetInteger("Walk", 0);
                switch (GameManager.instance.travelID)
                {
                    case 0:
                        foreach (Companion companion in Character.instance.companions)
                            companion.Regenerate(companion.maxHealth, Potion.PotionType.HEALTH);
                        GameManager.instance.currentLevel += 1;
                        foreach (StoreScript store in GameManager.instance.stores)
                            store.ChangeContainerContent(GameManager.instance.currentLevel);
                        GameManager.instance.mapBounds.x = -10.99f;
                        GameManager.instance.mapBounds.y = 8.85f;
                        foreach (GameObject obj in LevelGenerator.instance.containers)
                            Destroy(obj);
                        Character.instance.Teleport(new Vector3(-0.14f, -4.74f, Character.instance.transform.position.z));
                        break;
                    case 1:
                        CanvasScript.instance.levelAppear.GetComponent<LoadingScreen>().text1.text = GameManager.instance.currentLevel.ToString();
                        CanvasScript.instance.levelAppear.GetComponent<LoadingScreen>().Perform();
                        for (float i = 14; (int)i < 114; i += 1.5f)
                        {
                            GameObject temp = Instantiate(GameObjectContainers.instance.GetMobPrefab(Random.Range(0, 2)), new Vector3(i, 19.643f, -2.0f), Quaternion.identity) as GameObject;
                            LevelGenerator.instance.mobs.Add(temp);
                        }
                        LevelGenerator.instance.leavePrompt.SetActive(false);
                        GameManager.instance.mapBounds.x = -9.7f;
                        GameManager.instance.mapBounds.y = 112.38f;
                        Character.instance.Teleport(new Vector3(-6.45f, 19.56f, Character.instance.transform.position.z));
                        break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            CanvasScript.instance.hintPrompt.SetActive(false);
            CanvasScript.instance.hints.SetActive(!CanvasScript.instance.hints.activeInHierarchy);
        }

        if (Character.instance.isAttacking || GameManager.instance.openContainer || GameManager.instance.openStore || CanvasScript.instance.inventoryPanel.activeInHierarchy)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (attackWaitTimePassed >= attackWaitTime)
            {
                if(Character.instance.weapon != null)
                    mActor.isAttacking = true;
                Character.instance.mAnimator.SetInteger("Walk", 0);
                mRigidbody2D.velocity = new Vector2(0.0f, 0.0f);
                attackWaitTimePassed = 0.0f;
                mActor.Attack();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (skillWaitTimePassed >= skillWaitTime)
            {
                if (Character.instance.weapon != null)
                    mActor.isAttacking = true;
                Character.instance.mAnimator.SetInteger("Walk", 0);
                mRigidbody2D.velocity = new Vector2(0.0f, 0.0f);
                skillWaitTimePassed = 0.0f;
                mActor.UseSkill();
            }
        }

    }

    private void FixedUpdate()
    {
        if (!Character.instance.started || Character.instance.blockInput)
            return;

        Character.instance.transform.position = new Vector3(Mathf.Clamp(Character.instance.transform.position.x, GameManager.instance.mapBounds.x + 0.3f , GameManager.instance.mapBounds.y), Character.instance.transform.position.y, Character.instance.transform.position.y);
        if (Character.instance.isAttacking)
            return;

        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, groundMask);

        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        mRigidbody2D.velocity = new Vector2(horizontalAxis * mActor.moveSpeed, mRigidbody2D.velocity.y);

        if(CanvasScript.instance.welcomeText.activeInHierarchy && Mathf.Abs(horizontalAxis) > 0)
        {
            CanvasScript.instance.welcomeText.SetActive(false);
            CanvasScript.instance.hintPrompt.SetActive(false);
        }

        if ((mRigidbody2D.velocity.x > 0 && !isFacingRight) ||
            mRigidbody2D.velocity.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 newLocalScale = this.transform.localScale;
            newLocalScale.x *= -1;
            this.transform.localScale = newLocalScale;
        }
        if (Mathf.Abs(mRigidbody2D.velocity.x) == 0)
        {
            Character.instance.mAnimator.SetInteger("Walk", 0);
        }
        else
        {
            Character.instance.mAnimator.SetInteger("Walk", 2);
        }
    }
    
    void Update()
    {
        if (!Character.instance.started || Character.instance.blockInput)
            return;
        timePassed += Time.deltaTime;
        attackWaitTimePassed += Time.deltaTime;
        skillWaitTimePassed += Time.deltaTime;

        ProcessInputs();
    }
}
