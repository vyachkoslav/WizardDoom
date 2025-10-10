using UnityEngine;
using static UnityEngine.GraphicsBuffer;


// Note for later: doing rotation in Update instead of FixedUpdate can be smoother

// TODO:
// - make enemy only attack when facing player and only rotate towards player when not attacking
// - maybe: move entire body of PerformAttack() to BaseEnemyAI, set attack range for melee to agent.stoppingDistance and make attack range checking shared
public class MeleeEnemyAI : BaseEnemyAI
{
    protected override void Start()
    {
        // Use the base functionality from parent class, with addition of stopping distance
        base.Start();
        agent.stoppingDistance = 2f;
    }

    protected override void PerformAttack()
    {
        if (playerIsDetected && distanceToPlayer <= agent.stoppingDistance)
        {
            if (!attackInProgress)
            {
                attackInProgress = true;
                agent.isStopped = true;
                //Debug.Log("Melee attack");

                // insert attack method from other script here

                StartCoroutine("StopAttacking");
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (playerIsDetected)
        {
            target = player.transform;
            lastKnownPlayerPosition = target.position;

            distanceToPlayer = (target.position - transform.position).magnitude;

            if (!attackInProgress)
            {
                // Sets nav mesh destination to player's position
                agent.SetDestination(target.position);
            }

            // This check is made regardless of whether attack is in progress
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                // Makes the enemy rotate towards player even when stopping distance reached
                LookForPlayer();
                PerformAttack();
            }
            else
            {
                agent.isStopped = false;
            }
        }
        else
        {
            LookForPlayer();
        }
    }
}
