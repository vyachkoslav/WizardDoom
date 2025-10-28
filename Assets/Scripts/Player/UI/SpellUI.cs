using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Player.Spells;



namespace Player.UI{
    public class SpellUI : MonoBehaviour
    {

        [SerializeField] private Image spellColor;
        [SerializeField] private TMP_Text text;
        [SerializeField] private SpellController spellController;
        private Spell spell;
        private Color color;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            spellController.OnSpellChanged += OnSpellChanged;
        }

        // Update is called once per frame
        private void OnSpellChanged()
        {
            spell = spellController.CurrentSelectedSpell;
            color = spell.Color;
            spellColor.color = color;
            text.text = spell.ToString();
        }
    }
}

