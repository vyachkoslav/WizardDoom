using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemyAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected GameObject player;
    protected Transform target;
    protected float distanceToPlayer;
    protected Vector3 lastKnownPlayerPosition;
    [SerializeField] protected float rotationSpeed = 240f;

    // Virtual-keyword allows inheriting classes to override
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    protected void LookAtPlayer()
    {
        // Rotates enemy to look at the last known player location
        Vector3 directionToLook = lastKnownPlayerPosition - transform.position;
        Debug.DrawLine(transform.position, lastKnownPlayerPosition, Color.yellow);
        Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    protected bool CheckIfDestinationReached()
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

    // Abstract-keyword forces inheriting classes to inherit this method
    protected abstract void FixedUpdate();
}
