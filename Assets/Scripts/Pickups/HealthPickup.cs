using Player;
using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] private float _healthToAdd;

    // Apply health to player
    protected override void PickupEffect()
    {
        PlayerEntity.Instance.ApplyHealing(_healthToAdd);
        // SoundManager.Instance.PlaySound3D("PickupGet", transform.position);
    }
}