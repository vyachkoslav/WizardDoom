using System;
using System.Threading;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Enemy
{
    [CreateAssetMenu(fileName = "AOE attack", menuName = "Attacks/AOE Attack", order = 0)]
    public class AOEAttack : Attack
    {
        [SerializeField] private GameObject aoePrefab;
        [SerializeField] private float aoeDuration;
        [SerializeField] private float aoeDelayBetweenDamage;
        [SerializeField] private float delayBeforeDamage;
        [SerializeField] private float aoeSpawnMaxDispersion;
        [SerializeField] private float aoeRange;
        [SerializeField] private float yPositionForAoe;

        public override void AttackOnce(Func<AttackData> data, CancellationToken cancellationToken)
        {
            CreateAOE(data());
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
                    if (handle.Paused)
                    {
                        await Awaitable.NextFrameAsync(cancelToken);
                        continue;
                    }
                    handle.BeforeAttackDelay();
                    await Awaitable.WaitForSecondsAsync(attackDelay, cancelToken);
                    if (handle.Paused) continue;
                    
                    handle.Attacked();
                    CreateAOE(getAttackData());
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

        private void CreateAOE(AttackData data)
        {
            var aoe = Instantiate(aoePrefab).GetComponent<AOEDamage>();
            
            var targetPos = data.TargetPosition + Random.insideUnitSphere * aoeSpawnMaxDispersion;
            targetPos.y = yPositionForAoe;
            aoe.transform.position = targetPos;
            
            aoe.Duration = aoeDuration;
            aoe.Damage = Damage;
            aoe.DelayBetweenDamage = aoeDelayBetweenDamage;
            aoe.DelayBeforeDamage = delayBeforeDamage;
            aoe.SetRange(aoeRange);
            aoe.EnableDamage();
        }
    }
}
