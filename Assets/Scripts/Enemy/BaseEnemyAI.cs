using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.Events;
using Utils;

public abstract class BaseEnemyAI : MonoBehaviour
{
    private DetectPlayer detectPlayer;
    private Coroutine stopAttackCoroutine;
    
    protected NavMeshAgent agent { get; private set; }
    protected GameObject player { get; private set; }
    protected Entity playerEntity { get; private set; }
    protected Animator animator;
    protected bool playerIsDetected => detectPlayer.PlayerIsDetected;
    protected bool hasLineOfSight => detectPlayer.HasLineOfSight;
    protected bool sawPlayer;
    protected bool obstacleBlocksVision => detectPlayer.ObstacleBlocksVision;
    protected bool IsAttacking { get; private set; }
    protected float distanceToPlayer => Vector3.Distance(transform.position, player.transform.position);
    protected Vector3 lastKnownPlayerPosition = Vector3.negativeInfinity;

    
    private float rotationSpeed;
    protected Vector3 startPosition;

    [SerializeField] protected float delayBeforeAttack;
    [SerializeField] float attackAnimationDuration;
    [SerializeField] private float stopDuration = 1f;
    [SerializeField] protected RoomManager myRoom;
    
    public UnityEvent OnBeforeAttackDelay = new();
    public UnityEvent OnAttacked = new();

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = gameObject.transform.Find("Model").GetComponent<Animator>();
        playerEntity = PlayerEntity.Instance;
        player = playerEntity.gameObject;
        detectPlayer = GetComponent<DetectPlayer>();
        rotationSpeed = agent.angularSpeed;
        startPosition = transform.position;
    }

    protected void LookAtPlayer()
    {
        // Rotates enemy to look at the last known player location
        if (lastKnownPlayerPosition.sqrMagnitude > 1000000f || (lastKnownPlayerPosition - transform.position).sqrMagnitude < 0.1f)
            return;
        Vector3 directionToLook = (lastKnownPlayerPosition - transform.position).WithY(0);
        Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
        transform.rotation = targetRotation;
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
        if (agent.isActiveAndEnabled)
        {
            agent.isStopped = false;
        }
        // stopAttackCoroutine ??= StartCoroutine(StopAttackingRoutine());
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

    // Animation methods
    protected void StartAttackAnimation()
    {
        StartCoroutine(HandleAttackAnimation());
    }

    private IEnumerator HandleAttackAnimation()
    {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(attackAnimationDuration);
        animator.SetBool("isAttacking", false);
    }
}
