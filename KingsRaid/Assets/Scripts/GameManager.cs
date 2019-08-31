using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int loadedCount = 0;
    public struct Pair
    {
        public float x;
        public float y;

        public Pair(float _x, float _y) { x = _x; y = _y; }
    }
    public int currentLevel = 1;

    public TravelPrompt travel;
    public int travelID;

    public static GameManager instance;

    public Pair mapBounds;

    public SkillTypeFactory skillTypeFactory;
    public AttackTypeFactory attackTypeFactory;
    public WeaponFactory weaponFactory;
    public PotionFactory potionFactory;
    public ArmorFactory armorFactory;

    public StoreScript openStore = null;
    public Container openContainer = null;

    public bool toTravel = false;

    public Dictionary<int, Pair> ts;

    public StoreScript[] stores;

    private void Awake()
    {
        if (!instance)
        {
            travel = null;
            mapBounds = new Pair(-11.29f + 0.3f, 9.15f -0.3f);
            ts = new Dictionary<int, Pair>(2) { { 1, new Pair(0, 0) }, { 2, new Pair(0, 2) } };
            attackTypeFactory = new AttackTypeFactory();
            weaponFactory = new WeaponFactory();
            potionFactory = new PotionFactory();
            armorFactory = new ArmorFactory();
            skillTypeFactory = new SkillTypeFactory();
            instance = this;
        }
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }


    private void Start()
    {
        stores = FindObjectsOfType<StoreScript>();
        ++GameManager.instance.loadedCount;
    }
}
