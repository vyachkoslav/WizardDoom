using UnityEngine.UI;
using UnityEngine;


namespace Player.UI{
    public class ManaUI : MonoBehaviour
    {

        [SerializeField] private Slider manaSlider;
        [SerializeField] private SpellController spellController;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            spellController.OnManaChanged += OnManaChanged;
            //This might need changeing in the future if SpellController changed to sigleton
            manaSlider.minValue = 0;
            manaSlider.maxValue = spellController.MaxMana;

            manaSlider.value = spellController.CurrentMana;
        }

        private void OnManaChanged()
        {
            manaSlider.value = spellController.CurrentMana;
        }

    }
}