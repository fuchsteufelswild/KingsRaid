using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreScript : MonoBehaviour
{
    [Header("Container Essentials")]
    public Sprite renderableForm;
    public List<Item> items;
    public string containerName;
    public int containerSize = 16;

    public Sprite testRenderableForm;

    public enum ShopType { POTION, WEAPON, ARMOR }

    public ShopType shopType;

    public void Start()
    {
        items = new List<Item>();
        this.GetComponent<SpriteRenderer>().sprite = renderableForm;

        ChangeContainerContent(GameManager.instance.currentLevel);
    }

    public void ChangeContainerContent(int level)
    {
        this.items.Clear();
        if (shopType == ShopType.WEAPON)
        {
            for (int i = 0; i < 28; ++i)
            {
                int itemIconMin = 0;
                int itemIconMax = 0;

                int attributeTypeMax = 3;

                int skillProjectileId = 0;

                int skillTypeMin = 0;
                int skillTypeMax = 0;

                int skillIcon = 0;
                int itemIcon;
                int attributeType;

                string baseName = "Weapon";

                int skillType = 0;

                int wepType = Random.Range(0, 2);
                Random.Range(0, attributeTypeMax);
                attributeType = Random.Range(0, attributeTypeMax);

                GameManager.Pair skillProj = new GameManager.Pair(0, 0);

                switch (wepType)
                {
                    case 0:
                        itemIconMin = 6;
                        itemIconMax = 12;
                        skillTypeMin = 0;
                        skillTypeMax = 1;

                        baseName = "Sword";
                        break;
                    case 1:
                        itemIconMin = 0;
                        itemIconMax = 5;
                        skillTypeMin = 1;
                        skillTypeMax = 3;
                        baseName = "Bow";
                        break;
                    default:
                        break;
                }

                skillType = Random.Range(skillTypeMin, skillTypeMax);
                itemIcon = Random.Range(itemIconMin, itemIconMax);
                skillIcon = skillType;

                if (skillType > 0)
                {
                    skillProjectileId = Random.Range(0, 2);
                }
                else
                    skillProjectileId = skillType;

                itemIcon = Random.Range(itemIconMin, itemIconMax);
                items.Add(GameManager.instance.weaponFactory.CreateWeapon(new WeaponFactory.WeaponEssentials((WeaponFactory.AttributeType)attributeType, (Weapon.ClassType)wepType, GameObjectContainers.instance.GetAttackEffect(0), 10, (Weapon.SkillClassType)skillType, GameObjectContainers.instance.GetAttackEffect(1), skillType > 0 ? GameObjectContainers.instance.GetArrowProjectile(skillProjectileId) : GameObjectContainers.instance.GetMagicProjectile(skillProjectileId), wepType == 1 ? GameObjectContainers.instance.GetArrowProjectile(0) : null, GameObjectContainers.instance.GetSkillIcon(skillIcon)),
                                                                          new ItemFactory.ItemEssentials(GameObjectContainers.instance.GetItemIcon(itemIcon), 30, GameManager.instance.currentLevel, baseName, 0, true, new Vector2(1, 1), true, Item.ItemType.WEAPON, testRenderableForm, null, Character.instance)));
            }
        }

        else if (shopType == ShopType.POTION)
        {
            for (int i = 0; i < 1; ++i)
            {
                items.Add(GameManager.instance.potionFactory.CreatePotion(new PotionFactory.PotionEssentials(Potion.PotionType.HEALTH, 10 * GameManager.instance.currentLevel),
                                                                          new ItemFactory.ItemEssentials(GameObjectContainers.instance.GetPotionIcon(0), (int)(10 * GameManager.instance.currentLevel * 0.3f), GameManager.instance.currentLevel, "Health Potion", 1, false, new Vector2(1, 1), true, Item.ItemType.POTION, null, null, null)));
            }
        }

        else if (shopType == ShopType.ARMOR)
        {
            for (int i = 0; i < 28; ++i)
            {
                int tp = Random.Range(1, 4);
                int icon = 0;
                string baseName = "";

                switch (tp)
                {
                    case 1:
                        icon = Random.Range(0, 5);
                        baseName = "Legs";
                        break;
                    case 2:
                        icon = Random.Range(5, 10);
                        baseName = "Chest Armor";
                        break;
                    case 3:
                        icon = Random.Range(10, 15);
                        baseName = "Helmet";
                        break;
                    default:
                        break;
                }

                items.Add(GameManager.instance.armorFactory.CreateArmor(new ArmorFactory.ArmorEssentials((Armor.ClassType)tp),
                                                                        new ItemFactory.ItemEssentials(GameObjectContainers.instance.GetArmorIcon(icon), (int)(20 * GameManager.instance.currentLevel * 0.2f), GameManager.instance.currentLevel, baseName, 1, true, new Vector2(1, 1), true, Item.ItemType.ARMOR, null, null, null)));
            }
        }
    }

    public void SetImageActivity(bool newActivity)
    {
        if (shopType == ShopType.WEAPON)
        {
            CanvasScript.instance.NPCPanel.gameObject.SetActive(newActivity);
            CanvasScript.instance.prompt.gameObject.SetActive(false);
        }
        else if(shopType == ShopType.POTION)
        {
            CanvasScript.instance.potionPanel.gameObject.SetActive(newActivity);
            CanvasScript.instance.prompt.gameObject.SetActive(false);
        }
        else if (shopType == ShopType.ARMOR)
        {
            CanvasScript.instance.NPCArmorPanel.gameObject.SetActive(newActivity);
            CanvasScript.instance.prompt.gameObject.SetActive(false);
        }

        UpdateContainer();
    }

    public void UpdateContainer()
    {
        GUIItem[] containerItems = new GUIItem[]{ null };

        if (shopType == ShopType.WEAPON)
        {
            containerItems = CanvasScript.instance.NPCPanel.GetComponent<UIPanels>().itemHolders;
            for (int i = 0; i < 28; ++i)
                containerItems[i].SetItem(null);
        }
        else if(shopType == ShopType.POTION)
        {
            containerItems = CanvasScript.instance.potionPanel.GetComponent<UIPanels>().itemHolders;
            for (int i = 0; i < 6; ++i)
                containerItems[i].SetItem(null);
        }
        else if(shopType == ShopType.ARMOR)
        {
            containerItems = CanvasScript.instance.NPCArmorPanel.GetComponent<UIPanels>().itemHolders;
            for (int i = 0; i < 28; ++i)
                containerItems[i].SetItem(null);
        }
        
        for (int i = 0; i < items.Count; ++i)
            containerItems[i].SetItem(items[i]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Character>().toBeInteracted = this.gameObject;
            collision.GetComponent<Character>().toInteract = true;

            CanvasScript.instance.prompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Character>().toBeInteracted = null;
            collision.GetComponent<Character>().toInteract = false;

            CanvasScript.instance.prompt.gameObject.SetActive(false);
            if (shopType == ShopType.WEAPON)
                CanvasScript.instance.NPCPanel.gameObject.SetActive(false);
            else if (shopType == ShopType.POTION)
                CanvasScript.instance.potionPanel.gameObject.SetActive(false);
            else if(shopType == ShopType.ARMOR)
                CanvasScript.instance.NPCArmorPanel.gameObject.SetActive(false);

            GameManager.instance.openStore = null;

            if (CanvasScript.instance.itemInfo.activeInHierarchy && CanvasScript.instance.itemInfo.GetComponent<ItemInfoScript>().type == 2)
                CanvasScript.instance.itemInfo.SetActive(false);
        }
    }
}
