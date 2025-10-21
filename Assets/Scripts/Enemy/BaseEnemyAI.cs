using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.Events;

public abstract class BaseEnemyAI : MonoBehaviour
{
    private DetectPlayer detectPlayer;
    private Coroutine stopAttackCoroutine;
    
    protected NavMeshAgent agent { get; private set; }
    protected GameObject player { get; private set; }
    protected Entity playerEntity { get; private set; }
    protected bool playerIsDetected => detectPlayer.PlayerIsDetected;
    protected bool hasLineOfSight => detectPlayer.HasLineOfSight;
    protected bool sawPlayer;
    protected bool obstacleBlocksVision => detectPlayer.ObstacleBlocksVision;
    protected bool IsAttacking { get; private set; }
    protected float distanceToPlayer => Vector3.Distance(transform.position, player.transform.position);
    protected Vector3 lastKnownPlayerPosition;

    protected AudioSource Audio => audio;
    
    [SerializeField] private float rotationSpeed = 240f;
    [SerializeField] private float stopDuration = 1f;
    [SerializeField] private new AudioSource audio;
    
    public UnityEvent OnAttacked;

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

    protected void OnAttack()
    {
        OnAttacked.Invoke();
    }

    protected void OnEndAttacking()
    {
        Assert.IsTrue(IsAttacking);
        IsAttacking = false;
        stopAttackCoroutine ??= StartCoroutine(StopAttackingRoutine());
    }

    private IEnumerator StopAttackingRoutine()
    {
        var startTime = Time.time;
        while (!IsAttacking && Time.time - startTime < stopDuration)
        {
            yield return null;
        }
        stopAttackCoroutine = null;

        if (!IsAttacking)
            agent.isStopped = false;
    }
}
