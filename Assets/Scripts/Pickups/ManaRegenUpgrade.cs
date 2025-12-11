using Player;
using UnityEngine;

public class ManaRegenUpgrade : Pickup
{
    [SerializeField] private float _newRegenSpeedInSeconds;

    private PlayerEntity _player;

    protected override void Start()
    {
        base.Start();
        _player = PlayerEntity.Instance;
    }

    // Add mana regen speed (lessen wait time in RegenMana coroutine)
    protected override void PickupEffect()
    {
        _player.GetComponent<SpellController>().RegenSpeedInSeconds = _newRegenSpeedInSeconds;
        SoundManager.Instance.PlaySound3D("Mana", transform.position);

        DataManager.Instance.UpgradeList.Add(this, _newRegenSpeedInSeconds);
        this.gameObject.SetActive(false);
    }

    public override string ToString()
    {
        return "Faster mana regeneration";
    }
}