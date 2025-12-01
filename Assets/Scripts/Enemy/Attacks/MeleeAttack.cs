using System;
using System.Threading;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Melee attack", menuName = "Attacks/Melee", order = 0)]
    public class MeleeAttack : Attack
    {
        [SerializeField] private float attackRange;
        
        public override void AttackOnce(Func<AttackData> getAttackData, CancellationToken cancellationToken)
        {
            var attackData = getAttackData();
            if (Vector3.Distance(attackData.TargetPosition, attackData.WeaponPosition) <= attackRange)
                Attack(attackData);
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
            try
            {
                var lastAttack = float.MinValue;
                while (true)
                {
                    if (handle.Paused)
                    {
                        await Awaitable.NextFrameAsync(cancelToken);
                        continue;
                    }
                    
                    var attackData = getAttackData();
                    if (Vector3.Distance(attackData.TargetPosition, attackData.WeaponPosition) > attackRange)
                    {
                        await Awaitable.NextFrameAsync(cancelToken);
                        continue;
                    }
                    
                    lastAttack = Time.time;
                    
                    handle.BeforeAttackDelay();
                    await Awaitable.WaitForSecondsAsync(attackDelay, cancelToken);
                    if (Vector3.Distance(attackData.TargetPosition, attackData.WeaponPosition) > attackRange)
                        continue;
                    handle.Attacked();
                    Attack(attackData);

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

        private void Attack(AttackData data)
        {
            SoundManager.Instance.PlaySound3D("EnemyMeleeAttack", data.WeaponPosition);
            data.TargetEntity.ApplyDamage(Damage);
        }
    }
}
