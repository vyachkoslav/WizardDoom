using Player;
using Player.UI;
using UnityEngine;

public class HealthUpgrade : Pickup
{
    [SerializeField] private float _maxHealthToAdd;
    [SerializeField] private HealthUI _healthBar;

    private PlayerEntity _player;

    protected override void Start()
    {
        base.Start();
        _player = PlayerEntity.Instance;
    }

    // Add to player's max health
    protected override void PickupEffect()
    {
        _player.MaxHealth += _maxHealthToAdd;
        _healthBar.UpdateMaxHealth();
        SoundManager.Instance.PlaySound3D("Health", transform.position);

        DataManager.Instance.UpgradeList.Add(this, _maxHealthToAdd);
        
        this.gameObject.SetActive(false);
    }

    public override string ToString()
    {
        return "+" + _maxHealthToAdd + " max health";
    }
}