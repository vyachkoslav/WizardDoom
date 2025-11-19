using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace Player.UI{
    public class ManaUI : MonoBehaviour
    {
        [SerializeField] private Slider manaSlider;
        [SerializeField] private TMP_Text manaText;
        [SerializeField] private SpellController spellController;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            spellController.OnManaChanged += OnManaChanged;
            //This might need changeing in the future if SpellController changed to sigleton
            manaSlider.minValue = 0;
            manaSlider.maxValue = spellController.MaxMana;

            manaSlider.value = spellController.CurrentMana;

            manaText.text = "Mana: " + manaSlider.value + "/" + manaSlider.maxValue;
        }

        private void OnManaChanged()
        {
            manaSlider.value = spellController.CurrentMana;
            manaText.text = "Mana: " + manaSlider.value + "/" + manaSlider.maxValue;
        }

        public void UpdateMaxMana()
        {
            manaSlider.maxValue = spellController.MaxMana;
            manaText.text = "Mana: " + manaSlider.value + "/" + manaSlider.maxValue;
        }
    }
}