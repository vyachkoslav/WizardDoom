using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Player.Spells;

namespace Player
{
    public class SpellController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference castSpellAction;
        [SerializeField] private InputActionReference nextSpellAction;

        [Header("Audio")]
        [SerializeField] private AudioSource _spellAudioSource;

        [Header("Camera and projectile spawn position")]
        // [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _projectileSpawn;

        [Header("Spells")]
        [SerializeField] private List<Spell> _spellList;

        [Header("Mana")]
        [SerializeField] private int _playerMana;

        private int _currentSpellIndex;
        private Spell _currentSelectedSpell;
        private int _manaCost;

        public Transform ProjectileSpawn { get { return _projectileSpawn; }}

        // Set current spell to first in list
        private void Awake()
        {
            _currentSpellIndex = 0;
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

            if (_playerMana >= _manaCost)
            {
                _playerMana -= _manaCost;
                _currentSelectedSpell.Cast();
                _spellAudioSource.PlayOneShot(_currentSelectedSpell.SpellAudioClip, 0.5f);
                Debug.Log("Current mana: " + _playerMana);
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
        }
    }
}
