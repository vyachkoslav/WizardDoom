using UnityEngine;

//Abstract class for all kinds of physical projectiles
public abstract class Projectile : MonoBehaviour
{
    protected float _damage;
    protected float _moveSpeed;
    protected float _durationInSeconds;
    
    protected Rigidbody _myRigidBody;

    protected virtual void Awake()
    {
        _myRigidBody = GetComponent<Rigidbody>();
    }

    // Implement how projectile should move by overriding this in extending classes
    protected abstract void Move();

    // If nothing is hit, destroy projectile after duration
    protected virtual void KillProjectile()
    {
        Destroy(this.gameObject, _durationInSeconds);
    }
}
