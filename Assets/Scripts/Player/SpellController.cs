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

        [Header("Spells")]
        [SerializeField] private List<Spell> _spellList;

        private int _currentSpellIndex;

        //Set current spell to first in list
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

        //Call Cast()-function of current spell
        private void CastSpell(InputAction.CallbackContext context)
        {
            _spellList[_currentSpellIndex].Cast();
        }

        //Check if out of bounds, then append spell list index accordingly
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
