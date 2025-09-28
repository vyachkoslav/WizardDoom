using UnityEngine;
using UnityEngine.AI;


//To be added later: 
//-fleeing behaviour only happens when player is very close to enemy
//- ranged enemy moving to attack range to be added later

public class FollowPlayer : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    private Transform target;
    private string selfType;
    private Vector3 lastKnownPlayerPosition;
    private float rotationSpeed = 240f;

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
        float fleeDistance = 2f;

        if (GetComponent<DetectPlayer>().playerIsDetected)
        {
            target = player.transform;
            lastKnownPlayerPosition = target.position;

            // Makes enemy flee directly away from player by a fixed distance
            Vector3 directionAwayFromPlayer = -(target.position - transform.position).normalized;
            Vector3 fleePosition = transform.position + directionAwayFromPlayer * fleeDistance;
            agent.SetDestination(fleePosition);
        }
        else if (CheckIfDestinationReached())
        {
            // Rotates enemy to look at the last known player location
            Vector3 directionToLook = lastKnownPlayerPosition - transform.position;
            Debug.DrawLine(transform.position, directionToLook, Color.yellow);
            Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
                }
            }
        }

        return destinationReached;
    }
}
