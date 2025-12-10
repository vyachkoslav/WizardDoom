using System.Collections.Generic;
using Player.Spells;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossDoor : Door
{
    // [SerializeField] private List<Spell> _spellList = new List<Spell>();

    public override void Interact()
    {
        // Check which spells have been acquired
        if (DataManager.Instance.SpellList.Count == 3)
        {
            StartCoroutine(OpenDoor());
        }
    }

    public override string DisplayText()
    {
        if (DataManager.Instance.SpellList.Count == 3)
        {
            return "Press '" + interactAction.action.GetBindingDisplayString() + "' to open door and face the Grand Wizard!";
        }
        else { return "Acquire all three spells to face the Grand Wizard!"; }
    }
}
