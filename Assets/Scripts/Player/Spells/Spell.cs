using UnityEngine;

namespace Player.Spells
{
    //Abstract SO Spell template class 
    public abstract class Spell : ScriptableObject
    {
        //TOOD or expand according to future demand
        [Header("Stats")]
        [SerializeField] private int _cost;
        [SerializeField] private float _distance;
        [SerializeField] private float _durationInSeconds;
        [SerializeField] private float _damage;

        [SerializeField] private GameObject _spellObject;

        //Main functionality, different for each spell
        public abstract void Cast();
    }
}
