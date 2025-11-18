using Player;
using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] private float _healthToAdd;

    // Apply health to player
    protected override void OnTriggerEnter(Collider _)
    {
        var target = _.gameObject;

        if (target == PlayerEntity.Instance.gameObject)
        {
            PlayerEntity.Instance.ApplyHealing(_healthToAdd);
            // SoundManager.Instance.PlaySound3D("HealthPickup", transform.position);
            StartCoroutine(Respawn());
        }
    }
}