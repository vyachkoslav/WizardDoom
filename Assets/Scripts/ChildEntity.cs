using System;
using UnityEngine;

// Proxy entity object
public class ChildEntity : MonoBehaviour, IEntity
{
    [SerializeField] protected Entity entity;
    
    public float Health => entity.Health;

    public event Action OnHealthDecreased;
    public event Action OnHealthIncreased;
    public event Action OnDeath;

    private void OnValidate()
    {
        if (entity != null) return;
        entity = GetComponentInParent<Entity>();
    }

    protected virtual void Awake()
    {
        entity.OnDeath += OnDeath;
        entity.OnHealthDecreased += OnHealthDecreased;
        entity.OnHealthIncreased += OnHealthIncreased;
    }

    public virtual void ApplyDamage(float damage)
    {
        entity.ApplyDamage(damage);
    }

    public void ApplyHealing(float healing)
    {
        entity.ApplyHealing(healing);
    }
}