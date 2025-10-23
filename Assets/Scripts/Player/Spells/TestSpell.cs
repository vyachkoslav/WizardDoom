using UnityEngine;

namespace Player.Spells
{
    [CreateAssetMenu(fileName = "TestSpell", menuName = "Spells/TestSpell")]
    public class TestSpell : Spell
    {
        public override void Cast()
        {
            Debug.Log(this.name + " cast spell!");
        }
    }
}
