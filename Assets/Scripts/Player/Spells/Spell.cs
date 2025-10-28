using UnityEngine;

namespace Player.Spells
{
    // Abstract SO Spell template class 
    public abstract class Spell : ScriptableObject
    {
        // Expand according to future demand
        [Header("Stats")]
        [SerializeField] protected int _cost;
        [SerializeField] protected float _damage;
        [SerializeField] protected float _moveSpeed;
        [SerializeField] protected Color _color;

        [SerializeField] protected GameObject spellObject;

        // Spell mana cost getter
        public int Cost { get { return _cost; } }

        //  Spell color getter
        public Color Color { get { return _color; } }

        // Main functionality, different for each spell
        public abstract void Cast();
    }
}
