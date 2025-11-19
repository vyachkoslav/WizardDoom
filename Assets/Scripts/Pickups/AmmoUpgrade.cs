using System.Collections.Generic;
using Player;
using Player.Weapons;
using UnityEngine;

public class AmmoUpgrade : Pickup
{
    [SerializeField] private int _maxAmmoToAdd;
    
    private PlayerEntity _player;
    private List<Weapon> _weaponList;

    private void Start()
    {
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
            // SoundManager.Instance.PlaySound3D("PickupGet", transform.position);
        }
    }
}