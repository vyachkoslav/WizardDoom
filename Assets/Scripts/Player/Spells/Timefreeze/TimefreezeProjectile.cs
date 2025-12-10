using Player;
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
    private void OnCollisionEnter(Collision _)
    {
        var target = _.gameObject;

        // Ignore player and other projectiles
        if (target != PlayerEntity.Instance.gameObject && !target.CompareTag("Projectile")) 
        {
            _moveSpeed = 0;
            
            GameObject timefreezeExplosion = Instantiate(_explosionObject, transform.position, transform.rotation);
            timefreezeExplosion.GetComponent<TimefreezeExplosion>().Spawn(_freezeDuration, _explosionRadius);
            Destroy(this.gameObject);
        }
    }
}