using Player;
using UnityEngine;

public class FireballProjectile : Projectile
{
    [SerializeField] GameObject _explosionObject;

    private float _explosionDamage;
    private float _explosionRadius;
    private float _explosionDuration;
    private Vector3 _startDirection;

    // Initializes stats for the fireball projectile
    public void Spawn(float damage, float explosionDamage, float explosionRadius, float explosionDuration, float moveSpeed, Vector3 startDirection)
    {
        _damage = damage;
        _explosionDamage = explosionDamage;
        _explosionRadius = explosionRadius;
        _explosionDuration = explosionDuration;
        _moveSpeed = moveSpeed;
        _startDirection = startDirection;

        this.transform.position += _startDirection;
    }

    // Apply damage and create explosion when target is hit
    private void OnCollisionEnter(Collision _)
    {
        var target = _.gameObject;

        // Ignore player and other projectiles
        if (target != PlayerEntity.Instance.gameObject && !target.CompareTag("Projectile"))
        {
            Debug.Log(target);
            _moveSpeed = 0;

            target.GetComponent<Entity>()?.ApplyDamage(_damage);
            GameObject fireballExplosion = Instantiate(_explosionObject, transform.position, transform.rotation);
            fireballExplosion.GetComponent<FireballExplosion>().Spawn(_explosionDamage, _explosionDuration, _explosionRadius);
            Destroy(this.gameObject);
        }
    }

    // Projectile moves forwards from player camera
    protected override void Move()
    {
        _myRigidBody.linearVelocity = _startDirection * _moveSpeed * Time.deltaTime;
    }
}