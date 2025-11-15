using UnityEngine;

public class TimefreezeProjectile : Projectile
{
    [SerializeField] GameObject _explosionObject;

    private float _explosionRadius;
    private float _explosionDuration;
    private float _freezeDuration;
    private Vector3 _startDirection;

    protected override void Awake()
    {
        base.Awake();
    }

    // Initializes stats for timefreeze projectile
    public void Spawn(float explosionRadius, float freezeDuration, float moveSpeed, Vector3 startDirection)
    {
        _explosionRadius = explosionRadius;
        _freezeDuration = freezeDuration;
        _moveSpeed = moveSpeed;
        _startDirection = startDirection;

        this.transform.position += _startDirection;
    }

    // Projectile moves forwards from main camera
    protected override void Move()
    {
        _myRigidBody.linearVelocity = _startDirection * _moveSpeed * Time.deltaTime;
    }

    // Creates explosion when target is hit
    private void OnTriggerEnter(Collider _)
    {
        GameObject target = _.gameObject;

        if (target.layer == 7) //TODO fix magic number later
        {
            // Ignore targets that are not enemy, such as projectiles
            if (!target.GetComponent<BaseEnemyAI>())
            {
                return;
            }

            _moveSpeed = 0;
            
            GameObject timefreezeExplosion = Instantiate(_explosionObject, transform.position, transform.rotation);
            timefreezeExplosion.GetComponent<TimefreezeExplosion>().Spawn(_freezeDuration, _explosionRadius);
        }
        Destroy(this.gameObject);
    }
}