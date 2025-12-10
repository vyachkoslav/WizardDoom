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
        
        if (DataManager.Instance.SpellList.Contains(_spell))
        {
            Destroy(this.gameObject);
        }
    }

    // Provide spell to player
    protected override void PickupEffect()
    {
        _player.GetComponent<SpellController>().AddSpellToList(_spell);
        // SoundManager.Instance.PlaySound3D("SpellPickup", transform.position);

        // Destroys spell pickup object
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return "Acquired " + _spell;
    }
}
