using UnityEngine;

// Fireball xplosion, could possibly be expanded to a more generic one later?
public class Explosion : MonoBehaviour
{
    private float _damage;
    private float _durationInSeconds;
    private SphereCollider myCollider;

    public void Spawn(float damage, float durationInSeconds, float radius)
    {
        _damage = damage;
        _durationInSeconds = durationInSeconds;
        myCollider = GetComponent<SphereCollider>();
        myCollider.radius = radius;
    }

    // Entities inside explosion radius
    // TODO: apply damage to enemies
    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;

        if (target.layer == 7)
        {
            Debug.Log("Exploded " + target);
            Entity entity = target.transform.GetComponent<Entity>();
            entity?.ApplyDamage(_damage);
        }
    }

    private void Update()
    {
        Destroy(this.gameObject, _durationInSeconds);
    }

}
