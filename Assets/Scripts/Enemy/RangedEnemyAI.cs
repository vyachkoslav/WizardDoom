using UnityEngine;

public class RangedEnemyAI : BaseEnemyAI
{
    [SerializeField] private float fleeTriggerRange = 3f;
    [SerializeField] private float attackRange = 5f;
    
    private float fleeDistance;
    private bool isInAttackRange => distanceToPlayer <= attackRange;
    private bool isFleeing = false;

    private void PerformAttack()
    {
        isFleeing = false;

        if (playerIsDetected && isInAttackRange)
        {
            if (!IsAttacking)
            {
                OnStartAttacking();
                agent.isStopped = true;
                //Debug.Log("Ranged attack");

                // insert attack method from other script here

                OnEndAttacking();
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
                    //Debug.Log("fleeing");
                }
                else if (!isFleeing)
                {
                    agent.ResetPath();
                    PerformAttack();
                }

            }
            else if (!isInAttackRange && !isFleeing && !IsAttacking)
            {
                agent.SetDestination(player.transform.position);
                //Debug.Log("Moving to attack range");
            }
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
