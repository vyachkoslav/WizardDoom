using System.Collections;
using UnityEngine;
using Utils;

public class FinalBossAI : BossAI
{
    private Collider bossCollider;
    private GameObject model;

    private bool levitating = false;
    private bool levitationComplete = false;
    private float levitationDuration = 1.75f;
    private float totalLevitationHeight = 0;

    protected override void Start()
    {
        base.Start();

        bossCollider = gameObject.transform.Find("Collider").GetComponent<Collider>();
        model = gameObject.transform.Find("Model").gameObject;
    }

    protected override void Update()
    {
        fleeDistance = attackRange - fleeTriggerRange;

        if (playerIsDetected)
        {
            animator.SetBool("detectedPlayer", true);
            if (!levitating && !levitationComplete)
            {   
                StartCoroutine("LevitationSequence");
            }

            LookAtPlayer();
            lastKnownPlayerPosition = player.transform.position;

            if (hasLineOfSight)
            {
                sawPlayer = true;
            }

            if (isInAttackRange && !obstacleBlocksVision && levitationComplete)
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

    private IEnumerator LevitationSequence()
    {
        levitating = true;
        float timeStep = 0.1f;
        float levitateAmount = 0.1f;

        // Enable if taking damage can be tied to child object collider
        for (float i = 0; i < levitationDuration; i += timeStep)
        {
            totalLevitationHeight += levitateAmount;

            Vector3 colliderPos = bossCollider.transform.position;
            Vector3 modelPos = model.transform.position;

            bossCollider.transform.position = colliderPos.WithY(colliderPos.y + levitateAmount);
            model.transform.position = modelPos.WithY(modelPos.y + levitateAmount);

            yield return new WaitForSeconds(timeStep);
        }

        yAxisOffset += totalLevitationHeight;

        // ...else, enable this
        //yield return new WaitForSeconds(levitationDuration);

        levitationComplete = true;
    }
}
