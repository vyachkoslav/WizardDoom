using System.Collections.Generic;
using Player;
using Player.Weapons;
using UnityEngine;

public class AmmoPickup : Pickup
{
    [SerializeField] private int _ammoToAdd;

    private GameObject _player;
    private List<Weapon> _weaponList;

    // Get list of player weapons
    private void Start()
    {
        _player = PlayerEntity.Instance.gameObject;
        _weaponList = _player.GetComponent<WeaponController>().AvailableWeapons;
    }

    // Add ammo to player upon contact
    protected override void OnTriggerEnter(Collider _)
    {
        var target = _.gameObject;

        if (target == _player)
        {
            foreach (Weapon weapon in _weaponList)
            {
                weapon.AddAmmo(_ammoToAdd);
            }

            // SoundManager.Instance.PlaySound3D("AmmoPickup", transform.position);
            StartCoroutine(Respawn());
        }
    }
}