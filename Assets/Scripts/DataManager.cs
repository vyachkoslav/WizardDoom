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
    private static float maxHealthToAdd = 0;
    public float MaxHealthToAdd { set { maxHealthToAdd += value; } }

    private static int maxAmmoToAdd = 0;
    public int MaxAmmoToAdd { set { maxAmmoToAdd += value; } }

    private static int maxManaToAdd = 0;
    public int MaxManaToAdd { set { maxManaToAdd += value; } }
    
    private static float newManaRegenSpeed = 1f;
    public float NewManaRegenSpeed { set { newManaRegenSpeed = value; } }

    private static List<Pickup> upgradeList = new List<Pickup>();
    public List<Pickup> UpgradeList { get { return upgradeList; } }

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

        foreach (Pickup upgrade in upgradeList)
        {
            switch (upgrade)
            {
                case HealthUpgrade: 
                    PlayerEntity.Instance.MaxHealth += maxHealthToAdd;
                    break;
                case AmmoUpgrade:
                    List<Weapon> weaponList = PlayerEntity.Instance.GetComponent<WeaponController>().AvailableWeapons;
                    foreach (Weapon weapon in weaponList) 
                        { 
                            weapon.CurrentAmmo += maxAmmoToAdd;
                            weapon.MaxCarriableAmmo += maxAmmoToAdd; 
                        }
                    break;
                case ManaUpgrade:
                    PlayerEntity.Instance.GetComponent<SpellController>().AddMaxMana(maxManaToAdd);
                    break;
                case ManaRegenUpgrade:
                    PlayerEntity.Instance.GetComponent<SpellController>().RegenSpeedInSeconds = newManaRegenSpeed;
                    break;
            }
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
        checkPoint = Vector3.zero;
        spellList.Clear();
        upgradeList.Clear();
        maxHealthToAdd = 0;
        maxAmmoToAdd = 0;
        maxManaToAdd = 0;
        newManaRegenSpeed = 1f;
        isWin = false;
    }
}
