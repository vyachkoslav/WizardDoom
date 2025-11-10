using Player;
using Player.Spells;
using UnityEngine;

public class SpellPickup : MonoBehaviour
{
    [SerializeField] private Spell spell;

    private GameObject player;

    private void Start()
    {
        player = PlayerEntity.Instance.gameObject;
    }

    // Check if collided with player, then call provide spell
    private void OnTriggerEnter(Collider _)
    {
        GameObject target = _.gameObject;

        if (target == player)
        {
            player.GetComponent<SpellController>().AddSpellToList(spell);
        }
    }
}
