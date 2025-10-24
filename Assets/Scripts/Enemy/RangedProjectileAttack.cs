using System;
using System.Threading;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Ranged projectile attack", menuName = "Attacks/Ranged projectile", order = 0)]
    public class RangedProjectileAttack : Attack
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float projectileRange;

        private Func<Collider, bool> CreateCollisionHandler(Func<AttackData> getAttackData)
        {
            return other =>
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
            };
        }
        
        public override IDisposable StartAttacking(Func<AttackData> attackData)
        {
            var cancellableAttack = new CancelableAttack();
            _ = AttackRoutine(cancellableAttack.Token, attackData);
            return cancellableAttack;
        }
        
        private async Awaitable AttackRoutine(CancellationToken cancelToken, Func<AttackData> getAttackData)
        {
            var collisionHandler = CreateCollisionHandler(getAttackData);
            try
            {
                var lastShot = float.MinValue;
                while (true)
                {
                    await Awaitable.NextFrameAsync(cancelToken);

                    if (Time.time - lastShot < DelayBetweenAttacks) continue;
                    lastShot = Time.time;

                    Attack(getAttackData(), collisionHandler);
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

        private void Attack(AttackData data, Func<Collider, bool> collisionHandler)
        {
            SoundManager.Instance.PlaySound3D("EnemyRangedAttack", data.WeaponPosition);
            var direction = (data.TargetPosition - data.WeaponPosition).normalized;
            LinearProjectile.Spawn(projectilePrefab,
                data.WeaponPosition, direction, Quaternion.LookRotation(direction),
                projectileSpeed, projectileRange, collisionHandler);
            InvokeAttacked();
        }
    }
}