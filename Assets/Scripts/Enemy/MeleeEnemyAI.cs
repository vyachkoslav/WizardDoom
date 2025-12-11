using System;
using System.Collections;
using Enemy;
using UnityEngine;
using static Enemy.Attack;

/* TODO:
- make enemy only attack when facing player and only rotate towards player when not attacking
- maybe: move entire body of PerformAttack() to BaseEnemyAI, set attack range for melee to agent.stoppingDistance and make attack range checking shared
*/
public class MeleeEnemyAI : BaseEnemyAI
{
    [SerializeField] private Attack attack;

    private CancelableAttack attacker;
    private Func<AttackData> getAttackData;

    private void Awake()
    {
        var selfEntity = GetComponent<IEntity>();
        getAttackData = () => new AttackData()
        {
            WeaponPosition = transform.position,
            WeaponForward = transform.forward,
            SelfEntity = selfEntity,
            TargetEntity = playerEntity,
            TargetPosition = player.transform.position,
        };
        attacker = attack.CreateAttacker(getAttackData, delayBeforeAttack);
        attacker.OnAttack += OnAttacked.Invoke;
        attacker.OnBeforeAttackDelay += OnBeforeAttackDelay.Invoke;
    }

    protected override void Start()
    {
        base.Start();
        agent.stoppingDistance = 2f;
    }

    private void OnEnable()
    {
        animator = gameObject.transform.Find("Model").GetComponent<Animator>();
        // Sets bool for animator
        attacker.OnBeforeAttackDelay += StartAttackAnimation;

        // Always attack
        attacker.Start();
    }

    private void OnDisable()
    {
        attacker.OnBeforeAttackDelay -= StartAttackAnimation;

        attacker?.Pause();
    }

    private void OnDestroy()
    {
        attacker?.Dispose();
    }

    private void Update()
    {
        bool isWalking = agent.velocity.sqrMagnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);

        if (playerIsDetected)
        {
            lastKnownPlayerPosition = player.transform.position;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.SetDestination(startPosition);
            LookAtPlayer();
        }
    }
}
