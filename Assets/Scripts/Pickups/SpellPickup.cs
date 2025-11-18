using Player;
using Player.Spells;
using UnityEngine;

public class SpellPickup : Pickup
{
    [SerializeField] private Spell _spell;

    private GameObject _player;

    private void Start()
    {
        _player = PlayerEntity.Instance.gameObject;
    }

    // Check if collided with player, then call provide spell
    protected override void OnTriggerEnter(Collider _)
    {
        var target = _.gameObject;

        if (target == _player)
        {
            _player.GetComponent<SpellController>().AddSpellToList(_spell);
            // SoundManager.Instance.PlaySound3D("SpellPickup", transform.position);
            StartCoroutine(Respawn());
        }
    }
}
