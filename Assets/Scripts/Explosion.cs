using UnityEngine;

// Fireball xplosion, could possibly be expanded to a more generic one later?
public class Explosion : MonoBehaviour
{
    private float _damage;
    private SphereCollider myCollider;

    public void Spawn(float damage, float radius)
    {
        _damage = damage;
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
            Debug.Log("Explosion");
            // Entity entity = target.transform.GetComponent<Entity>();
            // entity.ApplyDamage(_damage);
        }

        Destroy(this.gameObject);
    }
}
