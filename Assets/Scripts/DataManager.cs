using System.Collections.Generic;
using Player;
using Player.Spells;
using Player.Weapons;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance { get { return _instance; } }

    // Player checkpoint
    private static Vector3 checkPoint = new Vector3(-2, 1, 3);
    public Vector3 CheckPoint { get { return checkPoint; } set { checkPoint = value; } }

    // Used when determining if LifeSteal effect should be applied
    private static bool isLifeStealActive = false;
    public bool IsLifeStealActive
    {
        get { return isLifeStealActive; }
        set { isLifeStealActive = value; }
    }

    //Has player won the game?
    private static bool isWin = false;
    public bool IsWin
    {
        get { return isWin; }
        set { isWin = value; }
    }

    // Is player in combat?
    private static bool isFighting = false;
    public bool IsFighting 
    { 
        get { return isFighting; } 
        set { isFighting = value; }
    }

    // Player's key collection
    // AddKeyToList and CheckKeyInList methods ensure only Key type can be used to add to list
    private static List<string> keyList = new List<string>();

    public void AddKeyToList(Key key)
    {
        if (!keyList.Contains(key.ID))
        {
            keyList.Add(key.ID);
        }
    }

    public bool CheckKeyInList(Key key)
    {
        return keyList.Contains(key.ID);
    }

    // Acquired spells
    private static List<Spell> spellList = new List<Spell>();
    public List<Spell> SpellList { get { return spellList; } }

    public void AcquireSpell(Spell spell)
    {
        if (!spellList.Contains(spell))
        {
            spellList.Add(spell);
        }
    }

    // Player's upgrades, applied in Awake after death
    private static Dictionary<Pickup, float> upgradeList = new Dictionary<Pickup, float>();
    public Dictionary<Pickup, float> UpgradeList { get { return upgradeList; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else 
        { 
            _instance = this; 
            DontDestroyOnLoad(this.gameObject);
        }

        foreach (KeyValuePair<Pickup, float> upgrade in upgradeList)
        {
            if (upgrade.Key is HealthUpgrade)
            {
                PlayerEntity.Instance.SetMaxHealth(PlayerEntity.Instance.Health + upgrade.Value);
            }
            if (upgrade.Key is AmmoUpgrade)
            {
                List<Weapon> weaponList = PlayerEntity.Instance.GetComponent<WeaponController>().AvailableWeapons;
                foreach (Weapon weapon in weaponList) 
                { 
                    weapon.CurrentAmmo += (int)upgrade.Value;
                    weapon.MaxCarriableAmmo += (int)upgrade.Value; 
                }
            }
            if (upgrade.Key is ManaUpgrade) { PlayerEntity.Instance.GetComponent<SpellController>().AddMaxMana((int)upgrade.Value); }
            if (upgrade.Key is ManaRegenUpgrade) { PlayerEntity.Instance.GetComponent<SpellController>().RegenSpeedInSeconds = upgrade.Value; }
        }
    }

    // When player dies, variables related to in game events should be reset
    public void ResetGameStats()
    {
        isLifeStealActive = false;
        isFighting = false;
    }

    // Used when reseting everything
    public void ResetAllStats()
    {
        ResetGameStats();

        keyList.Clear();
        checkPoint = new Vector3(-2, 1, 3);
        spellList.Clear();
        upgradeList.Clear();
        isWin = false;
    }
}
