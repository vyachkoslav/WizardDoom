using System.Collections.Generic;
using Player;
using Player.Weapons;
using UnityEngine;

public class AmmoUpgrade : Pickup
{
    [SerializeField] private int _maxAmmoToAdd;
    
    private PlayerEntity _player;
    private List<Weapon> _weaponList;

    protected override void Start()
    {
        base.Start();
        _player = PlayerEntity.Instance;
        _weaponList = _player.GetComponent<WeaponController>().AvailableWeapons;
    }

    // Add to player's max ammo
    protected override void PickupEffect()
    {
        foreach (Weapon weapon in _weaponList)
        {
            weapon.CurrentAmmo += _maxAmmoToAdd;
            weapon.MaxCarriableAmmo += _maxAmmoToAdd;
            SoundManager.Instance.PlaySound3D("WeaponReload", transform.position);
        }

        DataManager.Instance.UpgradeList.Add(this);
        DataManager.Instance.MaxAmmoToAdd = _maxAmmoToAdd;
        this.gameObject.SetActive(false);
    }

    public override string ToString()
    {
        return "+" + _maxAmmoToAdd + " max ammo";
    }
}