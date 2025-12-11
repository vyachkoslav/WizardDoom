using System;
using Enemy;
using UnityEngine;
using UnityEngine.Assertions;
using Utils;
using static Enemy.Attack;

public class BossAI : BaseEnemyAI
{
    [SerializeField] protected Attack attack;
    
    [SerializeField] protected float fleeTriggerRange = 3f;
    [SerializeField] protected float attackRange = 5f;
    protected float yAxisOffset = 1f;

    protected float fleeDistance;
    protected bool isInAttackRange => distanceToPlayer <= attackRange;
    protected bool isFleeing = false;
    protected Func<AttackData> getAttackData;
    protected CancelableAttack attacker;

    private Entity entity;
    [SerializeField] private SpellPickup spellPickup;

    protected override void Start()
    {
        base.Start();
        var selfEntity = GetComponent<IEntity>();
        var playerCharController = player.GetComponent<CharacterController>();
        entity = GetComponent<Entity>();

        if (gameObject.CompareTag("Miniboss"))
        {
            entity.OnDeath += DropSpellPickup;
        }

        getAttackData = () => new AttackData()
        {
            WeaponPosition = transform.position.WithY(transform.position.y + yAxisOffset),
            WeaponForward = transform.forward,
            SelfEntity = selfEntity,
            TargetEntity = playerEntity,
            TargetPosition = player.transform.position,
            TargetSpeed = playerCharController.velocity,
        };
        attacker = attack.CreateAttacker(getAttackData, delayBeforeAttack);
        attacker.OnAttack += OnAttacked.Invoke;
        attacker.OnBeforeAttackDelay += OnBeforeAttackDelay.Invoke;

        attacker.OnBeforeAttackDelay += StartAttackAnimation;
    }

    protected void OnDisable()
    {
        if (!IsAttacking) return;

        OnEndAttacking();
        attacker?.Pause();
    }

    protected void OnDestroy()
    {
        attacker?.Dispose();
    }

    protected void PerformAttack()
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


    protected virtual void Update()
    {
        bool isWalking = agent.velocity.sqrMagnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);

        fleeDistance = attackRange - fleeTriggerRange;

        if (playerIsDetected)
        {
            LookAtPlayer();
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
                else if (!IsAttacking)
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
        else { agent.SetDestination(startPosition);}
        
        if (IsAttacking && (!isInAttackRange || obstacleBlocksVision))
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

    private void DropSpellPickup()
    {
        Vector3 dropLocation = transform.position.WithY(transform.position.y + 1);
        Instantiate(spellPickup, dropLocation, spellPickup.transform.rotation);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
