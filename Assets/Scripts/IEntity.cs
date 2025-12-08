using System;

public interface IEntity
{
    public float Health { get; }
    public event Action OnHealthDecreased;
    public event Action OnHealthIncreased;
    public event Action OnDeath;
    
    public void ApplyDamage(float damage);
    public void ApplyHealing(float healing);
}