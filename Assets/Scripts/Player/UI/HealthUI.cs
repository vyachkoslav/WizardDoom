using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Player.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TMP_Text healthText;

        [Header("Damage and heal visuals")]
        [SerializeField] private Image damageFlash;
        [SerializeField] private Image healFlash;
        [SerializeField] private float flashDuration;
        [SerializeField] private float fadeSpeed;

        private float damageFlashDurationTimer;
        private float damageMaxAlpha;

        private float healFlashDurationTimer;
        private float healmaxAlpha;

        private void Start()
        {
            PlayerEntity.Instance.OnHealthDecreased += OnHealthDecreased;
            PlayerEntity.Instance.OnHealthIncreased += OnHealthIncreased;

            healthSlider.minValue = 0;
            healthSlider.maxValue = PlayerEntity.Instance.MaxHealth;

            healthSlider.value = PlayerEntity.Instance.Health;

            healthText.text = "Health: " + healthSlider.value + "/" + healthSlider.maxValue;

            // Store max alpha value and then set damage and heal flash invisible
            damageMaxAlpha = damageFlash.color.a;
            damageFlash.color = new Color(damageFlash.color.r, damageFlash.color.g, damageFlash.color.b, 0);

            healmaxAlpha = healFlash.color.a;
            healFlash.color = new Color(healFlash.color.r, healFlash.color.g, healFlash.color.b, 0);
        }

        private void Update()
        {
            // If damageFlash is visible, start fading after flash duration
            if (damageFlash.color.a > 0)
            {
                damageFlashDurationTimer += Time.deltaTime;

                if (damageFlashDurationTimer >= flashDuration)
                {
                    float newAlpha = damageFlash.color.a;
                    newAlpha -= fadeSpeed * Time.deltaTime;
                    damageFlash.color = new Color(damageFlash.color.r, damageFlash.color.g, damageFlash.color.b, newAlpha);
                }
            }

            // If heal flash is visible, start fading after duration
            if (healFlash.color.a > 0)
            {
                healFlashDurationTimer += Time.deltaTime;

                if (healFlashDurationTimer >= flashDuration)
                {
                    float newAlpha = healFlash.color.a;
                    newAlpha -= fadeSpeed * Time.deltaTime;
                    healFlash.color = new Color(healFlash.color.r, healFlash.color.g, healFlash.color.b, newAlpha);
                }
            }
        }

        private void OnHealthDecreased()
        {
            healthSlider.value = PlayerEntity.Instance.Health;
            UpdateMaxHealth();

            // damageFlash visible, reset duration timer for Update()
            damageFlash.color = new Color(damageFlash.color.r, damageFlash.color.g, damageFlash.color.b, damageMaxAlpha);
            damageFlashDurationTimer = 0;

            // Make heal flash invisible
            healFlash.color = new Color(healFlash.color.r, healFlash.color.g, healFlash.color.b, 0);
            healFlashDurationTimer = 0;
        }

        private void OnHealthIncreased()
        {
            healthSlider.value = PlayerEntity.Instance.Health;
            UpdateMaxHealth();

            // Heal flash visible, reset timer
            healFlash.color = new Color(healFlash.color.r, healFlash.color.g, healFlash.color.b, healmaxAlpha);
            healFlashDurationTimer = 0;

            // Make damage flash invisible
            damageFlash.color = new Color(damageFlash.color.r, damageFlash.color.g, damageFlash.color.b, 0);
            damageFlashDurationTimer = 0;
        }

        public void UpdateMaxHealth()
        {
            healthSlider.maxValue = PlayerEntity.Instance.MaxHealth;
            healthText.text = "Health: " + healthSlider.value + "/" + healthSlider.maxValue;
        }
    }
}