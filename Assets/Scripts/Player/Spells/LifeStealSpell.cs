using System.Collections;
using UnityEngine;

namespace Player.Spells
{
    [CreateAssetMenu(fileName = "LifestealSpell", menuName = "Spells/LifestealSpell")]
    public class LifestealSpell : Spell
    {
        [Header("Stats")]
        [SerializeField] private float _durationInSeconds;

        // Sets global bool IsLifeStealActive to true
        public override void Cast()
        {
            DataManager.Instance.IsLifeStealActive = true;
            Debug.Log("Lifesteal: " + DataManager.Instance.IsLifeStealActive);

            // Not really proud of this one... it works though
            FindAnyObjectByType<SpellController>().StartCoroutine(ActivateLifeSteal());
        }

        // Wait and then set global bool IsLifeStealActive to false
        private IEnumerator ActivateLifeSteal()
        {
            SoundManager.Instance.PlaySound2D("LifestealStart");
            yield return new WaitForSeconds(_durationInSeconds);
            DataManager.Instance.IsLifeStealActive = false;
            Debug.Log("Lifesteal: " + DataManager.Instance.IsLifeStealActive);
            SoundManager.Instance.PlaySound2D("LifestealEnd");
        }
        public override string ToString()
        {
            return "Lifesteal";
        }
    }
    
}
