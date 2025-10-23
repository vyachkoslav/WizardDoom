using System.Collections;
using UnityEngine;

namespace Player.Spells
{
    [CreateAssetMenu(fileName = "LifeStealSpell", menuName = "Spells/LifeStealSpell")]
    public class LifeStealSpell : Spell
    {

        public override void Cast()
        {
            DataManager.Instance.IsLifeStealActive = true;
            Debug.Log("Lifesteal: " + DataManager.Instance.IsLifeStealActive);

            // Not really proud of this one... it works though
            FindAnyObjectByType<SpellController>().StartCoroutine(ActivateLifeSteal());
        }
        
        private IEnumerator ActivateLifeSteal()
        {
            yield return new WaitForSeconds(_durationInSeconds);
            DataManager.Instance.IsLifeStealActive = false;
            Debug.Log("Lifesteal: " + DataManager.Instance.IsLifeStealActive);
        }
    }
}
