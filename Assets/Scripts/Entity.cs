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
    public bool IsDead { get; private set; } = false;
    public event Action OnDeath;

    [SerializeField] private UnityEvent onDeath;

    protected virtual void Awake()
    {
        OnDeath += onDeath.Invoke;
        health = maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        Assert.IsTrue(damage >= 0);
        if (health <= 0) return;
        
        health -= damage;
        Debug.Log(name + ": Health: " + health);
        
        // Heal player if lifesteal spell is active
        if (DataManager.Instance.IsLifeStealActive)
        {
            FindAnyObjectByType<PlayerEntity>().ApplyHealing(damage);
        }

        if (health <= 0)
            Die();
    }

    public void ApplyHealing(float healing)
    {
        Assert.IsTrue(healing >= 0);
        if (health <= 0) return;

        health += healing;
    }
    
        private void Die()
    {
        if (IsDead) return;
        IsDead = true;
        OnDeath?.Invoke();
    }
}