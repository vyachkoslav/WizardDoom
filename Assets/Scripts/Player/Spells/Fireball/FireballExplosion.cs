using Player;
using UnityEngine;

public class FireballExplosion : Explosion
{
    private float _damage;

    protected override void Awake()
    {
        base.Awake();
        SoundManager.Instance.PlaySound3D("FireballExplosion", transform.position);
    }

    public void Spawn(float damage, float durationInSeconds, float radius)
    {
        _damage = damage;
        _durationInSeconds = durationInSeconds;

        myCollider.radius = radius;
    }

    // Apply damage to entities inside explosion radius
    protected override void OnTriggerEnter(Collider _)
    {
        var target = _.gameObject;
        
        // Ignore player and other projectiles
        if (target != PlayerEntity.Instance.gameObject && !target.CompareTag("Projectile")) 
        {
            target.GetComponent<Entity>()?.ApplyDamage(_damage);
        }
    }
}
