using UnityEngine;
using static UnityEngine.GraphicsBuffer;


// Note for later: doing rotation in Update instead of FixedUpdate can be smoother

public class MeleeEnemyAI : BaseEnemyAI
{
    protected override void Start()
    {
        // Use the base functionality from parent class, with addition of stopping distance
        base.Start();
        agent.stoppingDistance = 2f;
    }

    protected override void FixedUpdate()
    {
        if (GetComponent<DetectPlayer>().playerIsDetected)
        {
            target = player.transform;
            lastKnownPlayerPosition = target.position;

            // Sets nav mesh destination to player's position
            agent.SetDestination(target.position);

            // Makes the enemy rotate towards player even when stopping distance reached
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                LookAtPlayer();
            }
            else
            {
                agent.isStopped = false;
            }
        }
        else
        {
            LookAtPlayer();
        }
    }
}
