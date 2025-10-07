using UnityEngine;

public class RangedEnemyAI : BaseEnemyAI
{
    [SerializeField] private float fleeTriggerRange = 3f;
    private bool isFleeing = false;

    protected override void FixedUpdate()
    {
        float fleeDistance = GetComponentInChildren<AttackRange>().rangeSize - fleeTriggerRange;
        bool IsInAttackRange = GetComponentInChildren<AttackRange>().IsInAttackRange;

        if (GetComponent<DetectPlayer>().playerIsDetected)
        {
            target = player.transform;
            lastKnownPlayerPosition = target.position;

            distanceToPlayer = (target.position - transform.position).magnitude;
            //Debug.Log("distance to player: " + distanceToPlayer);

            if (IsInAttackRange)
            {
                if (distanceToPlayer < fleeTriggerRange)
                {
                    // Makes enemy flee directly away from player by a fixed distance
                    Vector3 directionAwayFromPlayer = -(target.position - transform.position).normalized;
                    Vector3 fleePosition = transform.position + directionAwayFromPlayer * fleeDistance;
                    agent.isStopped = false; // remove when attack implemented
                    isFleeing = true;
                    agent.SetDestination(fleePosition);
                    //Debug.Log("fleeing");
                }
                else if (!isFleeing)
                {
                    LookAtPlayer();
                    // Insert attack here
                    agent.isStopped = true; // placeholder
                    //Debug.Log("Attack");
                }

            }
            else if (!IsInAttackRange && !isFleeing)
            {
                agent.isStopped = false; // remove when attack implemented
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

            LookAtPlayer();
        }
    }
}
