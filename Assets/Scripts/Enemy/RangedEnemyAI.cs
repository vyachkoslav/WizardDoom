using System;
using Enemy;
using UnityEngine;
using UnityEngine.Assertions;

/* TODO:
- after losing line of sight of player and moving to last known player location, enemy rotates toward direction where player went
*/
public class RangedEnemyAI : BaseEnemyAI
{
    [SerializeField] private Attack attack;
    [SerializeField] private float fleeTriggerRange = 3f;
    [SerializeField] private float attackRange = 5f;
    
    private float fleeDistance;
    private bool isInAttackRange => distanceToPlayer <= attackRange;
    private bool isFleeing = false;
    private Func<Attack.AttackData> getAttackData;
    private IDisposable attackRoutine;

    protected override void Start()
    {
        base.Start();
        var selfEntity = GetComponent<IEntity>();
        getAttackData = () => new Attack.AttackData()
        {
            WeaponPosition = transform.position, // todo
            WeaponForward = transform.forward, // todo
            SelfEntity = selfEntity,
            WeaponAudio = Audio,
            TargetEntity = playerEntity,
            TargetPosition = player.transform.position,
        };
        attack.OnAttacked += OnAttacked.Invoke;
    }

    private void OnDestroy()
    {
        attackRoutine?.Dispose();
    }

    private void PerformAttack()
    {
        isFleeing = false;

        if (playerIsDetected && isInAttackRange)
        {
            if (!IsAttacking)
            {
                Assert.IsNull(attackRoutine);
                OnStartAttacking();
                agent.isStopped = true;
                attackRoutine = attack.StartAttacking(getAttackData);
            }
        } 
    }


    private void Update()
    {
        fleeDistance = attackRange - fleeTriggerRange;

        if (playerIsDetected)
        {
            lastKnownPlayerPosition = player.transform.position;

            if (hasLineOfSight)
            {
                sawPlayer = true;
            }

            if (isInAttackRange && !obstacleBlocksVision)
            {
                if (distanceToPlayer < fleeTriggerRange)
                {
                    // Makes enemy flee directly away from player by a fixed distance
                    Vector3 directionAwayFromPlayer = -(player.transform.position - transform.position).normalized;
                    Vector3 fleePosition = transform.position + directionAwayFromPlayer * fleeDistance;
                    isFleeing = true;
                    agent.SetDestination(fleePosition);
                }
                else if (!isFleeing && !IsAttacking)
                {
                    agent.ResetPath();
                    PerformAttack();
                }
            }
            else if (!isInAttackRange && !isFleeing && !IsAttacking || obstacleBlocksVision)
            {
                agent.SetDestination(player.transform.position);
            }
        }
        
        if (IsAttacking && (!isInAttackRange || isFleeing || obstacleBlocksVision))
        {
            attackRoutine.Dispose();
            attackRoutine = null;
            OnEndAttacking();
        }
        if (IsDestinationReached())
        {
            isFleeing = false;
            LookAtPlayer();

            if (sawPlayer && !hasLineOfSight)
            {
                agent.SetDestination(lastKnownPlayerPosition);
                sawPlayer = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
