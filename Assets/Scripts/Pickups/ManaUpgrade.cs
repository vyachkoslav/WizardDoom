using Player;
using Player.UI;
using UnityEngine;

public class ManaUpgrade : Pickup
{
    [SerializeField] private int _maxManaToAdd;
    [SerializeField] private ManaUI _manaBar;

    private PlayerEntity _player;

    private void Start()
    {
        _player = PlayerEntity.Instance;
    }

    // Add to player's max mana
    protected override void PickupEffect()
    {
        _player.GetComponent<SpellController>().AddMaxMana(_maxManaToAdd);
        _manaBar.UpdateMaxMana();
        SoundManager.Instance.PlaySound3D("Mana", transform.position);
    }

    public override string ToString()
    {
        return "+" + _maxManaToAdd + " max mana";
    }
}