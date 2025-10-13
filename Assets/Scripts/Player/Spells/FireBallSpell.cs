using UnityEngine;

namespace Player.Spells
{
    [CreateAssetMenu(fileName = "FireBallSpell", menuName = "Spells/FireBallSpell")]
    public class FireBallSpell : Spell
    {
        [Header("Stats")]
        [SerializeField] private float _explosionDamage;
        [SerializeField] private float _explosionRadius;

        public override void Cast(Camera camera, Transform projectileSpawn)
        {
            GameObject fireBall = Instantiate(spellObject, projectileSpawn.position, projectileSpawn.rotation);
            fireBall.GetComponent<FireBallProjectile>().Spawn(_damage, _explosionDamage, _explosionRadius, _moveSpeed, _durationInSeconds, camera.transform.forward);
            Debug.Log("Fireball");
        }
    }
}
