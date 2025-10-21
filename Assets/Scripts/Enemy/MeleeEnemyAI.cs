using System;
using Enemy;
using UnityEngine;

/* TODO:
- make enemy only attack when facing player and only rotate towards player when not attacking
- maybe: move entire body of PerformAttack() to BaseEnemyAI, set attack range for melee to agent.stoppingDistance and make attack range checking shared
*/
public class MeleeEnemyAI : BaseEnemyAI
{
    [SerializeField] private Attack attack;
    
    private IDisposable attackRoutine;
    private Func<Attack.AttackData> getAttackData;
    
    protected override void Start()
    {
        base.Start();
        agent.stoppingDistance = 2f;
        
        var selfEntity = GetComponent<IEntity>();
        getAttackData = () => new Attack.AttackData()
        {
            WeaponPosition = transform.position,
            WeaponForward = transform.forward,
            SelfEntity = selfEntity,
            WeaponAudio = Audio,
            TargetEntity = playerEntity,
            TargetPosition = player.transform.position,
        };
        attack.OnAttacked += OnAttacked.Invoke;
        
        // Always attack
        attackRoutine = attack.StartAttacking(getAttackData);
    }

    private void OnDestroy()
    {
        attackRoutine?.Dispose();
    }

    private void Update()
    {
        if (playerIsDetected)
        {
            lastKnownPlayerPosition = player.transform.position;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            LookAtPlayer();
        }
    }
}
