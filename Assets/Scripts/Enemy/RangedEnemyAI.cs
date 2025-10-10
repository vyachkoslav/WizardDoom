using UnityEngine;

public class RangedEnemyAI : BaseEnemyAI
{
    [SerializeField] private float fleeTriggerRange = 3f;
    float fleeDistance;
    bool IsInAttackRange;
    private bool isFleeing = false;


    protected override void PerformAttack()
    {
        isFleeing = false;

        if (playerIsDetected && IsInAttackRange)
        {
            if (!attackInProgress)
            {
                attackInProgress = true;
                agent.isStopped = true;
                //Debug.Log("Ranged attack");

                // insert attack method from other script here

                StartCoroutine("StopAttacking");
            }
        } 
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        fleeDistance = GetComponentInChildren<AttackRange>().rangeSize - fleeTriggerRange;
        IsInAttackRange = GetComponentInChildren<AttackRange>().IsInAttackRange;

        if (playerIsDetected)
        {
            target = player.transform;
            lastKnownPlayerPosition = target.position;

            distanceToPlayer = (target.position - transform.position).magnitude;
            //Debug.Log("distance to player: " + distanceToPlayer);

            if (IsInAttackRange)
            {
                if (distanceToPlayer < fleeTriggerRange && !attackInProgress)
                {
                    // Makes enemy flee directly away from player by a fixed distance
                    Vector3 directionAwayFromPlayer = -(target.position - transform.position).normalized;
                    Vector3 fleePosition = transform.position + directionAwayFromPlayer * fleeDistance;
                    isFleeing = true;
                    agent.SetDestination(fleePosition);
                    //Debug.Log("fleeing");
                }
                else if (!isFleeing)
                {
                    agent.ResetPath();
                    LookForPlayer();
                    PerformAttack();
                }

            }
            else if (!IsInAttackRange && !isFleeing && !attackInProgress)
            {
                agent.SetDestination(target.position);
                //Debug.Log("Moving to attack range");
            }
        }
        
        if (CheckIfDestinationReached())
        {
            if (isFleeing)
            {
                isFleeing = false;
            }

            LookForPlayer();
        }
    }
}
