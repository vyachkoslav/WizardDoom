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
                    var entity = other.GetComponentInParent<Entity>();
                    if (entity != (object)getAttackData().SelfEntity)
                        entity?.ApplyDamage(Damage);
                    else
                        return false;
                }

                return !other.isTrigger;
            };
        }

        public override void AttackOnce(Func<AttackData> data, CancellationToken cancellationToken)
        {
            var collisionHandler = CreateCollisionHandler(data);
            Attack(data(), collisionHandler);
        }

        public override CancelableAttack CreateAttacker(Func<AttackData> attackData, float attackDelay)
        {
            var handle = new CancelableAttack.Handle();
            var cancelableAttack = new CancelableAttack(handle);
            _ = AttackRoutine(cancelableAttack.Token, handle, attackDelay, attackData);
            return cancelableAttack;
        }
        
        private async Awaitable AttackRoutine(CancellationToken cancelToken, 
                CancelableAttack.Handle handle,
                float attackDelay,
                Func<AttackData> getAttackData)
        {
            var collisionHandler = CreateCollisionHandler(getAttackData);
            try
            {
                while (true)
                {
                    if (handle.Paused)
                    {
                        await Awaitable.NextFrameAsync(cancelToken);
                        continue;
                    }

                    handle.BeforeAttackDelay();
                    await Awaitable.WaitForSecondsAsync(attackDelay, cancelToken);
                    if (handle.Paused) continue;
                    
                    handle.Attacked();
                    Attack(getAttackData(), collisionHandler);

                    await Awaitable.WaitForSecondsAsync(DelayBetweenAttacks, cancelToken);
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
            var direction = data.TargetPosition - data.WeaponPosition;
            var timeToHit = direction.magnitude / projectileSpeed; // not accurate
            var predictedPosition = data.TargetPosition + data.TargetSpeed * (timeToHit * 0.5f);
            var directionToPrediction = (predictedPosition - data.WeaponPosition).normalized;
            LinearProjectile.Spawn(projectilePrefab,
                data.WeaponPosition, directionToPrediction, Quaternion.LookRotation(direction),
                projectileSpeed, projectileRange, collisionHandler);
        }
    }
}
