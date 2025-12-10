using Player;
using UnityEngine;

public class ManaRegenUpgrade : Pickup
{
    [SerializeField] private float _newRegenSpeedInSeconds;

    private PlayerEntity _player;

    private void Start()
    {
        _player = PlayerEntity.Instance;
    }

    // Add mana regen speed (lessen wait time in RegenMana coroutine)
    protected override void PickupEffect()
    {
        _player.GetComponent<SpellController>().RegenSpeedInSeconds = _newRegenSpeedInSeconds;
        // SoundManager.Instance.PlaySound3D("PickupGet", transform.position);
    }

    public override string ToString()
    {
        return "Faster mana regeneration";
    }
}