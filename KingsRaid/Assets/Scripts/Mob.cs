using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : NPC
{
    public SpotEnemy detector;

    public UnityEngine.UI.Image healthBarFill;
    // Start is called before the first frame update
    void Start()
    {
        this.items = new Item[3];
        detector = GetComponentInChildren<SpotEnemy>();

        this.actorType = NPC.ActorType.MOB;
        typesToTarget.Add(NPC.ActorType.COMPANION);
        typesToTarget.Add(NPC.ActorType.PLAYER);

        AttackAreaDamage temp = hitPoint.GetComponent<AttackAreaDamage>();
        temp.damage = this.damage;
        //temp.typesToAttack = new string[2] { "Player", "Companion" };

        // this.state = NPC.State.PATROL;
        this.SetState(new PatrolState(this.gameObject));
        //if (this.type == NPCType.RANGED)
        //{
        //    projectile.GetComponent<Projectile>().typesToAttack.Add("Player");
        //    projectile.GetComponent<Projectile>().typesToAttack.Add("Companion");
        //}
        

        // Randomly Create Weapons
        for(int i = 0; i < 2; ++i)
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
            items[i] = (GameManager.instance.weaponFactory.CreateWeapon(new WeaponFactory.WeaponEssentials((WeaponFactory.AttributeType)attributeType, (Weapon.ClassType)wepType, GameObjectContainers.instance.GetAttackEffect(0), 10, (Weapon.SkillClassType)skillType, GameObjectContainers.instance.GetAttackEffect(1), skillType > 0 ? GameObjectContainers.instance.GetArrowProjectile(skillProjectileId) : GameObjectContainers.instance.GetMagicProjectile(skillProjectileId), wepType == 1 ? GameObjectContainers.instance.GetArrowProjectile(0) : null, GameObjectContainers.instance.GetSkillIcon(skillIcon)),
                                                                      new ItemFactory.ItemEssentials(GameObjectContainers.instance.GetItemIcon(itemIcon), (int)(Random.Range(0f, 1f) * 30), GameManager.instance.currentLevel, baseName, 0, true, new Vector2(1, 1), true, Item.ItemType.WEAPON, null, null, Character.instance)));
        }

        items[2] = (GameManager.instance.potionFactory.CreatePotion(new PotionFactory.PotionEssentials(Potion.PotionType.HEALTH, 10 * GameManager.instance.currentLevel),
                                                                          new ItemFactory.ItemEssentials(GameObjectContainers.instance.GetPotionIcon(0), (int)(10 * GameManager.instance.currentLevel * 0.3f), GameManager.instance.currentLevel, "Health Potion", 1, false, new Vector2(1, 1), true, Item.ItemType.POTION, null, null, null))); 

    }

    public override void Die()
    {
        // Instantiate a container
        // Set Container items to itemsToDrop
    }

    public override void FinishDeath()
    {
        GameObject tContainer = Instantiate(GameObjectContainers.instance.containers[0], new Vector3(this.transform.position.x, this.transform.position.y - 0.416f, this.transform.position.z), Quaternion.identity) as GameObject;
        LevelGenerator.instance.containers.Add(tContainer);

        for(int i = 0; i < this.items.Length; ++i)
            tContainer.GetComponent<Container>().AddItem(items[i]);

        LevelGenerator.instance.RemoveMob(this.gameObject);

        Destroy(this.gameObject);
    }

    protected override void Update()
    {
        base.Update();
    }

}
