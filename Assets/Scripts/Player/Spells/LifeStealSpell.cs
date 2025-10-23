using System.Collections;
using UnityEngine;

namespace Player.Spells
{
    [CreateAssetMenu(fileName = "LifeStealSpell", menuName = "Spells/LifeStealSpell")]
    public class LifeStealSpell : Spell
    {
        // Accessed when dealing dmg to enemies
        public static bool isLifeStealActive = false;

        public override void Cast()
        {
            isLifeStealActive = true;
            Debug.Log("Lifesteal: " + isLifeStealActive);

            // Not really proud of this one... it works though
            FindAnyObjectByType<SpellController>().StartCoroutine(ActivateLifeSteal());
        }
        
        private IEnumerator ActivateLifeSteal()
        {
            yield return new WaitForSeconds(_durationInSeconds);
            isLifeStealActive = false;
            Debug.Log("Lifesteal: " + isLifeStealActive);
        }
    }
}
