using TMPro;
using UnityEngine;

namespace Player.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;

        private void Start()
        {
            PlayerEntity.Instance.OnHealthDecreased += OnHealthDecreased;
            healthText.text = PlayerEntity.Instance.Health.ToString();
        }
        
        private void OnHealthDecreased()
        {
            healthText.text = PlayerEntity.Instance.Health.ToString();
            // TODO: add hit effects
        }
    }
}