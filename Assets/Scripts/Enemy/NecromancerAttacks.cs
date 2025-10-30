using System;
using System.Threading;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Necromancer attacks", menuName = "Attacks/Necromancer attacks", order = 0)]
    public class NecromancerAttacks : Attack
    {
        [SerializeField] private Attack rangedAttack;
        
        public override void AttackOnce(AttackData attackData)
        {
            rangedAttack.AttackOnce(attackData);
        }

        public override IDisposable StartAttacking(Func<AttackData> attackData)
        {
            var cancellableAttack = new CancelableAttack();
            _ = AttackRoutine(cancellableAttack.Token, attackData);
            return cancellableAttack;
        }
        
        private async Awaitable AttackRoutine(CancellationToken cancelToken, Func<AttackData> getAttackData)
        {
            try
            {
                var lastShot = float.MinValue;
                while (true)
                {
                    await Awaitable.NextFrameAsync(cancelToken);

                    if (Time.time - lastShot < DelayBetweenAttacks) continue;
                    lastShot = Time.time;
                    
                    rangedAttack.AttackOnce(getAttackData());
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
    }
}