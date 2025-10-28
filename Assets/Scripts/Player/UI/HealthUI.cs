using UnityEngine.UI;
using UnityEngine;

namespace Player.UI
{

    public class HealthUI : MonoBehaviour
    {
         [SerializeField] private Slider healthSlider;


        private void Start()
        {
            PlayerEntity.Instance.OnHealthDecreased += OnHealthDecreased;
            PlayerEntity.Instance.OnHealthIncreased += OnHealthIncreased;

            healthSlider.minValue = 0;
            healthSlider.maxValue = PlayerEntity.Instance.MaxHealth;

            healthSlider.value = PlayerEntity.Instance.Health;

        }

        private void OnHealthDecreased()
        {
            healthSlider.value = PlayerEntity.Instance.Health;
            // TODO: add hit effects

        }

        private void OnHealthIncreased()
        {
            healthSlider.value = PlayerEntity.Instance.Health;
        }
    }
}