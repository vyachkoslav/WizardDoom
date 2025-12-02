using System;
using Enemy;
using UnityEngine;
using UnityEngine.Assertions;
using static Enemy.Attack;

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
    private Func<AttackData> getAttackData;
    private CancelableAttack attacker;

    protected override void Start()
    {
        base.Start();
        var selfEntity = GetComponent<IEntity>();
        var playerCharController = player.GetComponent<CharacterController>();
        getAttackData = () => new AttackData()
        {
            WeaponPosition = transform.position, // todo
            WeaponForward = transform.forward, // todo
            SelfEntity = selfEntity,
            TargetEntity = playerEntity,
            TargetPosition = player.transform.position,
            TargetSpeed = playerCharController.velocity
        };
        attacker = attack.CreateAttacker(getAttackData, delayBeforeAttack);
        attacker.OnAttack += OnAttacked.Invoke;
        attacker.OnBeforeAttackDelay += OnBeforeAttackDelay.Invoke;

        // Sets bool for animator
        attacker.OnBeforeAttackDelay += StartAttackAnimation;
    }

    private void OnDisable()
    {
        if (!IsAttacking) return;

        OnEndAttacking();
        attacker?.Pause();

        attacker.OnBeforeAttackDelay -= StartAttackAnimation;
    }

    private void OnDestroy()
    {
        attacker?.Dispose();
    }

    private void PerformAttack()
    {
        isFleeing = false;

        if (playerIsDetected && isInAttackRange)
        {
            if (!IsAttacking)
            {
                OnStartAttacking();
                agent.isStopped = true;
                attacker.Start();
            }
        } 
    }


    private void Update()
    {
        bool isWalking = agent.velocity.sqrMagnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);

        fleeDistance = attackRange - fleeTriggerRange;

        if (myRoom?.IsPlayerInRoom != false)
        {
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
                attacker.Pause();
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
        else { agent.SetDestination(startLocation); }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
