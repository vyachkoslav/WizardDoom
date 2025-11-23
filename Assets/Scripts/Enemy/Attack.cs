using System;
using System.Threading;
using UnityEngine;

namespace Enemy
{
    public abstract class Attack : ScriptableObject
    {
        protected class CancelableAttack : IDisposable
        {
            private readonly CancellationTokenSource cts = new();
            public CancellationToken Token => cts.Token;

            public void Dispose()
            {
                cts.Cancel();
            }

            ~CancelableAttack()
            {
                Dispose();
            }
        }
        
        public struct AttackData
        {
            public Vector3 WeaponPosition;
            public Vector3 WeaponForward;
            public Vector3 TargetPosition;
            public Vector3 TargetSpeed;
            public IEntity SelfEntity;
            public IEntity TargetEntity;
        }
        
        [SerializeField] private float delayBetweenAttacks;
        [SerializeField] private float damage;
        

        protected float DelayBetweenAttacks => delayBetweenAttacks;
        protected float Damage => damage;


        public abstract IDisposable StartAttacking(Func<AttackData> attackData);
        public abstract void AttackOnce(Func<AttackData> data, CancellationToken cancellationToken);
    }
}