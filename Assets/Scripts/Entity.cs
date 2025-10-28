using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Entity : MonoBehaviour, IEntity
{
    [SerializeField] protected float maxHealth;
    
    private float health;
    public float Health => health;

    public event Action OnHealthDecreased;
    public event Action OnHealthIncreased;
    public event Action OnDeath;

    [SerializeField] private UnityEvent onHealthDecreased;
    [SerializeField] private UnityEvent onHealthIncreased;
    [SerializeField] private UnityEvent onDeath;

    public bool takingDamage = false;

    protected virtual void Awake()
    {
        OnDeath += onDeath.Invoke;
        OnHealthDecreased += onHealthDecreased.Invoke;
        OnHealthIncreased += onHealthIncreased.Invoke;
        health = maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        Assert.IsTrue(damage >= 0);
        if (health <= 0) return;
        
        health -= damage;
        StartCoroutine(TakingDamage());
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
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        OnHealthIncreased.Invoke();
    }
    

    private IEnumerator TakingDamage()
    {
        takingDamage = true;
        yield return new WaitForSeconds(0.2f);
        takingDamage = false;
    }

}