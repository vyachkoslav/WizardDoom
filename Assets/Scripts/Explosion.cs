using UnityEngine;

// Fireball xplosion, could possibly be expanded to a more generic one later?
public abstract class Explosion : MonoBehaviour
{
    protected float _durationInSeconds;
    protected SphereCollider myCollider;

    protected virtual void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
    }

    protected abstract void OnTriggerEnter(Collider target);
    
    protected virtual void Update()
    {
        Destroy(this.gameObject, _durationInSeconds);
    }
}
