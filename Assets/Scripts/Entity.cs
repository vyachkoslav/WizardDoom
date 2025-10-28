using System;
using Player;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Entity : MonoBehaviour, IEntity
{
    [SerializeField] private float maxHealth;
    
    private float health;
    public float Health => health;

    public event Action OnHealthDecreased;
    public event Action OnDeath;

    [SerializeField] private UnityEvent onHealthDecreased;
    [SerializeField] private UnityEvent onDeath;

    protected virtual void Awake()
    {
        OnDeath += onDeath.Invoke;
        OnHealthDecreased += onHealthDecreased.Invoke;
        health = maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        Assert.IsTrue(damage >= 0);
        if (health <= 0) return;
        
        health -= damage;
        OnHealthDecreased.Invoke();
        
        // Heal player if lifesteal spell is active
        if (DataManager.Instance.IsLifeStealActive && this is not PlayerEntity)
        {
            PlayerEntity.Instance.ApplyHealing(damage);
        }

        if (health <= 0)
            OnDeath.Invoke();
    }

    public void ApplyHealing(float healing)
    {
        Assert.IsTrue(healing >= 0);
        if (health <= 0) return;

        health += healing;
    }

}