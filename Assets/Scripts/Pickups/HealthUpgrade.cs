using Player;
using Player.UI;
using UnityEngine;

public class HealthUpgrade : Pickup
{
    [SerializeField] private float _maxHealthToAdd;
    [SerializeField] private HealthUI _healthBar;

    private PlayerEntity _player;

    private void Start()
    {
        _player = PlayerEntity.Instance;
    }

    // Add to player's max health
    protected override void PickupEffect()
    {
        _player.MaxHealth += _maxHealthToAdd;
        _healthBar.UpdateStats();
        // SoundManager.Instance.PlaySound3D("PickupGet", transform.position);
    }
}