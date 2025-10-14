using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Ranged projectile attack", menuName = "Attacks/Ranged projectile", order = 0)]
    public class RangedProjectileAttack : Attack
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float projectileRange;
        
        private Func<AttackData> getAttackData;
        private CancellationTokenSource cancelSource;
        private Func<Collider, bool> onProjectileCollided;

        private bool OnProjectileCollided(Collider other)
        {
            if (!other.isTrigger)
            {
                var entity = other.GetComponentInParent<IEntity>();
                if (entity != getAttackData().SelfEntity)
                    entity?.ApplyDamage(Damage);
                else
                    return false;
            }
            return !other.isTrigger;
        }
        
        public override void StartAttacking(Func<AttackData> attackData)
        {
            Assert.IsTrue(cancelSource == null || cancelSource.IsCancellationRequested);
            cancelSource = new();
            getAttackData = attackData;
            onProjectileCollided = OnProjectileCollided;
            _ = AttackRoutine(cancelSource.Token);
        }
        
        private async Awaitable AttackRoutine(CancellationToken cancelToken)
        {
            try
            {
                var lastShot = float.MinValue;
                while (true)
                {
                    await Awaitable.NextFrameAsync(cancelToken);

                    if (Time.time - lastShot < DelayBetweenAttacks) continue;
                    lastShot = Time.time;

                    Attack(getAttackData());
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void Attack(AttackData data)
        {
            data.WeaponAudio.PlayOneShot(AttackSound);
            var direction = (data.TargetPosition - data.WeaponPosition).normalized;
            LinearProjectile.Spawn(projectilePrefab, 
                data.WeaponPosition, direction, Quaternion.LookRotation(direction),
                projectileSpeed, projectileRange, onProjectileCollided);
            InvokeAttacked();
        }

        public override void StopAttacking()
        {
            Assert.IsFalse(cancelSource == null || cancelSource.IsCancellationRequested);
            cancelSource.Cancel();
        }
    }
}