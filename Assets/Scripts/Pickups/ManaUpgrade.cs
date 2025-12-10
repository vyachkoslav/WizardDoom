using Player;
using Player.UI;
using UnityEngine;

public class ManaUpgrade : Pickup
{
    [SerializeField] private int _maxManaToAdd;
    [SerializeField] private ManaUI _manaBar;

    private PlayerEntity _player;

    protected override void Start()
    {
        base.Start();
        _player = PlayerEntity.Instance;
    }

    // Add to player's max mana
    protected override void PickupEffect()
    {
        _player.GetComponent<SpellController>().AddMaxMana(_maxManaToAdd);
        _manaBar.UpdateMaxMana();
        SoundManager.Instance.PlaySound3D("Mana", transform.position);

        DataManager.Instance.UpgradeList.Add(this);
        DataManager.Instance.MaxManaToAdd = _maxManaToAdd;
        this.gameObject.SetActive(false);
    }

    public override string ToString()
    {
        return "+" + _maxManaToAdd + " max mana";
    }
}