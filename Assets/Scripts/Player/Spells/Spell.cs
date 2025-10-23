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
        [SerializeField] protected float _durationInSeconds;

        [SerializeField] protected GameObject spellObject;
        [SerializeField] protected AudioClip _spellAudioClip;

        // Spell mana cost getter
        public int Cost { get { return _cost; } }

        // Spell audio clip getter
        public AudioClip SpellAudioClip { get { return _spellAudioClip; } }

        // Main functionality, different for each spell
        public abstract void Cast();
    }
}
