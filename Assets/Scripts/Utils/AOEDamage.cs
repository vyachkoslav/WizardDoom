using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class AOEDamage : MonoBehaviour
    {
        public float Damage;
        public float DelayBetweenDamage;
        public float DelayBeforeDamage;
        public float Duration;

        [SerializeField] private bool startEnabled;
        
        private Dictionary<IEntity, float> timeDamageDealt = new();
        private HashSet<IEntity> entitiesInRange = new();
        private float enabledTime = float.MaxValue;

        private void Start()
        {
            if (startEnabled)
                EnableDamage();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<IEntity>(out var entity)) return;
            entitiesInRange.Add(entity);
            timeDamageDealt.TryAdd(entity, float.MinValue);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<IEntity>(out var entity)) return;
            entitiesInRange.Remove(entity);
        }

        private void Update()
        {
            if (Time.realtimeSinceStartup - enabledTime < DelayBeforeDamage)
                return;
            if (Time.realtimeSinceStartup - enabledTime >= Duration)
            {
                Destroy(gameObject);
                return;
            }
            
            foreach (var entity in entitiesInRange)
            {
                if (Time.realtimeSinceStartup - timeDamageDealt[entity] < DelayBetweenDamage)
                    return;

                timeDamageDealt[entity] = Time.realtimeSinceStartup;
                entity.ApplyDamage(Damage);
            }
        }

        public void SetRange(float range)
        {
            transform.localScale = new Vector3(range, transform.localScale.y, range);
        }

        public void EnableDamage()
        {
            enabledTime = Time.realtimeSinceStartup;
        }

        public void DisableDamage()
        {
            enabledTime = float.MaxValue;
        }
    }
}