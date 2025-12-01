using System;
using System.Threading;
using UnityEngine;

namespace Enemy
{
    public abstract class Attack : ScriptableObject
    {
        public class CancelableAttack : IDisposable
        {
            public class Handle
            {
                public bool Paused = true;
                public Action BeforeAttackDelay;
                public Action Attacked;
            }

            private readonly CancellationTokenSource cts = new();
            public CancellationToken Token => cts.Token;

            public event Action OnBeforeAttackDelay;
            public event Action OnAttack;

            private readonly Handle handle;

            public CancelableAttack(Handle handle)
            {
                this.handle = handle;
                handle.Attacked += InvokeOnAttack;
                handle.BeforeAttackDelay += InvokeOnBeforeDelay;
            }

            private void InvokeOnAttack()
            {
                OnAttack?.Invoke();
            }

            private void InvokeOnBeforeDelay()
            {
                OnBeforeAttackDelay?.Invoke();
            }

            public void Start()
            {
                handle.Paused = false;
            }

            public void Pause()
            {
                handle.Paused = true;
            }

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


        public abstract CancelableAttack CreateAttacker(Func<AttackData> attackData, float attackDelay);
        public abstract void AttackOnce(Func<AttackData> data, CancellationToken cancellationToken);
    }
}
