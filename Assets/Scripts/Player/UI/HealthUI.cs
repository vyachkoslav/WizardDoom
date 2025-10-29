using UnityEngine.UI;
using UnityEngine;

namespace Player.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image damageFlash;
        [SerializeField] private float flashDuration;
        [SerializeField] private float flashSpeed;

        private float flashDurationTimer;
        private float maxAlpha;

        private void Start()
        {
            PlayerEntity.Instance.OnHealthDecreased += OnHealthDecreased;
            PlayerEntity.Instance.OnHealthIncreased += OnHealthIncreased;

            healthSlider.minValue = 0;
            healthSlider.maxValue = PlayerEntity.Instance.MaxHealth;

            healthSlider.value = PlayerEntity.Instance.Health;

            maxAlpha = damageFlash.color.a;
            damageFlash.color = new Color(damageFlash.color.r, damageFlash.color.g, damageFlash.color.b, 0);
        }

        // If damageFlash is visible, start fading after flash duration
        private void Update()
        {
            if (damageFlash.color.a > 0)
            {
                flashDurationTimer += Time.deltaTime;

                if (flashDurationTimer >= flashDuration)
                {
                    float newAlpha = damageFlash.color.a;
                    newAlpha -= flashSpeed * Time.deltaTime;
                    damageFlash.color = new Color(damageFlash.color.r, damageFlash.color.g, damageFlash.color.b, newAlpha);
                }
            }
        }

        private void OnHealthDecreased()
        {
            healthSlider.value = PlayerEntity.Instance.Health;

            // damageFlash visible, reset duration timer for Update()
            damageFlash.color = new Color(damageFlash.color.r, damageFlash.color.g, damageFlash.color.b, maxAlpha);
            flashDurationTimer = 0;

        }

        private void OnHealthIncreased()
        {
            healthSlider.value = PlayerEntity.Instance.Health;
        }
    }
}