using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Player.Spells;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

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
        [SerializeField] private UnityEvent onManaChanged;
        [SerializeField] private UnityEvent onSpellChanged;
        public event Action OnManaChanged;
        public event Action OnSpellChanged;

        private int _currentSpellIndex;
        private Spell _currentSelectedSpell;
        private int _currentMana;
        private int _manaCost;

        private bool _manaRegen = false;

        public Transform ProjectileSpawn { get { return _projectileSpawn; } }
        public float MaxMana { get { return _maxMana; } }
        public float CurrentMana { get { return _currentMana; } }
        public Spell CurrentSelectedSpell { get { return _currentSelectedSpell; }}

        // Set current spell to first in list
        private void Awake()
        {
            _currentSpellIndex = 0;
            _currentSelectedSpell = _spellList[_currentSpellIndex];
            _currentMana = _maxMana;
            OnManaChanged += onManaChanged.Invoke;
            OnSpellChanged += onSpellChanged.Invoke;
            Debug.Log(_spellList.Count);
        }
        private void Start()
        {
            OnSpellChanged.Invoke();
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
            _manaCost = _currentSelectedSpell.Cost;

            if (_currentMana >= _manaCost)
            {
                // Prevent using spells and duplicating lifesteal spell while it's already active
                if (!DataManager.Instance.IsLifeStealActive)
                {
                    _currentMana -= _manaCost;
                    OnManaChanged.Invoke();
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
            _currentSelectedSpell = _spellList[_currentSpellIndex];
            OnSpellChanged.Invoke();
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
            OnManaChanged.Invoke();
            yield return new WaitForSeconds(1);
            _manaRegen = false;
            Debug.Log("Current mana: " + _currentMana);
        }
    }
}
