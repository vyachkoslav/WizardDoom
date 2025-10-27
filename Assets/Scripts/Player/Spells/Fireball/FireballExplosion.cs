using UnityEngine;

public class FireballExplosion : Explosion
{
    private float _damage;

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
        
        if (target.layer == 7)
        {
            Debug.Log("Exploded " + target);
            Entity entity = target.transform.GetComponent<Entity>();
            entity?.ApplyDamage(_damage);
        }
    }
}
