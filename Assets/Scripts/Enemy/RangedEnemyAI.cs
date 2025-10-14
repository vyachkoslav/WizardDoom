using System;
using Enemy;
using UnityEngine;

/* TODO:
- make ranged enemy pursue player's last known location if it loses line of sight of player
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

    private void PerformAttack()
    {
        isFleeing = false;

        if (playerIsDetected && isInAttackRange)
        {
            if (!IsAttacking)
            {
                OnStartAttacking();
                agent.isStopped = true;
                attack.StartAttacking(getAttackData);
            }
        } 
    }


    private void Update()
    {
        fleeDistance = attackRange - fleeTriggerRange;

        if (playerIsDetected)
        {
            lastKnownPlayerPosition = player.transform.position;

            if (isInAttackRange)
            {
                if (distanceToPlayer < fleeTriggerRange && !IsAttacking)
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
            else if (!isInAttackRange && !isFleeing && !IsAttacking)
            {
                agent.SetDestination(player.transform.position);
            }
        }
        
        // attacking when shouldn't
        if ((!isInAttackRange || isFleeing) && IsAttacking)
        {
            attack.StopAttacking();
            OnEndAttacking();
        }
        if (IsDestinationReached())
        {
            isFleeing = false;
            LookAtPlayer();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
