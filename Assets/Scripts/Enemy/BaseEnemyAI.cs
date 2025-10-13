using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public abstract class BaseEnemyAI : MonoBehaviour
{
    private DetectPlayer detectPlayer;
    
    protected NavMeshAgent agent { get; private set; }
    protected GameObject player { get; private set; }
    protected Entity playerEntity { get; private set; }
    protected bool playerIsDetected => detectPlayer.playerIsDetected;
    protected bool IsAttacking { get; private set; }
    protected float distanceToPlayer => Vector3.Distance(transform.position, player.transform.position);
    protected Vector3 lastKnownPlayerPosition;
    
    [SerializeField] private float rotationSpeed = 240f;
    [SerializeField] private float stopDuration = 1f;

    // Virtual-keyword allows inheriting classes to override
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerEntity = player.GetComponent<Entity>();
        detectPlayer = GetComponent<DetectPlayer>();
    }

    protected void LookAtPlayer()
    {
        // Rotates enemy to look at the last known player location
        Vector3 directionToLook = lastKnownPlayerPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    protected bool IsDestinationReached()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.01f &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }

    protected void OnStartAttacking()
    {
        Assert.IsFalse(IsAttacking);
        IsAttacking = true;
    }

    protected void OnEndAttacking()
    {
        Assert.IsTrue(IsAttacking);
        StartCoroutine(StopAttackingRoutine());
    }

    private IEnumerator StopAttackingRoutine()
    {
        yield return new WaitForSeconds(stopDuration);
        agent.isStopped = false;
        IsAttacking = false;
    }
}
