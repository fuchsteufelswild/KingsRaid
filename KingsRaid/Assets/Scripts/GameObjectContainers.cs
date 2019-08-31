using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectContainers : MonoBehaviour
{
    public GameObject[] magicProjectiles;
    public GameObject[] arrowProjectiles;

    public GameObject[] projectileContainer;
    public GameObject[] attackEffectContainer;
    public GameObject[] skillEffectContainer;
    public GameObject[] containers;
    public GameObject[] mobs;

    public Sprite[] skillIcons;
    public Sprite[] itemIcons;
    public Sprite[] potionIcons;
    public Sprite[] arrowProjectileSprites;
    public Sprite[] armorIcons;

    public Sprite[] backGroundIcons;

    public AudioClip[] clips;

    public static GameObjectContainers instance;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public GameObject GetAttackEffect(int id)
    {
        return attackEffectContainer[id];
    }

    public GameObject GetSkillEffect(int id)
    {
        return skillEffectContainer[id];
    }

    public GameObject GetProjectile(int id)
    {
        return projectileContainer[id];
    }

    public Sprite GetSkillIcon(int id)
    {
        return skillIcons[id];
    }

    public Sprite GetItemIcon(int id)
    {
        return itemIcons[id];
    }

    public Sprite GetArrowProjectileSprite(int id)
    {
        return arrowProjectileSprites[id];
    }

    public GameObject GetArrowProjectile(int id)
    {
        return arrowProjectiles[id];
    }

    public GameObject GetMagicProjectile(int id)
    {
        return magicProjectiles[id];
    }

    public Sprite GetPotionIcon(int id)
    {
        return potionIcons[id];
    }

    public Sprite GetArmorIcon(int id)
    {
        return armorIcons[id];
    }

    public AudioClip GetAudioClip(int id)
    {
        return clips[id];
    }

    public GameObject GetMobPrefab(int id)
    {
        return mobs[id];
    }
}
