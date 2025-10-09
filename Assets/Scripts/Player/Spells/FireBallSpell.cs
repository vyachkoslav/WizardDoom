using UnityEngine;

namespace Player.Spells
{
    [CreateAssetMenu(fileName = "FireBallSpell", menuName = "Spells/FireBallSpell")]
    public class FireBallSpell : Spell
    {
        // TODO
        public override void Cast()
        {
            Debug.Log("Fireball");
        }
    }
}
