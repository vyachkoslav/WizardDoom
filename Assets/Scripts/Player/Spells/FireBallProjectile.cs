using UnityEngine;

public class FireBallProjectile : Projectile
{
    [SerializeField] GameObject _fireBallExplosionObject;

    private float _explosionDamage;
    private float _explosionRadius;
    private float _explosionDuration;
    private Vector3 _startDirection;

    // Initializes stats for the fireball projectile
    public void Spawn(float damage, float explosionDamage, float explosionRadius, float explosionDuration, float moveSpeed, float durationInSeconds, Vector3 startDirection)
    {
        _damage = damage;
        _explosionDamage = explosionDamage;
        _explosionRadius = explosionRadius;
        _explosionDuration = explosionDuration;
        _moveSpeed = moveSpeed;
        _durationInSeconds = durationInSeconds;
        _startDirection = startDirection;

        this.transform.position += _startDirection;
    }

    // Projectile hits something
    // TODO: apply damage to enemies
    private void OnTriggerEnter(Collider _)
    {
        _moveSpeed = 0;
        GameObject target = _.gameObject;

        if (target.layer == 7)
        {
            Debug.Log("Target hit: " + target);
            Entity entity = target.GetComponent<Entity>();
            entity?.ApplyDamage(_damage);
            GameObject fireballExplosion = Instantiate(_fireBallExplosionObject, transform.position, transform.rotation);
            fireballExplosion.GetComponent<Explosion>().Spawn(_explosionDamage, _explosionDuration, _explosionRadius);
        }
        Destroy(this.gameObject);
    }

    // Expands Move() from abstract, projectile moves forwards from player camera
    protected override void Move()
    {
        _myRigidBody.linearVelocity = _startDirection * _moveSpeed * Time.deltaTime;
    }

    // Called just before destroying this object
    private void OnDestroy()
    {
        GameObject fireballExplosion = Instantiate(_fireBallExplosionObject, transform.position, transform.rotation);
        fireballExplosion.GetComponent<Explosion>().Spawn(_explosionDamage, _explosionDuration, _explosionRadius);
    }

    private void FixedUpdate()
    {
        Move();
        KillProjectile();
    }
}