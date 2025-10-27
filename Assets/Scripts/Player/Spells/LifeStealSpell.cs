using System.Collections;
using UnityEngine;

namespace Player.Spells
{
    [CreateAssetMenu(fileName = "LifestealSpell", menuName = "Spells/LifestealSpell")]
    public class LifestealSpell : Spell
    {
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
            yield return new WaitForSeconds(_durationInSeconds);
            DataManager.Instance.IsLifeStealActive = false;
            Debug.Log("Lifesteal: " + DataManager.Instance.IsLifeStealActive);
        }
    }
}
