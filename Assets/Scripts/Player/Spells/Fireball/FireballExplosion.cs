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
        GameObject target = _.gameObject;
        
        if (target.layer == 7) //TODO fix magic number later
        {
            // Ignore targets that are not enemy, such as projectiles
            if (!target.GetComponent<BaseEnemyAI>())
            {
                return;
            }

            Debug.Log("Exploded " + target);
            Entity entity = target.transform.GetComponent<Entity>();
            entity?.ApplyDamage(_damage);
        }
    }
}
