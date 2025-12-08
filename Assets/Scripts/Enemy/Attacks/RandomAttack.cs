using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Random attack", menuName = "Attacks/Random Attack", order = 0)]
    public class RandomAttack : Attack
    {
        [SerializeField] private List<Attack> attacks;

        private Attack GetRandomAttack()
        {
            return attacks[Random.Range(0, attacks.Count)];
        }
        
        public override void AttackOnce(Func<AttackData> data, CancellationToken cancellationToken)
        {
            _ = AttackRoutine(cancellationToken, null, 0, data);
            GetRandomAttack().AttackOnce(data, cancellationToken);
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
                    if (handle?.Paused == true) continue;
                    
                    handle?.Attacked();
                    GetRandomAttack().AttackOnce(getAttackData, cancelToken);

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
