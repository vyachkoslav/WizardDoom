using System;
using UnityEngine;

namespace Enemy
{
    public abstract class Attack : ScriptableObject
    {
        public struct AttackData
        {
            public Vector3 WeaponPosition;
            public Vector3 WeaponForward;
            public AudioSource WeaponAudio;
            public Vector3 TargetPosition;
            public IEntity SelfEntity;
            public IEntity TargetEntity;
        }
        
        [SerializeField] private float delayBetweenAttacks;
        [SerializeField] private float damage;
        
        [SerializeField] private AudioClip attackSound;

        protected float DelayBetweenAttacks => delayBetweenAttacks;
        protected float Damage => damage;
        protected AudioClip AttackSound => attackSound;

        public event Action OnAttacked;

        public abstract void StartAttacking(Func<AttackData> attackData);
        public abstract void StopAttacking();

        protected void InvokeAttacked()
        {
            OnAttacked?.Invoke();
        }
    }
}