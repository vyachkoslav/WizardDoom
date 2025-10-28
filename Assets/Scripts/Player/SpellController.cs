using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Player.Spells;
using System.Collections.Generic;

namespace Player
{
    public class SpellController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference castSpellAction;
        [SerializeField] private InputActionReference nextSpellAction;

        [Header("Camera and projectile spawn position")]
        // [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _projectileSpawn;

        [Header("Spells")]
        [SerializeField] private List<Spell> _spellList;

        [Header("Mana")]
        [SerializeField] private int _maxMana;
        [SerializeField] private int _manaRegenPerSecond;

        private int _currentSpellIndex;
        private Spell _currentSelectedSpell;
        private int _currentMana;
        private int _manaCost;

        private bool _manaRegen = false;

        public Transform ProjectileSpawn { get { return _projectileSpawn; }}

        // Set current spell to first in list
        private void Awake()
        {
            _currentSpellIndex = 0;
            _currentMana = _maxMana;
            Debug.Log(_spellList.Count);
        }

        private void OnEnable()
        {
            castSpellAction.action.performed += CastSpell;
            nextSpellAction.action.performed += NextSpell;
        }

        private void OnDisable()
        {
            castSpellAction.action.performed -= CastSpell;
            nextSpellAction.action.performed -= NextSpell;
        }

        // Check and subtract mana cost from player mana, call Cast()-function from 
        // current spell, play audio
        private void CastSpell(InputAction.CallbackContext context)
        {
            _currentSelectedSpell = _spellList[_currentSpellIndex];
            _manaCost = _currentSelectedSpell.Cost;

            if (_currentMana >= _manaCost)
            {
                // Prevent using spells and duplicating lifesteal spell while it's already active
                if (!DataManager.Instance.IsLifeStealActive)
                {
                    _currentMana -= _manaCost;
                    _currentSelectedSpell.Cast();
                    SoundManager.Instance.PlaySound2D("Cast");
                    Debug.Log("Current mana: " + _currentMana);    
                }
            }
        }

        // Check if out of bounds, then append spell list index accordingly
        private void NextSpell(InputAction.CallbackContext context)
        {
            if (_currentSpellIndex < _spellList.Count - 1)
            {
                _currentSpellIndex++;
            }
            else { _currentSpellIndex = 0; }

            Debug.Log("Using: " + _spellList[_currentSpellIndex]);
            SoundManager.Instance.PlaySound2D("NextSpell");
        }

        // Check if mana is less than max, then regenerate
        private void Update()
        {
            if (_currentMana < _maxMana && !_manaRegen)
            {
                _manaRegen = true;
                StartCoroutine(RegenMana());
            }
        }

        // Regenerate mana, don't let mana get over max, wait 1 second
        private IEnumerator RegenMana()
        {
            _currentMana += _manaRegenPerSecond;

            if (_currentMana > _maxMana)
            {
                _currentMana = _maxMana;
            }

            yield return new WaitForSeconds(1);
            _manaRegen = false;
            Debug.Log("Current mana: " + _currentMana);
        }
    }
}
