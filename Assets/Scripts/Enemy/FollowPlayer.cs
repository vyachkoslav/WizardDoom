using UnityEngine;
using UnityEngine.AI;


//TO BE ADDED LATER: 
// - split this script in two, melee and ranged version
//  * create a separate RangedEnemyBehaviour script, use global variables, define methods for fleeing etc
// - cleanup

public class FollowPlayer : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    private Transform target;
    private string selfType;
    private Vector3 lastKnownPlayerPosition;
    private float distanceToPlayer;
    private float rotationSpeed = 240f;
    private bool IsFleeing = false;

    void Start()
    {
        // Gets the nav mesh agent of the object this script is attached to
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        selfType = gameObject.tag;
    }

    void Update()
    {
        if (selfType == "EnemyMelee")
        {
            // Fetches whether player is detecteed from DetectPlayer script
            if (GetComponent<DetectPlayer>().playerIsDetected)
            {
                target = player.transform;
                lastKnownPlayerPosition = target.position;

                // Sets nav mesh destination to player's position
                agent.SetDestination(target.position);
            }
        }
        else if (selfType == "EnemyRanged")
        {
            RangedBehaviour();
        }
    }

    void RangedBehaviour()
    {
        float fleeTriggerRange = 2f;
        float fleeDistance = GetComponentInChildren<AttackRange>().rangeSize - fleeTriggerRange;
        bool IsInAttackRange = GetComponentInChildren<AttackRange>().IsInAttackRange;

        if (GetComponent<DetectPlayer>().playerIsDetected)
        {
            target = player.transform;
            lastKnownPlayerPosition = target.position;

            distanceToPlayer = (target.position - transform.position).magnitude;
            Debug.Log("distance to player: " + distanceToPlayer);

            if (IsInAttackRange)
            {
                if (distanceToPlayer < fleeTriggerRange)
                {
                    // Makes enemy flee directly away from player by a fixed distance
                    Vector3 directionAwayFromPlayer = -(target.position - transform.position).normalized;
                    Vector3 fleePosition = transform.position + directionAwayFromPlayer * fleeDistance;
                    agent.isStopped = false; // remove when attack implemented
                    IsFleeing = true;
                    agent.SetDestination(fleePosition);
                    Debug.Log("fleeing");
                }
                else if (!IsFleeing)
                {
                    LookAtPlayer();
                    // Insert attack here
                    agent.isStopped = true; // placeholder
                    Debug.Log("Attack");
                }
                    
            }
            else if (!IsInAttackRange && !IsFleeing)
            {
                agent.isStopped = false; // remove when attack implemented
                agent.SetDestination(target.position);
                Debug.Log("moving to attack range");
            }
        }
        else if (CheckIfDestinationReached())
        {
            LookAtPlayer();
        }

        if (IsFleeing)
        {
            if (CheckIfDestinationReached())
            {
                IsFleeing = false;
                LookAtPlayer();
            }
        }
    }

    bool CheckIfDestinationReached()
    {
        bool destinationReached = false;

        if (!agent.pathPending)
        {
            //Debug.Log(agent.remainingDistance);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    //Debug.Log("Destination reached");
                    destinationReached = true;
                    IsFleeing = false;
                }
            }
        }

        return destinationReached;
    }

    void LookAtPlayer()
    {
        // Rotates enemy to look at the last known player location
        Vector3 directionToLook = lastKnownPlayerPosition - transform.position;
        Debug.DrawLine(transform.position, directionToLook, Color.yellow);
        Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
