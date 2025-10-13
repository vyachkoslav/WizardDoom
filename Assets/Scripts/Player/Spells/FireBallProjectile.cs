using UnityEngine;

public class FireBallProjectile : Projectile
{
    [SerializeField] private Collider _projectileCollider;
    [SerializeField] private Collider _explosionCollider;

    private float _explosionDamage;
    private Vector3 _startDirection;

    // Initializes stats for the fireball projectile
    public void Spawn(float damage, float explosionDamage, float moveSpeed, float durationInSeconds, Vector3 startDirection)
    {
        _damage = damage;
        _explosionDamage = explosionDamage;
        _moveSpeed = moveSpeed;
        _durationInSeconds = durationInSeconds;
        _startDirection = startDirection;

        this.transform.position += _startDirection;
    }

    // TODO fix collision detection
    private void OnCollisionEnter(Collision _)
    {
        Explode(_.gameObject, _damage);
    }

    // TODO fix collision detection
    private void OnTriggerEnter(Collider _)
    {
        Explode(_.gameObject, _explosionDamage);
    }

    // TODO fix collision detection
    private void Explode(GameObject target, float damageType)
    {
        if (target.layer == _entityLayer)
        {
            Entity entity = target.GetComponent<Entity>();
            entity.ApplyDamage(damageType);
        }
        _currentLifeInSeconds = 0f;
    }

    // Expands Move() from abstract, projectile moves forwards from player camera
    protected override void Move()
    {
        _myRigidBody.linearVelocity = _startDirection * _moveSpeed * Time.deltaTime;
        base.Move();
    }

    // TODO fix collision detection
    private void FixedUpdate()
    {
        if (_currentLifeInSeconds < _durationInSeconds)
        {
            Move();
        }
        else { Destroy(this.gameObject); }

    }
}