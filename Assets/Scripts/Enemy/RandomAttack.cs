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
            _ = AttackRoutine(cancellationToken, data, true);
            GetRandomAttack().AttackOnce(data, cancellationToken);
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
                    GetRandomAttack().AttackOnce(getAttackData, cancelToken);

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