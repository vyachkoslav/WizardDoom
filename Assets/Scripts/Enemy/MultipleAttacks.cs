using System;
using System.Threading;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Multiple attacks", menuName = "Attacks/Multiple attacks", order = 0)]
    public class MultipleAttacks : Attack
    {
        [SerializeField] private Attack attack;
        
        [SerializeField] private int attacksAmount;
        [SerializeField] private float delayBetweenMultipleAttacksSeconds;
        
        public override void AttackOnce(Func<AttackData> data, CancellationToken cancellationToken)
        {
            _ = AttackRoutine(cancellationToken, data, true);
        }

        public override IDisposable StartAttacking(Func<AttackData> attackData)
        {
            var cancellableAttack = new CancelableAttack();
            _ = AttackRoutine(cancellableAttack.Token, attackData, false);
            return cancellableAttack;
        }
        
        private async Awaitable AttackRoutine(CancellationToken cancelToken, Func<AttackData> getAttackData, bool once)
        {
            try
            {
                while (true)
                {
                    for (int i = 0; i < attacksAmount; i++)
                    {
                        attack.AttackOnce(getAttackData, cancelToken);
                        await Awaitable.WaitForSecondsAsync(delayBetweenMultipleAttacksSeconds, cancelToken);
                    }

                    if (once) break;
                    
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
    }
}