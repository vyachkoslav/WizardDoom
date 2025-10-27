using UnityEngine;

namespace Player.Spells
{
    [CreateAssetMenu(fileName = "TimefreezeSpell", menuName = "Spells/TimefreezeSpell")]
    public class TimefreezeSpell : Spell
    {
        [Header("Stats")]
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _freezeDurationInSeconds;

        private Transform _projectileSpawn;
        private Camera _mainCamera;

        public override void Cast()
        {
            _projectileSpawn = FindAnyObjectByType<SpellController>().ProjectileSpawn;
            _mainCamera = FindAnyObjectByType<Camera>();

            GameObject projectile = Instantiate(spellObject, _projectileSpawn.position, _projectileSpawn.rotation);
            projectile.GetComponent<TimefreezeProjectile>().Spawn(
                _explosionRadius, _freezeDurationInSeconds,
                _moveSpeed, _mainCamera.transform.forward
            );
        }
    }
}