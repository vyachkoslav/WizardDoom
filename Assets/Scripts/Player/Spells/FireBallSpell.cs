using UnityEngine;

namespace Player.Spells
{
    [CreateAssetMenu(fileName = "FireBallSpell", menuName = "Spells/FireBallSpell")]
    public class FireBallSpell : Spell
    {
        [Header("Stats")]
        [SerializeField] private float _explosionDamage;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionDurationInSeconds;

        private Transform _projectileSpawn;
        private Camera _mainCamera;

        public override void Cast()
        {
            _projectileSpawn = FindAnyObjectByType<SpellController>().ProjectileSpawn;
            _mainCamera = FindAnyObjectByType<Camera>();

            GameObject fireBall = Instantiate(spellObject, _projectileSpawn.position, _projectileSpawn.rotation);
            fireBall.GetComponent<FireBallProjectile>().Spawn(_damage, _explosionDamage, _explosionRadius, _explosionDurationInSeconds, _moveSpeed, _durationInSeconds, _mainCamera.transform.forward);
        }
    }
}
