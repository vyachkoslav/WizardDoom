using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Random = UnityEngine.Random;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemySpawn attack", menuName = "Attacks/EnemySpawn Attack", order = 0)]
    public class EnemySpawnAttack : Attack
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float distanceBetweenSpawns;
        [SerializeField] private int enemyAmount;
        [SerializeField] private float yPositionForSpawn;

        public override void AttackOnce(Func<AttackData> data, CancellationToken cancellationToken)
        {
            CreateEnemies(data());
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
                    handle.Attacked();
                    CreateEnemies(getAttackData());
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

        private void CreateEnemies(AttackData data)
        {
            var width = (enemyAmount - 1) * distanceBetweenSpawns;
            var targetDir = (data.TargetPosition - data.WeaponPosition).WithY(0);
            var middlePoint = (targetDir / 2 + data.WeaponPosition).WithY(yPositionForSpawn);
            var rightDir = Vector3.Cross(targetDir, Vector3.up).normalized;
            var startPos = middlePoint + rightDir * (width / 2);
            for (int i = 0; i < enemyAmount; ++i)
            {
                var pos = startPos + -rightDir * (i * distanceBetweenSpawns);
                var rotation = Quaternion.LookRotation((data.TargetPosition - pos).WithY(0));
                Instantiate(enemyPrefab, pos, rotation);
            }
        }
    }
}
