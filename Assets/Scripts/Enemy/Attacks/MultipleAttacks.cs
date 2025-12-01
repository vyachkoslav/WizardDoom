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
            _ = AttackRoutine(cancellationToken, null, 0, data);
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
                while (true)
                {
                    if (handle?.Paused == true)
                    {
                        await Awaitable.NextFrameAsync(cancelToken);
                        continue;
                    }

                    handle?.BeforeAttackDelay();
                    await Awaitable.WaitForSecondsAsync(attackDelay, cancelToken);
                    handle?.Attacked();
                    for (int i = 0; i < attacksAmount; i++)
                    {
                        attack.AttackOnce(getAttackData, cancelToken);
                        await Awaitable.WaitForSecondsAsync(delayBetweenMultipleAttacksSeconds, cancelToken);
                    }

                    if (handle == null) break;
                    
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
