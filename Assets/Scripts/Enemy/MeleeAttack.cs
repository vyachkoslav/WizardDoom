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

        public override IDisposable StartAttacking(Func<AttackData> attackData)
        {
            var cancelableAttack = new CancelableAttack();
            _ = AttackRoutine(cancelableAttack.Token, attackData);
            return cancelableAttack;
        }

        private async Awaitable AttackRoutine(CancellationToken cancelToken, Func<AttackData> getAttackData)
        {
            try
            {
                var lastAttack = float.MinValue;
                while (true)
                {
                    await Awaitable.NextFrameAsync(cancelToken);

                    if (Time.time - lastAttack < DelayBetweenAttacks) 
                        continue;
                    
                    var attackData = getAttackData();
                    if (Vector3.Distance(attackData.TargetPosition, attackData.WeaponPosition) > attackRange)
                        continue;
                    
                    lastAttack = Time.time;

                    Attack(attackData);
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